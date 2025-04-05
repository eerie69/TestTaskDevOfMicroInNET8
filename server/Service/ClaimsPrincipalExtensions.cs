using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace server.Service
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var claim = user.FindFirst(ClaimTypes.NameIdentifier)
                       ?? user.FindFirst(JwtRegisteredClaimNames.Sub); // альтернативные варианты

            return claim?.Value ?? throw new InvalidOperationException(
                "User ID claim not found. Expected claim type: " +
                $"{ClaimTypes.NameIdentifier} or {JwtRegisteredClaimNames.Sub}");
        }
    }
}