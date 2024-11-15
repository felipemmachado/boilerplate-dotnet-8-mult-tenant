using System.Security.Claims;
namespace Infra.Extensions
{
    public static class HttpContextAccessorExtensions
    {
        public static Guid? TenantId(this ClaimsPrincipal claims)
        {
            try
            {
                var value = claims?.FindFirst("TenantId")?.Value;
                return Guid.Parse(value!);
            }
            catch
            {
                return null;
            }
        }
    }
}
