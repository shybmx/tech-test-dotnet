# Constraints and Missed Implementations

The following points outline the aspects that were not implemented due to the constraints:

- **Automated acceptance tests (AC tests)** were not implemented, which limits the ability to verify the system's behavior against business requirements.
- **Installers** were not configured at startup, reducing flexibility and testability of the application.
- **Concurrency considerations**, such as implementing an event-sourcing pattern, were not addressed, which could lead to data consistency issues in high-load scenarios.
- **CI/CD pipeline** was not created, meaning there is no automated process for building, testing, and deploying the application.
- **Logging** was not implemented, making it difficult to trace issues or understand the application's runtime behavior.
- **Monitoring** was not set up, leaving the system without visibility into its health and performance.
- **Alerts** were not configured, meaning potential issues cannot be proactively identified and addressed.
- **Performance testing** was not conducted, so the system's behavior under load remains unverified.
- **Provisioning** was not implemented, which means the necessary infrastructure or resources were not automatically set up for the application.