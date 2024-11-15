using Application.Common.Constants;
using Application.Common.Interfaces;
using Infra.Extensions;
namespace API.Middleware
{
    public class TenantResolver
    {
        private const string HeaderName = Consts.IdentifyTenant;
        private readonly RequestDelegate _next;
        private readonly List<string> _routesTenantIdFree = [];

        public TenantResolver(RequestDelegate next)
        {
            _next = next;
            _routesTenantIdFree.Add("/api/v1/companies");
        }

        public async Task InvokeAsync(
            HttpContext context,
            IHttpContextAccessor contextAccessor,
            ICurrentTenantService currentTenantService)
        {
            Guid? tenancyId;
            if (contextAccessor.HttpContext!.User.TenantId() != null)
            {
                tenancyId = contextAccessor.HttpContext.User.TenantId();
                currentTenantService.SetTenant(tenancyId);
            }
            else
            {
                var tenant = contextAccessor
                    .HttpContext
                    .Request
                    .Headers
                    .FirstOrDefault(p => p.Key.Equals(HeaderName, StringComparison.CurrentCultureIgnoreCase));

                if (tenant.Value.ToString() == "")
                {
                    var routeValue = contextAccessor.HttpContext?.Request.Path.ToString() ?? "";
                    var canAccess = _routesTenantIdFree.Exists(p =>
                        routeValue.StartsWith(p, StringComparison.CurrentCultureIgnoreCase));

                    if (!canAccess) throw new UnauthorizedAccessException(ApiResponseMessages.TenantIdRequired);
                }
                else
                {
                    tenancyId = Guid.Parse(tenant.Value!);
                    currentTenantService.SetTenant(tenancyId);
                }
            }
            await _next(context);
        }
    }
}
