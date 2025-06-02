# Desk Booking Application

A full-stack application for managing workspace bookings in an office environment.

## Technologies Used

### Frontend
- Angular 17
- TypeScript
- Tailwind CSS
- Angular Material
- RxJS
- NgRx (State Management)

### Backend
- .NET Core 8.0
- C#
- Entity Framework Core
- PostgreSQL
- Azure App Service

## Project Structure
```
desk-booking/
├── DeskBooking.API/          # .NET Core Web API
├── DeskBooking.Core/         # Core domain models and interfaces
├── DeskBooking.Application/  # Application services and DTOs
├── DeskBooking.Infrastructure/ # Infrastructure implementation
├── DeskBooking.UI/           # Angular frontend with Tailwind CSS
├── deploy-azure.ps1         # Azure deployment script
└── deploy-azure.sh          # Alternative Azure deployment script
```

## Prerequisites
- .NET Core 8.0 SDK
- Node.js 18 or higher
- Angular CLI
- PostgreSQL
- Azure CLI (for deployment)
- PowerShell (for Windows deployment)

## Local Development Setup

1. Clone the repository:
```bash
git clone <repository-url>
cd desk-booking
```

2. Configure the database connection in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=deskbooking;Username=postgres;Password=your_password"
  }
}
```

3. Run database migrations:
```bash
dotnet ef database update --project DeskBooking.Infrastructure --startup-project DeskBooking.API
```

4. Start the backend:
```bash
cd DeskBooking.API
dotnet run
```

5. Start the frontend:
```bash
cd DeskBooking.UI
npm install
ng serve
```

6. Access the application:
- Frontend: http://localhost:4200
- Backend API: http://localhost:5000

## Deployment to Azure

### Prerequisites
- Azure account
- Azure CLI installed
- PowerShell (for Windows)

### Deployment Steps

1. **Install Azure CLI**
   - [Download and install Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli)

2. **Run the deployment script:**
```powershell
.\deploy-azure-portal.ps1
```

The script will:
- Build the application
- Create a deployment package
- Provide instructions for Azure Portal deployment
- Open Azure Portal in your browser

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
az appservice plan create --name desk-booking-plan --resource-group desk-booking-rg --sku B1 --is-linux
```

4. **Create a Web App:**
```bash
az webapp create --resource-group desk-booking-rg --plan desk-booking-plan --name desk-booking-app --runtime "DOTNETCORE|8.0"
```

5. **Configure environment variables:**
```bash
az webapp config appsettings set --resource-group desk-booking-rg --name desk-booking-app --settings ASPNETCORE_ENVIRONMENT=Production
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