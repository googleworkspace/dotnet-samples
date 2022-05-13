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

// [START drive_share_file]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Requests;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use-case of Drive modify permissions.
    public class ShareFile
    {
        /// <summary>
        /// Batch permission modification.
        /// </summary>
        /// <param name="realFileId">File id.</param>
        /// <param name="realUser">User id.</param>
        /// <param name="realDomain">Domain id.</param>
        /// <returns>list of modified permissions, null otherwise.</returns>
        public static IList<String> DriveShareFile(string realFileId, string realUser, string realDomain)
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

                var ids = new List<String>();
                var batch = new BatchRequest(service);
                BatchRequest.OnResponse<Permission> callback = delegate(
                    Permission permission,
                    RequestError error,
                    int index,
                    HttpResponseMessage message)
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
                    EmailAddress = realUser
                };

                var request = service.Permissions.Create(userPermission, realFileId);
                request.Fields = "id";
                batch.Queue(request, callback);

                Permission domainPermission = new Permission()
                {
                    Type = "domain",
                    Role = "reader",
                    Domain = realDomain
                };
                request = service.Permissions.Create(domainPermission, realFileId);
                request.Fields = "id";
                batch.Queue(request, callback);
                var task = batch.ExecuteAsync();
                task.Wait();
                return ids;
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
// [END drive_share_file]