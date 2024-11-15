using Application.Common.Models.FileService;
using Microsoft.AspNetCore.Http;
namespace Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<BucketResponse> UploadFile(FileStream file, bool isPrivate = true);
        Task<BucketResponse> UploadFile(IFormFile file, bool isPrivate = true);
        Task<BucketResponse> UploadPublicFile(IFormFile file);
        Task<FileObject> GetObjectByKeyAsync(string fileKey, DateTime? expirationDate = null!, bool isPrivate = true);
        Task<FileObject> GetPrivateObjectByKeyAsync(string fileKey, DateTime expirationDate);
        Task<FileObject> GetPublicObjectByKeyAsync(string fileKey);
        Task DeleteObjectAsync(string fileKey);
        Task<List<FileObjectInfo>> ListAllObjectsAsync();
    }
}
