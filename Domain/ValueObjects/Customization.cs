using Domain.Common;
namespace Domain.ValueObjects
{
    public class Customization(string primaryColor, string buttonColor, string tabBrowserTitle) : ValueObject
    {
        public readonly string PrimaryColor = primaryColor;
        public readonly string ButtonColor = buttonColor;
        public readonly string TabBrowserTitle = tabBrowserTitle;
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PrimaryColor;
            yield return ButtonColor;
            yield return TabBrowserTitle;
        }
    }
}
