using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Configs
{
    [ExcludeFromCodeCoverage]
    public class EmailConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public EmailConfigTemplate Template { get; set; } = null!;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public abstract class EmailConfigTemplate
    {
        public string ResetPasswordEmail { get; set; } = string.Empty;
    }
}
