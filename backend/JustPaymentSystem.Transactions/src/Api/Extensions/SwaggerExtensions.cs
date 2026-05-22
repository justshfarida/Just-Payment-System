using Microsoft.OpenApi;

namespace Api.Extensions;

public static class SwaggerExtensions
{
    
    extension(IServiceCollection service)
    {
        public void AddSwaggerWithAuth(IConfiguration configuration)
        {
            var keycloakAuthority = configuration["Keycloak:Authority"]!;
            var keycloakClientId = configuration["Keycloak:ClientId"]!;
            service.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Demo API",
                    Version = "v1"
                });

                // Define the OAuth 2.0 security scheme
                options.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/auth"),
                            TokenUrl = new Uri($"{keycloakAuthority}/protocol/openid-connect/token"),
                            Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect scope" },
                    { "profile", "User profile" }
                }
                        }
                    }
                });

                options.AddSecurityRequirement(doc => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(nameof(SecuritySchemeType.OAuth2), doc),
            []
        }
    });
            });
        }
    }
}
