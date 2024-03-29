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

// [START drive_create_folder]

using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;


namespace DriveV2Snippets
{
    public class CreateFolder
    {
        /// <summary>
        /// Creates a new folder
        /// </summary>
        /// <returns></returns>
        
        public static string DriveCreateFolder()
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.Drive);

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Snippets"
                });
                // File metadata
                var fileMetadata = new Google.Apis.Drive.v2.Data.File()
                {
                    Title = "Invoices",
                    MimeType = "application/vnd.google-apps.folder"
                };
                // Create a new folder on drive.
                var request = service.Files.Insert(fileMetadata);
                request.Fields = "id";
                var file = request.Execute();
                // Prints the created folder id.
                Console.WriteLine("Folder ID: " + file.Id);
                return file.Id;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is GoogleApiException)
                {
                    Console.WriteLine("Failed to Create Folder with Error : {0}", e.Message);
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
// [END drive_create_folder]
