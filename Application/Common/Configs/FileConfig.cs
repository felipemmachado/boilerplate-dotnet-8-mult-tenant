using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Configs
{
    [ExcludeFromCodeCoverage]
    public class FileConfig
    {
        public string AccessKeyId { get; set; } = string.Empty;
        public string BucketName { get; set; } = string.Empty;
        public string BucketUrl { get; set; } = string.Empty;
        public string ServiceUrl { get; set; } = string.Empty;
        public string AwsSecretKey { get; set; } = string.Empty;
    }
}
