using Azure.Storage.Blobs;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TrainingPlan.API.Application.Common.Services
{
    public interface IAzureBlobService
    {
        Task<string> UploadContentToBlobStorage(string title, string type, string data);
    }

    public class AzureBlobService : IAzureBlobService
    {
        public async Task<string> UploadContentToBlobStorage(string title, string type, string data)
        {
            const string connectionString = "<Your_Connection_String>";
            const string containerName = "<Your_Container_Name>";

            // Remove spaces and non-alphanumeric characters from the title
            string sanitizedTitle = Regex.Replace(title, @"[^a-zA-Z0-9]", "");

            // Combine the sanitized title with the type to form the file name
            string fileName = $"{sanitizedTitle}.{type}";

            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Create the container if it does not exist
            await containerClient.CreateIfNotExistsAsync();

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Debug.WriteLine($"Uploading to Blob storage as blob:\n\t {blobClient.Uri}");

            // Convert Base64 string to byte array
            byte[] videoBytes = Convert.FromBase64String(data);

            // Open the file and upload its data
            using MemoryStream memoryStream = new MemoryStream(videoBytes);
            await blobClient.UploadAsync(memoryStream, true);

            Debug.WriteLine("Upload complete");

            // Return the Blob URL
            return blobClient.Uri.ToString();
        }
    }
}
