using backend.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace backend.Services
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            var account = new Account(
                config.Value.CloudName ?? throw new ArgumentNullException(nameof(config.Value.CloudName)),
                config.Value.ApiKey ?? throw new ArgumentNullException(nameof(config.Value.ApiKey)),
                config.Value.ApiSecret ?? throw new ArgumentNullException(nameof(config.Value.ApiSecret))
            );

            _cloudinary = new Cloudinary(account);
        }

        // Method to upload an image
        public async Task<ImageUploadResult> UploadImageAsync(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file)); // Ensure file is provided

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    // Add transformations if needed
                    // Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Check if the upload was successful
                if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new ApplicationException("Image upload failed.");
                }
            }
            else
            {
                throw new ArgumentException("File is empty.");
            }

            return uploadResult;
        }

        // Method to delete an image using its public ID
        public async Task<DeletionResult> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId)) throw new ArgumentException("Public ID cannot be null or empty.");

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed to delete image with Public ID {publicId}.");
            }

            return result;
        }
    }
}
