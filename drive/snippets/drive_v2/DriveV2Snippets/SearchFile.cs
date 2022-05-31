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

// [START drive_search_files] 
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Services;
using File = Google.Apis.Drive.v2.Data.File;


namespace DriveV2Snippets
{
    // Class to demonstrate use-case of Drive search files. 
    public class SearchFiles
    {   
        /// <summary>
        /// Search for specific set of files.
        /// </summary>
        /// <returns>search result list, null otherwise.</returns>
        public static IList<File> DriveSearchFiles()
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
         
                var files = new List<File>();
                string pageToken = null;
                do
                {
                    var request = service.Files.List();
                    request.Q = "mimeType='image/jpeg'";
                    request.Spaces = "drive";
                    request.Fields = "nextPageToken, items(id, title)";
                    request.PageToken = pageToken;
                    var result = request.Execute();
                    foreach (var file in result.Items)
                    {
                        Console.WriteLine($"Found file: {file.Title} ({file.Id})");
                    }
                    // [START_EXCLUDE silent]
                    files.AddRange(result.Items);
                    // [END_EXCLUDE]
                    pageToken = result.NextPageToken;
                } while (pageToken != null);
                return files;
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
                    Console.WriteLine("Failed with an error {0}",e.Message);
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
// [END drive_search_files] 
