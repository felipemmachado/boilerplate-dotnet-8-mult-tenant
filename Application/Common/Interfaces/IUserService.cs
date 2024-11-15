namespace Application.Common.Interfaces
{
    public interface IUserService
    {
        Guid UserId { get; }
        string[] Roles { get; }
        bool HaveAllRoles(string[] rolesCheck);
        bool HaveSomeRole(string[] rolesCheck);
        bool HaveSomeRole(string roleCheck);
    }
}
