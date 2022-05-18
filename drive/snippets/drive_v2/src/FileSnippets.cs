using Google.Apis.Drive.v2beta;
using Google.Apis.Drive.v2beta.Data;
using System;
using System.Linq;
using Google.Apis.Download;
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
            var driveService = service;
            // [START uploadBasic]
            var fileMetadata = new File()
            {
                Title = "photo.jpg"
            };
            FilesResource.InsertMediaUpload request;
            using (var stream = new System.IO.FileStream("files/photo.jpg",
                                    System.IO.FileMode.Open))
            {
                request = driveService.Files.Insert(
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
            var driveService = service;
            // [START uploadToFolder]
            var folderId = "0BwwA4oUTeiV1TGRPeTVjaWRDY1E";
            // [START_EXCLUDE silent]
            folderId = realFolderId;
            // [END_EXCLUDE]
            var fileMetadata = new File()
            {
                Title = "photo.jpg",
                Parents = new List<ParentReference>
                {
                    new ParentReference()
                    {
                        Id = folderId
                    }
                }
            };
            FilesResource.InsertMediaUpload request;
            using (var stream = new System.IO.FileStream("files/photo.jpg",
                System.IO.FileMode.Open))
            {
                request = driveService.Files.Insert(
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
            var driveService = service;
            // [START uploadWithConversion]
            var fileMetadata = new File()
            {
                Title = "My Report",
                MimeType = "application/vnd.google-apps.spreadsheet"
            };
            FilesResource.InsertMediaUpload request;
            using (var stream = new System.IO.FileStream("files/report.csv",
                                    System.IO.FileMode.Open))
            {
                request = driveService.Files.Insert(
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
            var driveService = service;
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
            var driveService = service;
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
            var driveService = service;
            // [START createShortcut]
            var fileMetadata = new File()
            {
                Title = "Project plan",
                MimeType = "application/vnd.google-apps.drive-sdk"
            };
            var request = driveService.Files.Insert(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("File ID: " + file.Id);
            // [END createShortcut]
            return file.Id;
        }

        public DateTime? TouchFile(string realFileId, DateTime now)
        {
            var driveService = service;
            // [START touchFile]
            var fileId = "1sTWaJ_j7PkjzaBWtNc3IzovK5hQf21FbOw9yLeeLPNQ";
            var fileMetadata = new File()
            {
                ModifiedDate = DateTime.Now
            };
            // [START_EXCLUDE silent]
            fileId = realFileId;
            fileMetadata.ModifiedDate = now;
            // [END_EXCLUDE]
            var request = driveService.Files.Update(fileMetadata, fileId);
            request.SetModifiedDate = true;
            request.Fields = "id, modifiedDate";
            var file = request.Execute();
            Console.WriteLine("Modified time: " + file.ModifiedDate);
            // [END touchFile]
            return file.ModifiedDate;
        }

        public string CreateFolder()
        {
            var driveService = service;
            // [START createFolder]
            var fileMetadata = new File()
            {
                Title = "Invoices",
                MimeType = "application/vnd.google-apps.folder"
            };
            var request = driveService.Files.Insert(fileMetadata);
            request.Fields = "id";
            var file = request.Execute();
            Console.WriteLine("Folder ID: " + file.Id);
            // [END createFolder]
            return file.Id;
        }

        public IList<string> MoveFileToFolder(string realFileId,
                                              string realFolderId)
        {
            var driveService = service;
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
            var previousParents = string.Join(",", file.Parents.Select(parent => parent.Id));
            // Move the file to the new folder
            var updateRequest = driveService.Files.Update(new File(), fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderId;
            updateRequest.RemoveParents = previousParents;
            file = updateRequest.Execute();
            // [END moveFileToFolder]
            return file.Parents.Select(parent => parent.Id).ToList();
        }

        public IList<File> SearchFiles()
        {
            var driveService = service;
            var files = new List<File>();
            // [START searchFiles]
            string pageToken = null;
            do
            {
                var request = driveService.Files.List();
                request.Q = "mimeType='image/jpeg'";
                request.Spaces = "drive";
                request.Fields = "nextPageToken, items(id, title)";
                request.PageToken = pageToken;
                var result = request.Execute();
                foreach (var file in result.Items)
                {
                    Console.WriteLine(string.Format(
                            "Found file: {0} ({1})", file.Title, file.Id));
                }
                // [START_EXCLUDE silent]
                files.AddRange(result.Items);
                // [END_EXCLUDE]
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            // [END searchFiles]
            return files;
        }


        public IList<string> ShareFile(string realFileId, string realUser, string realDomain)
        {
            var driveService = service;
            var ids = new List<string>();
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
                Value = "user@example.com"
            };
            // [START_EXCLUDE silent]
            userPermission.EmailAddress = realUser;
            // [END_EXCLUDE]
            var request = driveService.Permissions.Insert(userPermission, fileId);
            request.Fields = "id";
            batch.Queue(request, callback);

            Permission domainPermission = new Permission()
            {
                Type = "domain",
                Role = "reader",
                Value = "example.com"
            };
            // [START_EXCLUDE silent]
            domainPermission.Domain = realDomain;
            // [END_EXCLUDE]
            request = driveService.Permissions.Insert(domainPermission, fileId);
            request.Fields = "id";
            batch.Queue(request, callback);
            var task = batch.ExecuteAsync();
            // [END shareFile]
            task.Wait();
            return ids;
        }
    }
}
