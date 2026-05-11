# Inventory API -Deployed by containering via Azure and Docker 
A robust inventory API built with .NET 9, Docker and Azure. The goal of this project was to make a functioning API redy for deploy by removing secrets to secure location (key vault). 

## Features 
* Docker
* Azure Key Vault
* Azure container registry
* Azure container Apps
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
`http://localhost:5210/api/inventory (to check data)`
`http://localhost:5210/api/inventory/system/verify-integration (to check configuration)`

4. **Stop the application** Press CTRL + C in the terminal to stop and automatically remove the container.

## CI/CD pipeline
### Triggers
Triggers on push and pull-request into master branch. 

### Steps 

   
