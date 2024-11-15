using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Configs
{
    [ExcludeFromCodeCoverage]
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpiresInMinutes { get; set; }
    }
}
