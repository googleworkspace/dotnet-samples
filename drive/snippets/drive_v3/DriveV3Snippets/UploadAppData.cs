// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START drive_upload_appdata]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class of demonstrate the use of Drive upload app data. 
    public class UploadAppData
    {
        /// <summary>
        /// Insert a file in the application data folder and prints file Id.
        /// </summary>
        /// <param name="filePath">File path to upload.</param>
        /// <returns>ID's of the inserted files, null otherwise.</returns>
        public static string DriveUploadAppData(string filePath)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.DriveAppdata);

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Snippets"
                });
                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    Name = "config.json",
                    Parents = new List<string>()
                    {
                        "appDataFolder"
                    }
                };
                FilesResource.CreateMediaUpload request;
                using (var stream = new FileStream(filePath,
                           FileMode.Open))
                {
                    request = service.Files.Create(
                        fileMetadata, stream, "application/json");
                    request.Fields = "id";
                    request.Upload();
                }

                var file = request.ResponseBody;
                // Prints the file id.
                Console.WriteLine("File ID: " + file.Id);
                return file.Id;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else
                {
                    throw;
                }
            }
            return null;
        }
    }
}
// [END drive_upload_appdata]