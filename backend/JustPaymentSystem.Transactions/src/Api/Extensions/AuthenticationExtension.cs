using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class AuthenticationExtension
{
    extension(IServiceCollection services)
    {
        public void AddAuthentication(IConfiguration configuration, bool isDevelopment = true)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MetadataAddress = configuration["Keycloak:MetadataAddress"]!;
                options.Audience = configuration["Keycloak:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["Keycloak:Issuer"]
                };

                // Required for HTTP in development (Keycloak uses HTTP by default in dev mode)
                options.RequireHttpsMetadata = isDevelopment;
            });
        }
    }
}
