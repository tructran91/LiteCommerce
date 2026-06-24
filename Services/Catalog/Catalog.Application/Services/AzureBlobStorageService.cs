using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Configuration;

namespace Catalog.Application.Services
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly string? _cdnUrl;
        private readonly int _sasExpiryHours;

        public AzureBlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["Storage:Azure:ConnectionString"];
            var containerName = configuration["Storage:Azure:ContainerName"];
            _cdnUrl = configuration["Storage:Azure:CdnUrl"];
            _sasExpiryHours = int.TryParse(configuration["Storage:Azure:SasExpiryHours"], out var hours) ? hours : 24;

            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        private static string GetBlobName(string fileName, string? subFolder)
        {
            return string.IsNullOrEmpty(subFolder)
                ? fileName
                : $"{subFolder.Replace("\\", "/")}/{fileName}";
        }

        public string GetFileUrl(string fileName, string? subFolder = null)
        {
            var blobName = GetBlobName(fileName, subFolder);

            if (!string.IsNullOrEmpty(_cdnUrl))
            {
                return $"{_cdnUrl.TrimEnd('/')}/{blobName}";
            }

            var blobClient = _containerClient.GetBlobClient(blobName);
            return GetAuthenticatedUri(blobClient, _sasExpiryHours).ToString();
        }

        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName, string? subFolder = null)
        {
            var blobName = GetBlobName(fileName, subFolder);
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(mediaBinaryStream, overwrite: true);
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.DeleteIfExistsAsync();
        }

        public async Task MoveFileAsync(string fileName, string sourceSubFolder, string destSubFolder)
        {
            var sourceBlobName = GetBlobName(fileName, sourceSubFolder);
            var destBlobName = GetBlobName(fileName, destSubFolder);

            var sourceBlobClient = _containerClient.GetBlobClient(sourceBlobName);
            var destBlobClient = _containerClient.GetBlobClient(destBlobName);

            await destBlobClient.StartCopyFromUriAsync(GetAuthenticatedUri(sourceBlobClient, 1));
            await sourceBlobClient.DeleteIfExistsAsync();
        }

        public async Task DeleteFolderAsync(string subFolder)
        {
            var prefix = subFolder.Replace("\\", "/").TrimEnd('/') + "/";
            await foreach (var blobItem in _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, prefix, CancellationToken.None))
            {
                var blobClient = _containerClient.GetBlobClient(blobItem.Name);
                await blobClient.DeleteIfExistsAsync();
            }
        }

        public async Task MoveFolderContentAsync(string sourceSubFolder, string destSubFolder)
        {
            var sourcePrefix = sourceSubFolder.Replace("\\", "/").TrimEnd('/') + "/";
            var destPrefix = destSubFolder.Replace("\\", "/").TrimEnd('/') + "/";

            await foreach (var blobItem in _containerClient.GetBlobsAsync(BlobTraits.None, BlobStates.None, sourcePrefix, CancellationToken.None))
            {
                var relativePath = blobItem.Name[sourcePrefix.Length..];
                var destBlobName = $"{destPrefix}{relativePath}";

                var sourceBlobClient = _containerClient.GetBlobClient(blobItem.Name);
                var destBlobClient = _containerClient.GetBlobClient(destBlobName);

                await destBlobClient.StartCopyFromUriAsync(GetAuthenticatedUri(sourceBlobClient, 1));
                await sourceBlobClient.DeleteIfExistsAsync();
            }
        }

        private Uri GetAuthenticatedUri(BlobClient blobClient, int expiryHours)
        {
            if (blobClient.CanGenerateSasUri)
            {
                var sasBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(expiryHours))
                {
                    Resource = "b"
                };
                return blobClient.GenerateSasUri(sasBuilder);
            }
            return blobClient.Uri;
        }
    }
}
