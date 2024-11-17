using Application.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
namespace API.Configs
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfigs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // add JWT Authentication
                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                var tenancyId = new OpenApiSecurityScheme
                {
                    Name = Consts.IdentifyTenant,
                    Description = "Tenant Id",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "ApiKey"
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityDefinition(tenancyId.Reference.Id, tenancyId);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme, new List<string>()
                    }
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        tenancyId, new List<string>()
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    Description = "Api da youin multi tenancy, para utilizar a api é necessário adicionar o tenancyId no 'Authorize'."
                });
            });

        }
    }
}
