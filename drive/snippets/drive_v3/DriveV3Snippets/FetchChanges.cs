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

// [START drive_fetch_changes]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use-case of Drive's fetch changes in file.
    public class FetchChanges
    {
        /// <summary>
        /// Retrieve the list of changes for the currently authenticated user.
        /// prints changed file's ID
        /// </summary>
        /// <param name="savedStartPageToken">last saved start token for this user.</param>
        /// <returns>saved token for the current state of the account, null otherwise.</returns>
        public static string DriveFetchChanges(string savedStartPageToken)
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

                // Begin with our last saved start token for this user or the
                // current token from GetStartPageToken()
                string pageToken = savedStartPageToken;
                while (pageToken != null)
                {
                    var request = service.Changes.List(pageToken);
                    request.Spaces = "drive";
                    var changes = request.Execute();
                    foreach (var change in changes.Changes)
                    {
                        // Process change
                        Console.WriteLine("Change found for file: " + change.FileId);
                    }

                    if (changes.NewStartPageToken != null)
                    {
                        // Last page, save this token for the next polling interval
                        savedStartPageToken = changes.NewStartPageToken;
                    }
                    pageToken = changes.NextPageToken;
                }
                return savedStartPageToken;
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
// [END drive_fetch_changes]