using Domain.Common;
namespace Domain.Entities
{
    public class User(string name, string email, string password, IList<string> roles) : TenancyBase
    {
        public string Name { get; protected set; } = name;
        public string Email { get; protected set; } = email.ToLower();
        public string Password { get; protected set; } = password;
        public bool ForceChangePassword { get; protected set; } = true;
        public DateTime? DisabledAt { get; protected set; }
        public DateTime? FirstAccess { get; protected set; }
        public DateTime? LastAccess { get; protected set; }
        public IList<string> Roles { get; protected set; } = roles;

        public void Update(string name)
        {
            Name = name;
        }

        public void UpdateEmail(string email)
        {
            Email = email;
        }

        public void SetForceChangePassword(bool force) { ForceChangePassword = force; }

        public bool IsDisabled()
        {
            return DisabledAt != null;
        }

        public void Disabled()
        {
            DisabledAt = DateTime.UtcNow;
        }

        public void Enabled()
        {
            DisabledAt = null;
        }

        public void UpdatePassword(string newPassword)
        {
            Password = newPassword;
        }

        public void UpdateLastAccess(DateTime when)
        {
            LastAccess = when;
        }

        public void UpdateFirstAccess(DateTime when)
        {
            FirstAccess = when;
        }
    }
}
