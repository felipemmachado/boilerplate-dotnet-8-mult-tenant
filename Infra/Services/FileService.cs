using Amazon.S3;
using Amazon.S3.Model;
using Application.Common.Configs;
using Application.Common.Interfaces;
using Application.Common.Models.FileService;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
namespace Infra.Services
{
    [ExcludeFromCodeCoverage]
    public class FileService : IFileService
    {
        private readonly AmazonS3Client _amazonS3Client;
        private readonly FileConfig _fileConfig;

        public FileService(IOptions<FileConfig> fileConfig)
        {
            _fileConfig = fileConfig.Value ?? throw new ArgumentNullException(nameof(fileConfig));
            _amazonS3Client = new AmazonS3Client(
                _fileConfig.AccessKeyId,
                _fileConfig.AwsSecretKey,
                new AmazonS3Config
                {
                    ServiceURL = _fileConfig.ServiceUrl,
                    ForcePathStyle = false
                }
            );
        }

        public async Task DeleteObjectAsync(string fileKey)
        {
            await _amazonS3Client.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _fileConfig.BucketName,
                Key = fileKey
            });
        }

        public async Task<FileObject> GetObjectByKeyAsync(
            string fileKey,
            DateTime? expirationDate = null!,
            bool isPrivate = true)
        {
            expirationDate ??= DateTime.Now.AddHours(4);

            return isPrivate ? await GetPrivateObjectByKeyAsync(fileKey, expirationDate.Value) : await GetPublicObjectByKeyAsync(fileKey);
        }

        public async Task<FileObject> GetPrivateObjectByKeyAsync(string fileKey, DateTime expirationDate)
        {
            var storageObject = await _amazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _fileConfig.BucketName,
                Key = fileKey
            });

            var objectByKey = new FileObject(
                _amazonS3Client.GetPreSignedURL(
                    new GetPreSignedUrlRequest
                    {
                        BucketName = _fileConfig.BucketName,
                        Key = fileKey,
                        Expires = expirationDate
                    }),
                new FileObjectInfo(
                    storageObject.Key,
                    storageObject.ResponseStream.Length,
                    storageObject.LastModified));

            return objectByKey;
        }



        public async Task<FileObject> GetPublicObjectByKeyAsync(string fileKey)
        {
            var storageObject = await _amazonS3Client.GetObjectAsync(new GetObjectRequest
            {
                BucketName = _fileConfig.BucketName,
                Key = fileKey
            });

            var objectByKey = new FileObject(
                _fileConfig.BucketUrl + fileKey,
                new FileObjectInfo(
                    storageObject.Key,
                    storageObject.ResponseStream.Length,
                    storageObject.LastModified));

            return objectByKey;
        }

        public async Task<List<FileObjectInfo>> ListAllObjectsAsync()
        {
            var storageObjects = await _amazonS3Client.ListObjectsV2Async(
                new ListObjectsV2Request
                {
                    BucketName = _fileConfig.BucketName
                });

            return storageObjects.S3Objects.Select(sObj
                => new FileObjectInfo(
                    sObj.Key,
                    sObj.Size,
                    sObj.LastModified)).ToList();
        }

        public async Task<BucketResponse> UploadFile(FileStream file, bool isPrivate = true)
        {
            using var newMemoryStream = new MemoryStream();

            await file.CopyToAsync(newMemoryStream);

            var fileExtension = Path.GetExtension(file.Name);
            var fileKey = $"{Guid.NewGuid()}{fileExtension}";

            var response = await _amazonS3Client.PutObjectAsync(new PutObjectRequest
            {
                InputStream = newMemoryStream,
                Key = fileKey,
                BucketName = _fileConfig.BucketName,
                CannedACL = isPrivate ? S3CannedACL.Private : S3CannedACL.PublicRead
            });

            return new BucketResponse(response.HttpStatusCode, fileKey);
        }

        public async Task<BucketResponse> UploadFile(IFormFile file, bool isPrivate = true)
        {
            using var newMemoryStream = new MemoryStream();

            await file.CopyToAsync(newMemoryStream);

            var fileExtension = Path.GetExtension(file.FileName);
            var fileKey = $"{Guid.NewGuid()}{fileExtension}";

            var response = await _amazonS3Client.PutObjectAsync(new PutObjectRequest
            {
                InputStream = newMemoryStream,
                Key = fileKey,
                BucketName = _fileConfig.BucketName,
                CannedACL = isPrivate ? S3CannedACL.Private : S3CannedACL.PublicRead
            });

            return new BucketResponse(response.HttpStatusCode, fileKey);
        }

        public async Task<BucketResponse> UploadPublicFile(IFormFile file)
        {
            return await UploadFile(file, false);
        }

        public string GetUrl(string fileName)
        {
            return $"{_fileConfig.ServiceUrl}{_fileConfig.BucketName}/{fileName}";
        }
    }
}
