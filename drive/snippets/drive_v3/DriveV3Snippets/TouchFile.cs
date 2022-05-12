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

// [START drive_touch_file]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use-case of Drive touch file API
    public class TouchFile
    {
        /// <summary>
        /// Change the file's modification timestamp.
        /// </summary>
        /// <param name="fileId">Id of file to be modified.</param>
        /// <param name="now">Timestamp in milliseconds.</param>
        /// <returns>newly modified timestamp, null otherwise.</returns>
        public static DateTime? DriveTouchFile(string fileId, DateTime now)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential
                    .GetApplicationDefault()
                    .CreateScoped(DriveService.Scope.Drive);

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Drive API Snippets"
                });

                var fileMetadata = new Google.Apis.Drive.v3.Data.File()
                {
                    ModifiedTime = DateTime.Now
                };
                // [START_EXCLUDE silent]
                fileMetadata.ModifiedTime = now;
                // [END_EXCLUDE]
                var request = service.Files.Update(fileMetadata, fileId);
                request.Fields = "id, modifiedTime";
                var file = request.Execute();
                // Prints the modified date of the file.
                Console.WriteLine("Modified time: " + file.ModifiedTime);
                return file.ModifiedTime;
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
                    Console.WriteLine("File not found");
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
// [END drive_touch_file]