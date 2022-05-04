using Google.Apis.Drive.v3beta;
using Google.Apis.Drive.v3beta.Data;
using Google.Apis.Services;
using System;
using Google.Apis.Download;
using System.Collections;
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
            var driveService = this.service;
            // [START uploadAppData]
            var fileMetadata = new File()
            {
                Name = "config.json",
                Parents = new List<string>()
                {
                    "appDataFolder"
                }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream("files/config.json",
                System.IO.FileMode.Open))
            {
                request = driveService.Files.Create(
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
            var driveService = this.service;
            // [START listAppData]
            var request = driveService.Files.List();
            request.Spaces = "appDataFolder";
            request.Fields = "nextPageToken, files(id, name)";
            request.PageSize = 10;
            var result = request.Execute();
            foreach (var file in result.Files)
            {
                Console.WriteLine(String.Format(
                    "Found file: {0} ({1})", file.Name, file.Id));
            }
            // [END listAppData]
            return result;
        }

        public string FetchAppDataFolder()
        {
            var driveService = this.service;
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
