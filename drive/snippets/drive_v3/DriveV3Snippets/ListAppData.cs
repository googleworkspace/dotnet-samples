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

// [START drive_list_appdata]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use-case of Drive's list files in the application data folder.
    public class ListAppData
    {
        /// <summary>
        /// List down files in the application data folder.
        /// </summary>
        /// <returns>list of 10 files, null otherwise.</returns>
        public static FileList DriveListAppData()
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

                var request = service.Files.List();
                request.Spaces = "appDataFolder";
                request.Fields = "nextPageToken, files(id, name)";
                request.PageSize = 10;
                var result = request.Execute();
                foreach (var file in result.Files)
                {
                    // Prints the list of 10 file names.
                    Console.WriteLine("Found file: {0} ({1})", file.Name, file.Id);
                }
                return result;
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
// [END drive_list_appdata]