using System.Diagnostics.CodeAnalysis;
using System.Net;
namespace Application.Common.Models.FileService
{
    [ExcludeFromCodeCoverage]
    public class BucketResponse(HttpStatusCode httpStatusCode, string fileKey)
    {
        public bool Success { get; private set; } = (int)httpStatusCode <= 299;
        public string FileKey { get; private set; } = fileKey;
    }
}
