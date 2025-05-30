FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["DeskBooking.API/DeskBooking.API.csproj", "DeskBooking.API/"]
COPY ["DeskBooking.Core/DeskBooking.Core.csproj", "DeskBooking.Core/"]
COPY ["DeskBooking.Infrastructure/DeskBooking.Infrastructure.csproj", "DeskBooking.Infrastructure/"]
COPY ["DeskBooking.Application/DeskBooking.Application.csproj", "DeskBooking.Application/"]
RUN dotnet restore "DeskBooking.API/DeskBooking.API.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet build "DeskBooking.API/DeskBooking.API.csproj" -c Release -o /app/build
RUN dotnet publish "DeskBooking.API/DeskBooking.API.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DeskBooking.API.dll"] 