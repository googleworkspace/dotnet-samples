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

// [START drive_recover_team_drives]

using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;

namespace DriveV2Snippets
{
    // Class to demonstrate use of Drive recover team drive.
    public class RecoverTeamDrives
    {
        /// <summary>
        /// Finds all Team Drives without an organizer and add one
        /// </summary>
        /// <param name="realUser">user id for the new organizer.</param>
        /// <returns></returns>
        public static IList<TeamDrive> DriveRecoverTeamDrives(string realUser)
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
                var teamDrives = new List<TeamDrive>();
                // Find all Team Drives without an organizer and add one.
                // Note: This example does not capture all cases. Team Drives
                // that have an empty group as the sole organizer, or an
                // organizer outside the organization are not captured. A
                // more exhaustive approach would evaluate each Team Drive
                // and the associated permissions and groups to ensure an active
                // organizer is assigned.
                string pageToken = null;
                var newOrganizerPermission = new Permission()
                {
                    Type = "user",
                    Role = "organizer",
                    Value = "user@example.com"
                };
                // [START_EXCLUDE silent]
                newOrganizerPermission.Value = realUser;
                // [END_EXCLUDE]

                do
                {
                    var request = service.Teamdrives.List();
                    request.UseDomainAdminAccess = true;
                    request.Q = "organizerCount = 0";
                    request.Fields = "nextPageToken, items(id, name)";
                    request.PageToken = pageToken;
                    var result = request.Execute();
                    foreach (var teamDrive in result.Items)
                    {
                        Console.WriteLine($"Found Team Drive without organizer: {teamDrive.Name} ({teamDrive.Id})");
                        // Note: For improved efficiency, consider batching
                        // permission insert requests
                        var permissionRequest = service.Permissions.Insert(
                            newOrganizerPermission,
                            teamDrive.Id
                        );
                        permissionRequest.UseDomainAdminAccess = true;
                        permissionRequest.SupportsTeamDrives = true;
                        permissionRequest.Fields = "id";
                        var permissionResult = permissionRequest.Execute();
                        Console.WriteLine($"Added organizer permission: {permissionResult.Id}");

                    }

                    // [START_EXCLUDE silent]
                    teamDrives.AddRange(result.Items);
                    // [END_EXCLUDE]
                    pageToken = result.NextPageToken;
                } while (pageToken != null);

                return teamDrives;
                
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credentials Not found {0}", e.Message);
                }
                else if (e is GoogleApiException)
                {
                    Console.WriteLine("Failed with an error {0}", e);
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
// [END drive_recover_team_drives]