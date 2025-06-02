#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Logging function
log() {
    echo -e "${GREEN}[$(date '+%Y-%m-%d %H:%M:%S')] $1${NC}"
}

error() {
    echo -e "${RED}[$(date '+%Y-%m-%d %H:%M:%S')] ERROR: $1${NC}"
    exit 1
}

warning() {
    echo -e "${YELLOW}[$(date '+%Y-%m-%d %H:%M:%S')] WARNING: $1${NC}"
}

# Configuration
RESOURCE_GROUP="desk-booking-rg"
LOCATION="westeurope"
APP_SERVICE_PLAN="desk-booking-plan"
APP_NAME="desk-booking-$(date +%s)"
SKU="B1"
RUNTIME="DOTNETCORE|8.0"

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    error "Azure CLI is not installed. Please install it first."
fi

# Check if user is logged in
if ! az account show &> /dev/null; then
    error "Please login to Azure first using 'az login'"
fi

# Start deployment
log "Starting deployment process..."

# Create resource group if it doesn't exist
if ! az group show --name $RESOURCE_GROUP &> /dev/null; then
    log "Creating resource group $RESOURCE_GROUP..."
    az group create --name $RESOURCE_GROUP --location $LOCATION || error "Failed to create resource group"
else
    log "Resource group $RESOURCE_GROUP already exists"
fi

# Create app service plan if it doesn't exist
if ! az appservice plan show --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP &> /dev/null; then
    log "Creating app service plan $APP_SERVICE_PLAN..."
    az appservice plan create --name $APP_SERVICE_PLAN --resource-group $RESOURCE_GROUP --sku $SKU --is-linux || error "Failed to create app service plan"
else
    log "App service plan $APP_SERVICE_PLAN already exists"
fi

# Check if app name is available
log "Checking if app name $APP_NAME is available..."
if ! az webapp show --name $APP_NAME --resource-group $RESOURCE_GROUP &> /dev/null; then
    log "Creating web app $APP_NAME..."
    az webapp create --name $APP_NAME --resource-group $RESOURCE_GROUP --plan $APP_SERVICE_PLAN --runtime $RUNTIME || error "Failed to create web app"
else
    error "Web app $APP_NAME already exists"
fi

# Configure web app settings
log "Configuring web app settings..."
az webapp config set --resource-group $RESOURCE_GROUP --name $APP_NAME --linux-fx-version "DOTNETCORE|8.0" || error "Failed to configure web app settings"

# Configure database connection string
log "Configuring database connection string..."
az webapp config connection-string set \
    --resource-group $RESOURCE_GROUP \
    --name $APP_NAME \
    --settings DefaultConnection="Server=tcp:deskbooking.database.windows.net,1433;Initial Catalog=DeskBooking;Persist Security Info=False;User ID=your_username;Password=your_password;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" \
    --connection-string-type PostgreSQL || error "Failed to configure database connection string"

# Configure app settings
log "Configuring app settings..."
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings ASPNETCORE_ENVIRONMENT=Production || error "Failed to configure ASPNETCORE_ENVIRONMENT"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITE_RUN_FROM_PACKAGE=1 || error "Failed to configure WEBSITE_RUN_FROM_PACKAGE"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings SCM_DO_BUILD_DURING_DEPLOYMENT=true || error "Failed to configure SCM_DO_BUILD_DURING_DEPLOYMENT"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITE_HTTPLOGGING_RETENTION_DAYS=7 || error "Failed to configure WEBSITE_HTTPLOGGING_RETENTION_DAYS"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITE_NODE_DEFAULT_VERSION=~18 || error "Failed to configure WEBSITE_NODE_DEFAULT_VERSION"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings DOTNET_ENVIRONMENT=Production || error "Failed to configure DOTNET_ENVIRONMENT"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITE_WEBSOCKETS_ENABLED=1 || error "Failed to configure WEBSITE_WEBSOCKETS_ENABLED"
az webapp config appsettings set --resource-group $RESOURCE_GROUP --name $APP_NAME --settings WEBSITE_HTTPLOGGING_ENABLED=1 || error "Failed to configure WEBSITE_HTTPLOGGING_ENABLED"

# Build and publish the application
log "Building and publishing the application..."
dotnet publish DeskBooking.API/DeskBooking.API.csproj -c Release -o ./publish --self-contained true -r linux-x64 || error "Failed to build application"

# Create deployment package using PowerShell
log "Creating deployment package..."
powershell -Command "Compress-Archive -Path './publish/*' -DestinationPath './deploy.zip' -Force" || error "Failed to create deployment package"

# Deploy to Azure
log "Deploying to Azure..."
az webapp deploy --resource-group $RESOURCE_GROUP --name $APP_NAME --src-path deploy.zip --type zip --async true || error "Failed to deploy to Azure"

# Wait for deployment to complete
log "Waiting for deployment to complete..."
sleep 60

# Clean up
log "Cleaning up..."
rm -rf publish deploy.zip

# Get the website URL
WEBSITE_URL="https://$APP_NAME.azurewebsites.net"
log "Deployment completed successfully!"
log "Your application is available at: $WEBSITE_URL"

# Open the website in the default browser
log "Opening website in your default browser..."
if [[ "$OSTYPE" == "msys" || "$OSTYPE" == "win32" ]]; then
    start $WEBSITE_URL
elif [[ "$OSTYPE" == "darwin"* ]]; then
    open $WEBSITE_URL
else
    xdg-open $WEBSITE_URL
fi
