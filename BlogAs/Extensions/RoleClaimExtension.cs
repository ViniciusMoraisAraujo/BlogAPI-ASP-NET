using System.Security.Claims;
using BlogAs.Models;

namespace BlogAs.Extensions;

public static class RoleClaimExtension
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>{ new (ClaimTypes.Name, user.Email)};
        result.AddRange(user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));
        
        return result;
    }
}