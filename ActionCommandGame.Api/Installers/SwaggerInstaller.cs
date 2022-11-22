using ActionCommandGame.Api.Installers.Abstractions;
using Microsoft.OpenApi.Models;

namespace ActionCommandGame.Api.Installers
{
    public class SwaggerInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Action Command Game API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWT Authorization using the Bearer Token scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                };
                options.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirementScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                };
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        securityRequirementScheme, new string[] { }
                    }
                };
                options.AddSecurityRequirement(securityRequirement);
			});
        }
    }
}
