using Application.Common.Constants;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace Infra.Services
{
    public class UserService : IUserService
    {
        public UserService(IHttpContextAccessor accessor)
        {
            var userId = accessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var companyId = accessor.HttpContext?.User.FindFirst(Consts.IdentifyTenant)?.Value;

            if (companyId == null || userId == null)
                return;

            CompanyId = new Guid(companyId);
            UserId = new Guid(userId);
            Roles = accessor.HttpContext?.User.FindAll(ClaimTypes.Role)?.Select(p => p.Value).ToArray() ?? [];
        }
        public Guid CompanyId { get; set; }

        public Guid UserId { get; set; }
        public string[] Roles { get; set; } = [];

        public bool HaveAllRoles(string[] rolesCheck)
        {
            return !(from role in rolesCheck from r in Roles where r != role select role).Any();
        }

        public bool HaveSomeRole(string[] rolesCheck)
        {
            foreach (var userRole in rolesCheck)
            {
                return HaveSomeRole(userRole);
            }

            return false;
        }

        public bool HaveSomeRole(string roleCheck)
        {
            return Roles.Any(r => r == roleCheck);
        }
    }
}
