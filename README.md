# Dataverse Reader

This repository contains a .NET console application that demonstrates how to connect to and query Microsoft Dataverse using OAuth authentication and the Dataverse Web API.

## Getting Started

### Prerequisites
- .NET 6+ SDK
- Azure App Registration with Dataverse API permissions

### Configuration
1. **Set up Azure App Registration**:
   - Register an application in [Azure Portal](https://portal.azure.com)
   - Add API permission: `Dataverse API` → `user_impersonation`
   - Create a client secret

2. **Configure the application**:
   Replace these placeholders in `Program.cs`:
   ```csharp
   string tenantId = "YOUR_TENANT_ID";          // Azure AD Directory (tenant) ID
   string clientId = "YOUR_CLIENT_ID";          // Application (client) ID
   string clientSecret = "YOUR_CLIENT_SECRET";  // Client secret value
   string dataverseUrl = "YOUR_DATAVERSE_URL";  // e.g., https://yourorg.crm.dynamics.com/

 # Security Note
**⚠️ Important: The sample code contains placeholder credentials for demonstration purposes. In production environments, always:**

- Store credentials securely (e.g., Azure Key Vault, environment variables)
- Never commit secrets to version control
- Follow principle of least privilege for API permissions
# Key Features
OAuth 2.0 authentication with MSAL (Microsoft.Identity.Client).

- Customizable API queries ($select, $top, $orderby).
- Structured JSON response parsing.
- Error handling for API failures.
