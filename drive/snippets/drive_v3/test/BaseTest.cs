using System;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3beta;
using Google.Apis.Drive.v3beta.Data;
using Google.Apis.Services;
using NUnit.Framework;
using Google;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using Google.Apis.Http;
using System.Collections.Generic;

namespace dotnet
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

        static BaseTest()
        {
            ApplicationContext.RegisterLogger(
                new Google.Apis.Logging.Log4NetLogger());
        }

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

        protected string CreateTestDocument()
        {
            var fileMetadata = new File();
            fileMetadata.Name = "Test Document";
            fileMetadata.MimeType = "application/vnd.google-apps.document";
            using (var stream = new System.IO.FileStream("files/document.txt",
                System.IO.FileMode.Open))
            {
                var request = this.service.Files.Create(
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

        protected string CreateTestBlob()
        {
            var fileMetadata = new File();
            fileMetadata.Name = "photo.jpg";
            using (var stream = new System.IO.FileStream("files/photo.jpg",
                System.IO.FileMode.Open))
            {
                var request = this.service.Files.Create(
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
