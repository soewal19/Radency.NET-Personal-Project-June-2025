# Desk Booking Application

A modern desk booking application built with Angular and ASP.NET Core, following clean architecture principles.

## Project Structure

### Backend (.NET Core)
- DeskBooking.API - Web API project
- DeskBooking.Core - Domain entities and interfaces
- DeskBooking.Application - Application business logic
- DeskBooking.Infrastructure - Data access and external services

### Frontend (Angular)
- Angular application with TypeScript
- Responsive design using Tailwind CSS
- Component-based architecture

## Prerequisites

- .NET 6.0 SDK or later
- Node.js 16.x or later
- PostgreSQL 14 or later
- Docker and Docker Compose
- Angular CLI

## Getting Started

### Backend Setup

1. Navigate to the backend directory:
```bash
cd DeskBooking.API
```

2. Update the connection string in `appsettings.json` if needed

3. Run the application:
```bash
dotnet run
```

### Frontend Setup

1. Navigate to the frontend directory:
```bash
cd desk-booking-ui
```

2. Install dependencies:
```bash
npm install
```

3. Run the development server:
```bash
ng serve
```

### Docker Setup

1. Build and run using Docker Compose:
```bash
docker-compose up --build
```

## Features

- Workspace type management (Open Space, Private Rooms, Meeting Rooms)
- Real-time availability checking
- Booking management (create, edit, delete)
- Calendar integration
- Responsive design
- User authentication and authorization

## API Documentation

Swagger documentation is available at `/swagger` when running the backend application.

## Development

### Backend Development
- Follow Clean Architecture principles
- Use async/await for all I/O operations
- Implement proper validation using FluentValidation
- Use AutoMapper for object mapping
- Follow REST API best practices

### Frontend Development
- Use Angular best practices
- Implement lazy loading for modules
- Use async pipe where possible
- Follow component-based architecture
- Implement proper error handling

## Testing

### Backend Tests
```bash
dotnet test
```

### Frontend Tests
```bash
ng test
```

## Deployment

The application is configured for deployment to Azure using GitHub Actions CI/CD pipeline.

## Contributing

1. Create a feature branch from `develop`
2. Make your changes
3. Create a pull request to `develop`
4. Ensure all tests pass
5. Get code review approval

## License

This project is licensed under the MIT License.

## To Run an application using docker

```bash
docker-compose up --build
```

---

## To Run an application locally

1. Сконфигурируйте `.env` файл (пример ниже)
2. Установите зависимости и запустите приложение

```env
DATABASE_USERNAME=postgres
DATABASE_PASSWORD=postgres
DATABASE_NAME=deskbooking
DATABASE_HOST=localhost
NODE_ENV=development
```

```bash
# Установка зависимостей
npm install

# Запуск в режиме разработки
npm run start

# Запуск в watch-режиме
npm run start:dev

# Запуск в production-режиме
npm run start:prod
```

---

## Пример docker-compose файла

```yaml
version: '3.8'
services:
  db:
    image: postgres:15
    restart: always
    environment:
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres
      POSTGRES_DB: deskbooking
    ports:
      - "5432:5432"
    volumes:
      - db_data:/var/lib/postgresql/data

  backend:
    build: ./backend
    env_file:
      - ./backend/.env
    depends_on:
      - db
    ports:
      - "5000:5000"

  frontend:
    build: ./desk-booking-ui
    ports:
      - "4200:80"
    depends_on:
      - backend

volumes:
  db_data:
```

---

## Пример .env файла для backend

```env
DATABASE_USERNAME=postgres
DATABASE_PASSWORD=postgres
DATABASE_NAME=deskbooking
DATABASE_HOST=db
NODE_ENV=production
```

---

## Ссылка на деплой

[Демо (пример)](https://your-deployed-app-link.com)

---

## Скриншоты интерфейса

![Workspaces](./screenshots/workspaces.png)
![Booking Form](./screenshots/booking-form.png)
![My Bookings](./screenshots/my-bookings.png) 