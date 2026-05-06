## Decision
I have selected Azure Container Apps as the compute platform for this project.

## Rationale:

- Container Apps supports containerized applications and microservices, including full Docker integration.
- Container Apps supports scale-to-zero, ensuring that services incur no costs when not in use.
- It requires no server administration and is simpler to manage than Kubernetes, yet more powerful than App Service for container-based workloads.

## Azure App Service was deselected because:

- App Service cannot scale to zero, leading to unnecessary costs during periods of low traffic.
- Autoscaling is more limited and not as event-driven as in Container Apps.
- It is less flexible for microservices running as standalone containers.

## Consequences
### Positive Consequences:

- Significant cost savings due to scale-to-zero capabilities.
- The services can handle traffic spikes automatically without manual scaling.
- A modern, container-based architecture.
- Easier to maintain than hosting Kubernetes, while remaining powerful enough for microservice architectures.

### Negative Consequences:

- Docker knowledge is required, along with an understanding of container-based workflows.
- Cold start latency may occur when services scale up from zero, which can impact user experience during initial requests.
- Local development environments must support container execution (e.g., Docker Desktop or equivalent).
- A learning curve exists regarding KEDA-based scaling and Container Apps configuration.