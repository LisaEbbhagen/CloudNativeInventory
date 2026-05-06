# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
WORKDIR /src

# Install the .NET SDK
COPY *.sln ./
COPY CloudNativeInventory.Api/*.csproj ./CloudNativeInventory.Api/
COPY CloudNativeInventory.Tests/*.csproj ./CloudNativeInventory.Tests/
RUN dotnet restore

# Copy the remaining source code and publish the application
COPY . .
RUN dotnet publish CloudNativeInventory.Api/CloudNativeInventory.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Create a non-root user and set permissions
RUN adduser --disabled-password --gecos "" appuser \
	&& chown -R appuser:appuser /app
USER appuser
 
# Copy the published application from the build stage
COPY --from=build /app/publish .
 
# Expose the application port
EXPOSE 8080

# Set the entry point for the application
ENTRYPOINT ["dotnet", "CloudNativeInventory.Api.dll"]