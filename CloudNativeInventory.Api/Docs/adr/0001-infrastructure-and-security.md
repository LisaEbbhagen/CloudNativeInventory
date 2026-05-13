## Infrastructure and Security Decisions

### 1.Compute Platform: Azure Container Apps
* **Decision:** Use Azure Container Apps as the compute platform for the application.
* **Rationale:** Container Apps supports containerized applications and microservices, including full Docker integration. It also supports scale-to-zero, ensuring that services incur no costs when not in use. Container Apps provides isolated environments and supports Managed Identities, which eliminates the need to manage sensitive credentials or "secrets" within the code to access other Azure resources, such as our Container Registry (ACR), significantly reducing the attack surface.
* **Consequences:** Significant cost savings due to scale-to-zero capabilities. The services can handle traffic spikes automatically without manual scaling. A modern, container-based architecture. Easier to maintain than hosting Kubernetes, while remaining powerful enough for microservice architectures. Enhanced security through Managed Identities, which eliminates the need for hardcoded credentials when accessing Azure resources like ACR.

### 2. Secrets: Azure Key Vault
* **Decision:** All application secrets are moved from `appsettings.json` to Azure Key Vault. Includes a health check/endpoint to ensure the application gets secrets from Key Vault and not from `appsettings.json`. 
* **Rationale:** Storing secrets in code is a major security risk. Key Vault allows us to inject secrets at runtime. The health check ensures that the application is correctly configured to use Key Vault and not fallback to `appsettings.json`. 
* **Consequences:** Increased security. Requires `Azure.Identity` and `DefaultAzureCredential` in the code. 

### 3. Identity: Managed Identity
* **Decision:** Use Managed Identity for the Container App to access Key Vault and Azure Container Registry.
* **Rationale:** Eliminates the need to store credentials in code, reducing the risk of credential leakage. RBAC (Role-Based Access Control) is used with the "Key Vault Secrets User" role
* **Consequences:** Follows best practices for secure access to Azure resources and principle of least privilege. Requires proper RBAC configuration in Azure.

### 4. Container Security: Rootless & Multi-stage
* **Decision:** Running the container as `USER app` on port 8080 using multi-stage builds. 
* **Rationale:** A multi-stage build ensures the production image only contains the runtime, not the SDK or source code. Running as rootless ensures that even if the app is compromised, the attacker lacks administrative privileges within the container. Port 8080 is used to avoid running on privileged ports (below 1024), which require root access.
* **Consequences:** Significantly reduced attack surface. Requires specific port mapping in the infrastructure. 

### 5. Pipeline Design: Github Actions with caching
* **Decision:** Use Github Actions for a three-stage CI/CD pipeline (Test->Build->Deploy) with caching to speed up builds. Also apply branch protection rules to the `master` branch, requiring PR reviews and successful pipeline runs before merging. Use Github SHA as image-tag for traceability.
* **Rationale:** Github Actions provides a flexible and powerful CI/CD platform. Automates quility control. If test fail, the deplyment is blocked. Caching based on `.csproj` hash reduces build times.
* **Consequences:** Faster feedback loops and guaranteed traceability, reduced build times, and more efficient CI/CD processes. Requires proper configuration of caching strategies and secrets management. By using Github SHA as image-tag, we ensure traceability of which code changes are in which container image, enhancing debugging and accountability.