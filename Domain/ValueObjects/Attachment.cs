using Domain.Common;
namespace Domain.ValueObjects
{
    public class Attachment(string resourceName, string resourceKey, bool isPrivate = true) : ValueObject
    {
        public string ResourceName { get; set; } = resourceName;
        public string ResourceKey { get; set; } = resourceKey;
        public bool IsPrivate { get; set; } = isPrivate;

        public void UpdateUrl(string url)
        {
            ResourceKey = url;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ResourceKey;
            yield return ResourceName;
        }
    }
}
