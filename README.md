# Desk Booking Application

A full-stack application for managing workspace bookings in an office environment.

## Technologies Used

### Frontend
- Angular 17
- TypeScript
- SCSS
- Angular Material
- RxJS

### Backend
- Spring Boot
- Java
- Spring Data JPA
- PostgreSQL
- Docker & Docker Compose

## Project Structure
```
desk-booking/
├── desk-booking-ui/          # Angular frontend
├── desk-booking-backend/     # Spring Boot backend
├── docker-compose.yml        # Docker configuration
├── .env                      # Environment variables
└── deploy-azure.sh          # Azure deployment script
```

## Prerequisites
- Docker and Docker Compose
- Node.js and npm
- Java 17 or higher
- Azure CLI

## Local Development Setup

1. Clone the repository:
```bash
git clone <repository-url>
cd desk-booking
```

2. Create a `.env` file in the root directory:
```env
DATABASE_USERNAME=postgres
DATABASE_PASSWORD=postgres
DATABASE_NAME=deskbooking
DATABASE_HOST=db
NODE_ENV=development
```

3. Start the application using Docker Compose:
```bash
docker-compose up
```

4. Access the application:
- Frontend: http://localhost:4200
- Backend API: http://localhost:8080

## Deployment to Azure

### Prerequisites
- Azure account
- Azure CLI installed
- Docker and Docker Compose installed
- Git repository with your project

### Deployment Steps

1. **Install Azure CLI**
   - [Download and install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

2. **Make the deployment script executable:**
```bash
chmod +x deploy-azure.sh
```

3. **Run the deployment script:**
```bash
./deploy-azure.sh
```

The script will:
- Log in to Azure
- Create a resource group
- Create an App Service Plan
- Create a Web App
- Deploy your Docker Compose application
- Configure environment variables
- Open your application in the browser

### Manual Deployment (Alternative)

If you prefer to deploy manually:

1. **Login to Azure:**
```bash
az login
```

2. **Create a resource group:**
```bash
az group create --name desk-booking-rg --location westeurope
```

3. **Create an App Service Plan:**
```bash
az appservice plan create --name desk-booking-plan --resource-group desk-booking-rg --is-linux
```

4. **Create a Web App:**
```bash
az webapp create --resource-group desk-booking-rg --plan desk-booking-plan --name desk-booking-app --multicontainer-config-type compose --multicontainer-config-file docker-compose.yml
```

5. **Configure environment variables:**
```bash
az webapp config appsettings set --resource-group desk-booking-rg --name desk-booking-app --settings @.env
```

## API Documentation

### Workspace Endpoints
- `GET /api/workspaces` - List all workspaces
- `GET /api/workspaces/{id}` - Get workspace details
- `POST /api/workspaces` - Create new workspace
- `PUT /api/workspaces/{id}` - Update workspace
- `DELETE /api/workspaces/{id}` - Delete workspace

### Booking Endpoints
- `GET /api/bookings` - List all bookings
- `GET /api/bookings/{id}` - Get booking details
- `POST /api/bookings` - Create new booking
- `PUT /api/bookings/{id}` - Update booking
- `DELETE /api/bookings/{id}` - Cancel booking

## Contributing
1. Fork the repository
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License
This project is licensed under the MIT License.

## Contact
For any questions or concerns, please open an issue in the repository. 