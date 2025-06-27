using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogAs.Models;
using Microsoft.IdentityModel.Tokens;

namespace BlogAs.Services;

public class TokenService
{
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity( new Claim[]
            { 
                new (ClaimTypes.Name, "vinicius"),
                new (ClaimTypes.Role, "user"),
                new (ClaimTypes.Role , "admin")
            }),
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature), 
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}