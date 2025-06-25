using System.Security.Claims;
using EcoinverCultive_api.Models.Dto;


namespace EcoinverCultive_api.Services
{
    public interface ITokenUserService
    {
        UserInfo GetUserFromToken(ClaimsPrincipal principal);
    }

    public class TokenUserService : ITokenUserService
    {
        public UserInfo GetUserFromToken(ClaimsPrincipal principal)
        {
            foreach (var claim in principal.Claims)
            {
                Console.WriteLine($"CLAIM: {claim.Type} = {claim.Value}");
            }
            var username = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            var userId = principal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var role = principal.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
            var name = principal.FindFirst("name")?.Value;
            var lastName = principal.FindFirst("lastName")?.Value;
            var roleLevel = principal.FindFirst("RoleLevel")?.Value;

            return new UserInfo
            {
                Id = userId,
                Username = username,
                Role = role,
                name=name,
                lastName=lastName,
                RoleLevel = int.Parse(roleLevel ?? "0")
            };
        }
    }
}
