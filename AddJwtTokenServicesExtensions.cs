using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Servirform.Models.JWT;

namespace Servirform;
public static class AddJwtTokenServicesExtensions
{
    public static void AddJwtTokenServices(this IServiceCollection services, IConfiguration configuration)
    {
        // add settings of our jwtsettings
        var bindJwtSetting = new JwtSettings();
        configuration.Bind("JsonWebTokenKeys", bindJwtSetting);

        // add a Singleton JwtSettings
        services.AddSingleton(bindJwtSetting);

        // add authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidateIssuerSigningKey = bindJwtSetting.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSetting.IssuerSigningKey)),
                ValidIssuer = bindJwtSetting.ValidIssuer,
                ValidateAudience = bindJwtSetting.ValidateAudience,
                ValidAudience = bindJwtSetting.ValidAudience,
                RequireExpirationTime = bindJwtSetting.RequiereExpirationTime,
                ClockSkew = TimeSpan.FromDays(1)
            };
        });
    }
}
