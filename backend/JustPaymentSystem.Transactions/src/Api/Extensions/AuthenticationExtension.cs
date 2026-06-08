using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;

public static class AuthenticationExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddAuthentication(IConfiguration configuration, bool isDevelopment = true)
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

                options.RequireHttpsMetadata = false;
            });
            return services;
        }
    }
}
