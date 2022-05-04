using Google.Apis.Drive.v3beta;
using Google.Apis.Drive.v3beta.Data;
using Google.Apis.Services;
using System;
using Google.Apis.Download;
using System.Collections;
using System.Collections.Generic;
using Google.Apis.Requests;

namespace dotnet
{
    public class FileSnippets
    {
        private DriveService service;

        public FileSnippets(DriveService service)
        {
            this.service = service;
        }

        public string UploadBasic()
        {
            var driveService = this.service;
            // [START uploadBasic]
            var fileMetadata = new File()
            {
                Name = "photo.jpg"
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream("files/photo.jpg",
                                    System.IO.FileMode.Open))
            {
                request = driveService.Files.Create(
                    fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
            // [END uploadBasic]
            return file.Id;
        }

        public File UploadToFolder(string realFolderId)
        {
            var driveService = this.service;
            // [START uploadToFolder]
            var folderId = "0BwwA4oUTeiV1TGRPeTVjaWRDY1E";
            // [START_EXCLUDE silent]
            folderId = realFolderId;
            // [END_EXCLUDE]
            var fileMetadata = new File()
            {
                Name = "photo.jpg",
                Parents = new List<string>
                {
                    folderId
                }
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream("files/photo.jpg",
                System.IO.FileMode.Open))
            {
                request = driveService.Files.Create(
                    fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
            // [END uploadToFolder]
            return file;
        }

        public string UploadWithConversion()
        {
            var driveService = this.service;
            // [START uploadWithConversion]
            var fileMetadata = new File()
            {
                Name = "My Report",
                MimeType = "application/vnd.google-apps.spreadsheet"
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream("files/report.csv",
                                    System.IO.FileMode.Open))
            {
                request = driveService.Files.Create(
                    fileMetadata, stream, "text/csv");
                request.Fields = "id";
                request.Upload();
            }
            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
            // [END uploadWithConversion]
            return file.Id;
        }

        public System.IO.MemoryStream ExportPdf(string realFileId)
        {
            var driveService = this.service;
            // [START exportPdf]
            var fileId = "1ZdR3L3qP4Bkq8noWLJHSr_iBau0DNT4Kli4SxNc2YEo";
            // [START_EXCLUDE silent]
            fileId = realFileId;
            // [END_EXCLUDE]
            var request = driveService.Files.Export(fileId, "application/pdf");
            var stream = new System.IO.MemoryStream();
            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                    (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
            // [END exportPdf]
            return stream;
        }

        public System.IO.MemoryStream DownloadFile(string realFileId)
        {
            var driveService = this.service;
            // [START downloadFile]
            var fileId = "0BwwA4oUTeiV1UVNwOHItT0xfa2M";
            // [START_EXCLUDE silent]
            fileId = realFileId;
            // [END_EXCLUDE]
            var request = driveService.Files.Get(fileId);
            var stream = new System.IO.MemoryStream();

            // Add a handler which will be notified on progress changes.
            // It will notify on each chunk download and when the
            // download is completed or failed.
            request.MediaDownloader.ProgressChanged +=
                (IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case DownloadStatus.Downloading:
                        {
                            Console.WriteLine(progress.BytesDownloaded);
                            break;
                        }
                    case DownloadStatus.Completed:
                        {
                            Console.WriteLine("Download complete.");
                            break;
                        }
                    case DownloadStatus.Failed:
                        {
                            Console.WriteLine("Download failed.");
                            break;
                        }
                }
            };
            request.Download(stream);
            // [END downloadFile]
            return stream;
        }

        public string CreateShortcut()
        {
            var driveService = this.service;
            // [START createShortcut]
            var fileMetadata = new File()
            {
                Name = "Project plan",
                MimeType = "application/vnd.google-apps.drive-sdk"
            };
            var request = driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("File ID: " + file.Id);
            // [END createShortcut]
            return file.Id;
        }

        public DateTime? TouchFile(string realFileId, DateTime now)
        {
            var driveService = this.service;
            // [START touchFile]
            var fileId = "1sTWaJ_j7PkjzaBWtNc3IzovK5hQf21FbOw9yLeeLPNQ";
            var fileMetadata = new File()
            {
                ModifiedTime = DateTime.Now
            };
            // [START_EXCLUDE silent]
            fileId = realFileId;
            fileMetadata.ModifiedTime = now;
            // [END_EXCLUDE]
            var request = driveService.Files.Update(fileMetadata, fileId);
            request.Fields = "id, modifiedTime";
            var file = request.Execute();
            Console.WriteLine("Modified time: " + file.ModifiedTime);
            // [END touchFile]
            return file.ModifiedTime;
        }

        public string CreateFolder()
        {
            var driveService = this.service;
            // [START createFolder]
            var fileMetadata = new File()
            {
                Name = "Invoices",
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);
            // [END createFolder]
            return file.Id;
        }

        public IList<string> MoveFileToFolder(string realFileId,
                                              string realFolderId)
        {
            var driveService = this.service;
            // [START moveFileToFolder]
            var fileId = "1sTWaJ_j7PkjzaBWtNc3IzovK5hQf21FbOw9yLeeLPNQ";
            var folderId = "0BwwA4oUTeiV1TGRPeTVjaWRDY1E";
            // [START_EXCLUDE silent]
            fileId = realFileId;
            folderId = realFolderId;
            // [END_EXCLUDE]
            // Retrieve the existing parents to remove
            var getRequest = driveService.Files.Get(fileId);
            getRequest.Fields = "parents";
            var file = getRequest.Execute();
            var previousParents = String.Join(",", file.Parents);
            // Move the file to the new folder
            var updateRequest = driveService.Files.Update(new File(), fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderId;
            updateRequest.RemoveParents = previousParents;
            file = updateRequest.Execute();
            // [END moveFileToFolder]
            return file.Parents;
        }

        public IList<File> SearchFiles()
        {
            var driveService = this.service;
            var files = new List<File>();
            // [START searchFiles]
            string pageToken = null;
            do
            {
                var request = driveService.Files.List();
                request.Q = "mimeType='image/jpeg'";
                request.Spaces = "drive";
                request.Fields = "nextPageToken, files(id, name)";
                request.PageToken = pageToken;
                var result = request.Execute();
                foreach (var file in result.Files)
                {
                    Console.WriteLine(String.Format(
                            "Found file: {0} ({1})", file.Name, file.Id));
                }
                // [START_EXCLUDE silent]
                files.AddRange(result.Files);
                // [END_EXCLUDE]
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            // [END searchFiles]
            return files;
        }


        public IList<String> ShareFile(string realFileId, string realUser, string realDomain)
        {
            var driveService = this.service;
            var ids = new List<String>();
            // [START shareFile]
            var fileId = "1sTWaJ_j7PkjzaBWtNc3IzovK5hQf21FbOw9yLeeLPNQ";
            // [START_EXCLUDE silent]
            fileId = realFileId;
            // [END_EXCLUDE]
            var batch = new BatchRequest(driveService);
            BatchRequest.OnResponse<Permission> callback = delegate (
                Permission permission,
                RequestError error,
                int index,
                System.Net.Http.HttpResponseMessage message)
            {
                if (error != null)
                {
                    // Handle error
                    Console.WriteLine(error.Message);
                }
                else
                {
                    Console.WriteLine("Permission ID: " + permission.Id);
                    // [START_EXCLUDE silent]
                    ids.Add(permission.Id);
                    // [END_EXCLUDE]
                }
            };
            Permission userPermission = new Permission()
            {
                Type = "user",
                Role = "writer",
                EmailAddress = "user@example.com"
            };
            // [START_EXCLUDE silent]
            userPermission.EmailAddress = realUser;
            // [END_EXCLUDE]
            var request = driveService.Permissions.Create(userPermission, fileId);
            request.Fields = "id";
            batch.Queue(request, callback);

            Permission domainPermission = new Permission()
            {
                Type = "domain",
                Role = "reader",
                Domain = "example.com"
            };
            // [START_EXCLUDE silent]
            domainPermission.Domain = realDomain;
            // [END_EXCLUDE]
            request = driveService.Permissions.Create(domainPermission, fileId);
            request.Fields = "id";
            batch.Queue(request, callback);
            var task = batch.ExecuteAsync();
            // [END shareFile]
            task.Wait();
            return ids;
        }
    }
}
