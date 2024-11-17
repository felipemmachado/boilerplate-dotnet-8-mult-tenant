using Domain.Common;
using Domain.ValueObjects;
using System.Linq.Expressions;
namespace Domain.Entities
{
    public class Company : EntityBase
    {
        public Company()
        {
            Expression.Empty();
        }

        public Company(string url,
            string companyName,
            string tradingName,
            string document)
        {
            Url = url;
            CompanyName = companyName;
            TradingName = tradingName;
            Document = document;
        }

        public string? Url { get; protected set; } = string.Empty;
        public string? CompanyName { get; protected set; } = string.Empty;
        public string? TradingName { get; protected set; } = string.Empty;
        public Attachment? Logo { get; private set; }
        public Customization? Customization { get; private set; } = null;
        public string? Document { get; protected set; }
        public DateTime? DisabledAt { get; protected set; }
        public bool IsActive => DisabledAt == null;
        public IList<User> Users { get; protected set; } = [];

        public void Update(string url, string companyName, string tradingName, string document)
        {
            Url = url;
            CompanyName = companyName;
            TradingName = tradingName;
            Document = document;
        }

        public void Disable()
        {
            DisabledAt = DateTime.UtcNow;
        }

        public void Enable()
        {
            DisabledAt = null;
        }

        public void SetLogo(Attachment logo)
        {
            Logo = logo;
        }

        public void RemoveLogo()
        {
            Logo = null;
        }

        public void AddUser(User user)
        {
            Users.Add(user);
        }

        public void DisableUser(Guid? id)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            user?.Disabled();
        }

        public void UpdateUser(Guid? id, string name)
        {
            var user = Users.FirstOrDefault(x => x.Id == id);
            user?.Update(name);
        }

        public void UpdateCustomization(Customization? customization)
        {
            Customization = customization;
        }
    }
}
