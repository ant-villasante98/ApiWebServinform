using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Servirform.Models.JWT;

namespace Servirform.Helpers;



public static class JwtHelpers
{
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, Guid id)
    {
        List<Claim> claims = new List<Claim> {
            new Claim(ClaimTypes.Email, userAccounts.UserEmail),
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MM ddd dd yyyy HH:mm:ss tt")),
            new Claim(ClaimTypes.Role, userAccounts.Rol.ToString()),
        };

        return claims;
    }
    public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, out Guid id)
    {
        id = Guid.NewGuid();
        return GetClaims(userAccounts, id);
    }
    public static UserTokens GetTokenKey(UserTokens model, JwtSettings jwtSettings)
    {
        Console.WriteLine($"Uso de las funciones del Helper {nameof(JwtHelpers)}");
        try
        {
            var userToken = new UserTokens();
            if (model == null)
            {
                throw new ArgumentException(nameof(model));
            }

            // optener clave secreta -- Nota el IssuerSigningKey debe tener 16 o mas caracteres
            var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);

            Guid id;

            // Expira en 1 dia
            DateTime expireTime = DateTime.UtcNow.AddDays(1);

            //Validacion de nuestro token
            userToken.Validity = expireTime.TimeOfDay;

            // Generar la clave secreta
            var jwbToken = new JwtSecurityToken(
                issuer: jwtSettings.ValidIssuer,
                audience: jwtSettings.ValidAudience,
                claims: GetClaims(model, out id),
                notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                expires: new DateTimeOffset(expireTime).DateTime,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            );
            userToken.Token = new JwtSecurityTokenHandler().WriteToken(jwbToken);
            userToken.UserEmail = model.UserEmail;
            userToken.GuidId = id;
            userToken.Rol = model.Rol;

            return userToken;

        }
        catch (Exception ex)
        {
            throw new Exception("Error Generating the JWT ", ex);
        }
    }

}
