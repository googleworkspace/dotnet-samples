using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Http;
using Google.Apis.Services;
using NUnit.Framework;


namespace DriveV2SnippetsTest
{

    class ErrorHandler : IHttpUnsuccessfulResponseHandler
    {
        public Task<bool> HandleResponseAsync(HandleUnsuccessfulResponseArgs args)
        {
            Console.WriteLine(args.Request);
            Console.WriteLine(args.Response);
            return Task.FromResult(false);
        }

    };

    public abstract class BaseTest
    {
        protected DriveService service;
        protected HashSet<string> filesToDelete = new HashSet<string>();

    

        public BaseTest()
        {
            this.service = BuildService();
            service.HttpClient.MessageHandler
                .AddUnsuccessfulResponseHandler(new ErrorHandler());
        }


        public DriveService BuildService()
        {
            var credential = GoogleCredential
                .GetApplicationDefaultAsync()
                .Result;
            if (credential.IsCreateScopedRequired)
            {
                var scopes = new[]
                {
                    DriveService.Scope.Drive,
                    DriveService.Scope.DriveAppdata
                };
                credential = credential.CreateScoped(scopes);
            }
            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        [SetUp]
        public void Setup()
        {
            filesToDelete.Clear();
        }

        [TearDown]
        public void CleanupFiles()
        {
            foreach (var id in filesToDelete)
            {
                try
                {
                    this.service.Files.Delete(id);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to delete file " + id);
                }
            }
        }

        protected void DeleteFileOnCleanup(string id)
        {
            filesToDelete.Add(id);
        }

        protected string CreateTestDocument(string filePath)
        {
            var fileMetadata = new Google.Apis.Drive.v2.Data.File();
            fileMetadata.Title = "Test Document";
            fileMetadata.MimeType = "application/vnd.google-apps.document";
            using (var stream = new System.IO.FileStream(filePath,
                System.IO.FileMode.Open))
            {
                var request = this.service.Files.Insert(
                      fileMetadata, stream, "text/plain");
                request.Fields = "id, mimeType";
                request.Upload();
                var file = request.ResponseBody;
                if (file != null)
                {
                    DeleteFileOnCleanup(file.Id);
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
            using (var stream = new System.IO.FileStream(filePath,
                System.IO.FileMode.Open))
            {
                var request = this.service.Files.Insert(
                      fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
                var file = request.ResponseBody;
                DeleteFileOnCleanup(file.Id);
                return file.Id;
            }
        }
    }
}