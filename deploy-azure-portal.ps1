# Configuration
$config = @{
    ResourceGroup = "desk-booking-rg"
    Location = "westeurope"
    AppServicePlan = "desk-booking-plan"
    AppName = "desk-booking-$(Get-Date -Format 'yyyyMMddHHmmss')"
    Sku = "B1"
    Runtime = "DOTNETCORE|8.0"
    DatabaseServer = "deskbooking.database.windows.net"
    DatabaseName = "DeskBooking"
    DatabaseUser = "your_username"
    DatabasePassword = "your_password"
}

# Colors for output
$RED = [System.ConsoleColor]::Red
$GREEN = [System.ConsoleColor]::Green
$YELLOW = [System.ConsoleColor]::Yellow

# Logging functions
function Write-Log {
    param($Message, $Color = $GREEN)
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] $Message" -ForegroundColor $Color
}

function Write-Error {
    param($Message)
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] ERROR: $Message" -ForegroundColor $RED
    exit 1
}

function Write-Warning {
    param($Message)
    Write-Host "[$(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')] WARNING: $Message" -ForegroundColor $YELLOW
}

# Build and package the application
function Build-Application {
    Write-Log "Building application..."
    
    # Clean previous builds
    if (Test-Path "./publish") {
        Remove-Item -Path "./publish" -Recurse -Force
    }
    if (Test-Path "./deploy.zip") {
        Remove-Item -Path "./deploy.zip" -Force
    }

    # Build the application
    dotnet publish DeskBooking.API/DeskBooking.API.csproj -c Release -o ./publish --self-contained true -r linux-x64
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to build application"
    }

    # Create deployment package
    Write-Log "Creating deployment package..."
    Compress-Archive -Path './publish/*' -DestinationPath './deploy.zip' -Force
    if ($LASTEXITCODE -ne 0) {
        Write-Error "Failed to create deployment package"
    }
}

# Generate deployment instructions
function Write-DeploymentInstructions {
    Write-Log "Deployment Instructions:" $YELLOW
    Write-Log "1. Open Azure Portal (https://portal.azure.com)" $YELLOW
    Write-Log "2. Create a new Resource Group named '$($config.ResourceGroup)' in '$($config.Location)'" $YELLOW
    Write-Log "3. Create a new App Service Plan:" $YELLOW
    Write-Log "   - Name: $($config.AppServicePlan)" $YELLOW
    Write-Log "   - OS: Linux" $YELLOW
    Write-Log "   - Region: $($config.Location)" $YELLOW
    Write-Log "   - Pricing tier: $($config.Sku)" $YELLOW
    Write-Log "4. Create a new Web App:" $YELLOW
    Write-Log "   - Name: $($config.AppName)" $YELLOW
    Write-Log "   - Publish: Code" $YELLOW
    Write-Log "   - Runtime stack: .NET 8" $YELLOW
    Write-Log "   - Operating System: Linux" $YELLOW
    Write-Log "5. Configure the following Application Settings:" $YELLOW
    Write-Log "   - ASPNETCORE_ENVIRONMENT=Production" $YELLOW
    Write-Log "   - WEBSITE_RUN_FROM_PACKAGE=1" $YELLOW
    Write-Log "   - SCM_DO_BUILD_DURING_DEPLOYMENT=true" $YELLOW
    Write-Log "   - WEBSITE_HTTPLOGGING_RETENTION_DAYS=7" $YELLOW
    Write-Log "   - WEBSITE_NODE_DEFAULT_VERSION=~18" $YELLOW
    Write-Log "   - DOTNET_ENVIRONMENT=Production" $YELLOW
    Write-Log "   - WEBSITE_WEBSOCKETS_ENABLED=1" $YELLOW
    Write-Log "   - WEBSITE_HTTPLOGGING_ENABLED=1" $YELLOW
    Write-Log "6. Configure the following Connection String:" $YELLOW
    Write-Log "   - Name: DefaultConnection" $YELLOW
    Write-Log "   - Type: PostgreSQL" $YELLOW
    Write-Log "   - Value: Server=tcp:$($config.DatabaseServer),1433;Initial Catalog=$($config.DatabaseName);Persist Security Info=False;User ID=$($config.DatabaseUser);Password=$($config.DatabasePassword);MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" $YELLOW
    Write-Log "7. Deploy the application using the generated deploy.zip file" $YELLOW
    Write-Log "8. The application will be available at: https://$($config.AppName).azurewebsites.net" $YELLOW
}

# Main execution
try {
    Write-Log "Starting deployment preparation..."
    
    # Build and package the application
    Build-Application
    
    # Generate deployment instructions
    Write-DeploymentInstructions
    
    # Open Azure Portal
    Write-Log "Opening Azure Portal..."
    Start-Process "https://portal.azure.com"
    
    Write-Log "Deployment package has been created: deploy.zip" $GREEN
    Write-Log "Please follow the instructions above to deploy your application." $GREEN
}
catch {
    Write-Error "An error occurred: $_"
}
finally {
    # Clean up
    if (Test-Path "./publish") {
        Remove-Item -Path "./publish" -Recurse -Force
    }
} 