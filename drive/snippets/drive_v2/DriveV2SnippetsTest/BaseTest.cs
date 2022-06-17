using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using NUnit.Framework;

namespace DriveV2SnippetsTest
{
 public abstract class BaseTest
    {
        protected DriveService service;

        public DriveService BuildService()
        {
            /* Load pre-authorized user credentials from the environment.
             TODO(developer) - See https://developers.google.com/identity for
             guides on implementing OAuth2 for your application. */
            GoogleCredential credential = GoogleCredential.GetApplicationDefault();
            var scopes = new[]
            {
                DriveService.Scope.Drive,
                DriveService.Scope.DriveAppdata
            };
            credential = credential.CreateScoped(scopes);

            // Create Drive API service.
            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Snippets"
            });

            return service;
        }

        [SetUp]
        public void Setup()
        {
            service = BuildService();
        }

        protected void DeleteFileOnCleanup(string id)
        {
            service.Files.Delete(id).Execute();
        }

        protected string CreateTestDocument(string filePath)
        {
            var fileMetadata = new Google.Apis.Drive.v2.Data.File();
            fileMetadata.Title = "Test Document";
            fileMetadata.MimeType = "application/vnd.google-apps.document";
            using (var stream = new FileStream(filePath,
                       FileMode.Open))
            {
                var request = service.Files.Insert(
                    fileMetadata, stream, "text/plain");
                request.Fields = "id, mimeType";
                request.Upload();
                var file = request.ResponseBody;
                if (file != null)
                {
                    return file.Id;
                }
                else
                {
                    return null;
                }
            }
        }

        protected string CreateTestBlob(string filePath)
        {
            var fileMetadata = new Google.Apis.Drive.v2.Data.File();
            fileMetadata.Title = "photo.jpg";
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var request = service.Files.Insert(
                    fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
                var file = request.ResponseBody;
                return file.Id;
            }
        }
    }
}