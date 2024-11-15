using System.Diagnostics.CodeAnalysis;
namespace Application.Common.Models.FileService
{
    [ExcludeFromCodeCoverage]
    public record FileObject(string FileUrl, FileObjectInfo Information);
}
