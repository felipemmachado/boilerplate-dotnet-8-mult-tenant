using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Models.FileService
{
    [ExcludeFromCodeCoverage]
    public record FileObjectInfo(string Key, long Size, DateTime LastModified);
}
