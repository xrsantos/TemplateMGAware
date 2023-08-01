using MGAware.Database.Context;
using MGAware.Database.DTO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MGAware.Security.JWT.Extensions;

public static class JwtSecurityExtension
{
    public static IServiceCollection AddJwtSecurity(
        this IServiceCollection services,
        TokenConfigurationsDto tokenConfigurations)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<MGADBContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<AccessManager>();
        services.AddScoped<JwtSecurityExtensionEvents>();

        var signingConfigurations = new SigningConfigurations(
            tokenConfigurations.SecretJwtKey!);
        services.AddSingleton(signingConfigurations);

        services.AddSingleton(tokenConfigurations);

        services.AddAuthentication(authOptions =>
        {
            authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(bearerOptions =>
        {
            var paramsValidation = bearerOptions.TokenValidationParameters;
            paramsValidation.IssuerSigningKey = signingConfigurations.Key;
            paramsValidation.ValidAudience = tokenConfigurations.Audience;
            paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
            paramsValidation.ValidateIssuerSigningKey = true;
            paramsValidation.ValidateLifetime = true;
            paramsValidation.ClockSkew = TimeSpan.Zero;
            bearerOptions.EventsType = typeof(JwtSecurityExtensionEvents);
        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().Build());
        });

        return services;
    }
}