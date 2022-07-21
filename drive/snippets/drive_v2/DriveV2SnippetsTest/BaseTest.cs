// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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