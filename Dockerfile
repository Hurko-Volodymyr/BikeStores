# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the entire solution and restore dependencies
COPY . .

# Publish the application
RUN dotnet publish -c Release -o /app

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy the published output from the build stage
COPY --from=build /app .

# Copy the appsettings.json file
COPY ./BikeStores.Host/appsettings.json .

# Set the entry point for the container
ENTRYPOINT ["dotnet", "BikeStores.dll"]
