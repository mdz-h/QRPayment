using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Oxxo.Cloud.Security.Application.Administrators.Commands.ChangePassword;
using Oxxo.Cloud.Security.Application.Auth.Queries;
using Oxxo.Cloud.Security.Application.Common.Email;
using Oxxo.Cloud.Security.Application.Common.Interfaces;
using Oxxo.Cloud.Security.Application.Common.Security;
using Oxxo.Cloud.Security.Application.Device.Queries.Get;
using Oxxo.Cloud.Security.Application.ExternalApps.Queries.Get;
using Oxxo.Cloud.Security.Domain.Consts;
using Oxxo.Cloud.Security.Infrastructure.ExternalServices;
using Oxxo.Cloud.Security.Infrastructure.Persistence;
using Oxxo.Cloud.Security.Infrastructure.Persistence.Interceptors;
using Oxxo.Cloud.Security.Infrastructure.Services;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;
public static class ConfigureServices
{

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Environment.GetEnvironmentVariable(GlobalConstantHelpers.JWT_KEY);
        var issuer = Environment.GetEnvironmentVariable(GlobalConstantHelpers.JWT_ISSUER);
        var audience = Environment.GetEnvironmentVariable(GlobalConstantHelpers.JWT_AUDIENCE);
        var publicKey = Environment.GetEnvironmentVariable(GlobalConstantHelpers.PUBLIC_KEY_CRYPTOGRAPHY);
        var privateKey = Environment.GetEnvironmentVariable(GlobalConstantHelpers.PRIVATE_KEY_CRYPTOGRAPHY);

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidAudience = audience,
                ValidIssuer = issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key ?? string.Empty)),
                ClockSkew = TimeSpan.Zero,

            };
        });
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();


        services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Environment.GetEnvironmentVariable(GlobalConstantHelpers.SECURITY_CONNECTION) ?? string.Empty,
                 builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddTransient<ILogService, LogService>();
        services.AddSingleton<ICryptographyService>(new CryptographyService(publicKey ?? string.Empty, privateKey ?? string.Empty));
        services.AddSingleton<ITokenGenerator>(new TokenGenerator(key ?? string.Empty, issuer ?? string.Empty, audience ?? string.Empty));
        services.AddTransient<IAuthenticateQuery, AuthenticateQuery>();
        services.AddTransient<IDeviceQueryGet, DeviceQueryGet>();
        services.AddTransient<IExternalAppsQueryGet, ExternalAppsQueryGet>();
        services.AddTransient<ISecurity, Security>();
        services.AddTransient<IChangePassword, ChangePassword>();
        services.AddTransient<IEmail, Email>();
        services.AddTransient(typeof(IExternalService), typeof(ExternalService));
        services.AddHttpClient();
        return services;

    }
}
