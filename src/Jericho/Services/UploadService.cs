namespace Jericho.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Jericho.Common;
    using Jericho.Providers;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Http;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;

    public class UploadService : IUploadService
    {
        private CloudStorageAccount storageAccount;

        private CloudBlobClient blobClient;

        private CloudBlobContainer imagesContainer;

        private CloudBlobContainer gifsContainer;

        public UploadService()
        {
            this.ConfigureAzureStorageServices();
            this.ConfigureContainersAsync();
        }

        public async Task<ServiceResult<string>> UploadAsync(IFormCollection collection)
        {
            Ensure.Argument.NotNull(collection, nameof(collection));

            var fileToUpload = collection.Files.First();
            var uploadedPath = await this.UploadFileAsync(fileToUpload);

            if (string.IsNullOrEmpty(uploadedPath))
            {
                var error = new Error("mimetypemismatch", "Invalid File");
                return new ServiceResult<string>(false, new List<Error> { error });
            }

            return new ServiceResult<string>(true, uploadedPath);
        }

        private async Task<string> UploadFileAsync(IFormFile file)
        {
            string filePath;
            if (file.FileName.IsGif())
            {
                filePath = await this.UploadGifAsync(file);
                return $"{this.gifsContainer.Uri.AbsoluteUri}/{filePath}";
            }

            if (file.FileName.IsImage())
            {
                filePath = await this.UploadImageAsync(file);
                return $"{this.imagesContainer.Uri.AbsoluteUri}/{filePath}";
            }

            return string.Empty;
        }

        private async Task<string> UploadImageAsync(IFormFile file)
        {
            var fileName = file.FileName.GetUniqueFileName();
            var imagesBlob = this.imagesContainer.GetBlockBlobReference(fileName);
            using (var fileStream = file.OpenReadStream())
            {
                await imagesBlob.UploadFromStreamAsync(fileStream);
            }

            return fileName;
        }

        private async Task<string> UploadGifAsync(IFormFile file)
        {
            var fileName = file.FileName.GetUniqueFileName();
            var gifsBlob = this.gifsContainer.GetBlockBlobReference(fileName);
            using (var fileStream = file.OpenReadStream())
            {
                await gifsBlob.UploadFromStreamAsync(fileStream);
            }

            return fileName;
        }

        private void ConfigureAzureStorageServices()
        {
            this.storageAccount = CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=jerichodevblob;AccountKey=NeyP6OCO9PlrtD4Bqifo5Ii4MbKAM4B8nm/nSpQ9+KXvhc7BjMCnBjqhkVw22jSNP+/il5XD6IRfELZdjQjAVg==");
            this.blobClient = this.storageAccount.CreateCloudBlobClient();
        }

        private async void ConfigureContainersAsync()
        {
            await this.ConfigureImagesContainerAsync();
            await this.ConfigureGifsContainerAsync();
        }

        private async Task ConfigureImagesContainerAsync()
        {
            this.imagesContainer = this.blobClient.GetContainerReference("images");

            await this.imagesContainer.CreateIfNotExistsAsync();
            await this.imagesContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }

        private async Task ConfigureGifsContainerAsync()
        {
            this.gifsContainer = this.blobClient.GetContainerReference("gifs");

            await this.gifsContainer.CreateIfNotExistsAsync();
            await this.gifsContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
        }
    }
}
