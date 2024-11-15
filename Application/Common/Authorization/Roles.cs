using Application.Common.Constants;
using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Authorization
{
    [ExcludeFromCodeCoverage]
    public static class Roles
    {
        public const string Administrator = nameof(Administrator);
        private static readonly Auth AdministratorRole = new Auth(
            Administrator, 
            ApiResponseMessages.AllowAccessLikeAdministrator, 
            []);

        public static readonly IReadOnlyCollection<Auth> AllRoles = [AdministratorRole];

        public static bool Exists(string role)
        {
            var list = AllRoles.Select(x => x.Role).ToList();
            return list.Contains(role);
        }
    }
}
