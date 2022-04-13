using Google.Apis.Drive.v2beta;
using Google.Apis.Drive.v2beta.Data;
using System;
using System.Collections.Generic;

namespace dotnet
{
    public class AppDataSnippets
    {
        private DriveService service;

        public AppDataSnippets(DriveService service)
        {
            this.service = service;
        }

        public string UploadAppData()
        {
            var driveService = service;
            // [START uploadAppData]
            var fileMetadata = new File()
            {
                Title = "config.json",
                Parents = new List<ParentReference>() {
                  new ParentReference() {
                      Id = "appDataFolder"
                    }
                }
            };
            FilesResource.InsertMediaUpload request;
            using (var stream = new System.IO.FileStream("files/config.json",
                System.IO.FileMode.Open))
            {
                request = driveService.Files.Insert(
                    fileMetadata, stream, "application/json");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
            // [END uploadAppData]
            return file.Id;
        }

        public FileList ListAppData()
        {
            var driveService = service;
            // [START listAppData]
            var request = driveService.Files.List();
            request.Spaces = "appDataFolder";
            request.Fields = "nextPageToken, items(id, title)";
            request.MaxResults = 10;
            var result = request.Execute();
            foreach (var file in result.Items)
            {
                Console.WriteLine(string.Format(
                    "Found file: {0} ({1})", file.Title, file.Id));
            }
            // [END listAppData]
            return result;
        }

        public string FetchAppDataFolder()
        {
            var driveService = service;
            // [START fetchAppDataFolder]
            var getRequest = driveService.Files.Get("appDataFolder");
            getRequest.Fields = "id";
            var file = getRequest.Execute();
            Console.WriteLine("Folder ID: " + file.Id);
            // [END fetchAppDataFolder]
            return file.Id;
        }
    }
}
