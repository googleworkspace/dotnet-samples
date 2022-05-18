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

// [START drive_create_team_drive]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use of Drive create team drive.
    public class CreateTeamDrive
    {
        /// <summary>
        /// Create a drive for team.
        /// </summary>
        /// <returns>id of the created drive, null otherwise.</returns>
        public static string DriveCreateTeamDrive()
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

                var teamDriveMetadata = new TeamDrive()
                {
                    Name = "Project Resources"
                };
                var requestId = Guid.NewGuid().ToString();
                var request = service.Teamdrives.Create(teamDriveMetadata, requestId);
                request.Fields = "id";
                var teamDrive = request.Execute();
                Console.WriteLine("Team Drive ID: " + teamDrive.Id);

                return teamDrive.Id;
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
// [END drive_create_team_drive]