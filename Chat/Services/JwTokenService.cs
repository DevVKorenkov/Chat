using Chat.Config;
using Chat.Models;
using Chat.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Services;

public class JwTokenService : IJwTokenService
{
    public async Task<string> CreateAsync(AppIdentityUser tokenSubject)
    {
        return await Task.Run(CreateToken);

        string CreateToken()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, tokenSubject.Id),
                new Claim(ClaimTypes.Name, tokenSubject.UserName),
            };
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingsManager.AppSettings["JWT:Secret"]));
            var credentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(
                issuer: SettingsManager.AppSettings["Jwt:ValidIssuer"],
                audience: SettingsManager.AppSettings["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenString = jwtSecurityTokenHandler.WriteToken(tokenOptions);

            return tokenString;
        }

    }
}
