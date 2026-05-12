# Inventory API -Deployed by containerization via Azure and Docker 
A robust inventory API built with .NET 9, Docker and Azure. The goal of this project was to make a functioning API ready for deployment by migrating secrets to a secure location (Azure Key Vault). 

## Features 
* Docker
* Azure Key Vault
* Azure Container Registry
* Azure Container Apps
* DevOps
* CI/CD workflows

## Tech Stack 
* .NET 9
* Azure
* Docker

## Run the project
### Container build & run locally
To verify the containerized application locally, follow these steps:

1. **Build the image** Run the following command from the solution root directory:
   `docker build -t inventory-api -f CloudNativeInventory.Api/Dockerfile .`
   
2. **Run the container** Start the container by mapping the host port 5210 to the internal rootless port 8080. We also set the environment to Development for the local session:
`docker run -it --rm -p 5210:8080 -e ASPNETCORE_ENVIRONMENT=Development inventory-api`

3. **Verify the endpoints** Open your browser or an HTTP client and navigate to:
[http://localhost:5210/api/inventory] (to check data)
[http://localhost:5210/api/inventory/system/verify-integration] (to check configuration)

4. **Stop the application** Press CTRL + C in the terminal to stop and automatically remove the container.

## CI/CD pipeline
The pipeline in divided in three logic jobs to make sure that only validate and secure code reach production. 

### Triggers
Triggers on push and pull-request into master branch. Branch protection demands status checks (tests) to pass before merge is allowed.

### Steps 
1. **test:**
* installs .NET 9 SDK 
* Caching based on .csproj-hash to speed upp `dotnet restore` (no significant timewin in this project)
* Run `dotnet build` and `dotnet test` in release configuration. If a test fails, the pipeline stops imedietly (fast fail).

2. **build-and-push:**
* Authenticates with Azure Container Registry (ACR) using Github Secrets. 
* Builds the Docker image using multi-stage build for optimization.
* Builds docker-image with unique tags `${{ github.sha }}` for traceability and `latest` for current deployment.
* pushes image to Azure Container Registry (ACR).

3. **deploy:**
* Updates Azure Container Apps with newly build image from above.


## Architecture Decision Record (ADR) 
Motivation for the following decisions can be found in [[0001-infrastructure-and-security.md](./CloudNativeInventory.Api/Docs/adr/001-infrastructure-and-security.md)]
* Compute Platform: Azure Container Apps (ACA)
* Secrets: Azure Key Vault and Managed Identity
* Identity: Managed Identity
* Container Security: Rootless and Multi-stage
* Pipeline Design: Github Actions with caching

## Verify Production
When the app is deployed, security configuration can be verified at the following endpoint: 
[https://ca-inventory-api-dev.delightfulpebble-2fdba7cd.polandcentral.azurecontainerapps.io/api/inventory/system/verify-integration]
* Expected result: `200 OK` with status `"Secured"` 
This confirms that the aplication has successfully authenticated using Managed Identity and replaced local configuration with the real secret from Azure Key Vault. 

   
