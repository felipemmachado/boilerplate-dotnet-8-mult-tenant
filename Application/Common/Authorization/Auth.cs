using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Authorization
{
    [ExcludeFromCodeCoverage]
    public class Auth(string role, string description, IEnumerable<string> rolesNeeds)
    {
        public readonly string Role = role;
        public string Description = description;
        public IEnumerable<string> RolesNeeds = rolesNeeds;
    }
}
