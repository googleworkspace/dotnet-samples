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

// [START drive_move_file_to_folder]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;

namespace DriveV3Snippets
{
    // Class to demonstrate use-case of Drive move file to folder.
    public class MoveFileToFolder
    {
        /// <summary>
        /// Move specified file to the specified folder.
        /// </summary>
        /// <param name="fileId">Id of file to be moved.</param>
        /// <param name="folderId">Id of folder where the fill will be moved.</param>
        /// <returns>list of parent ids for the file, null otherwise.</returns>
        public static IList<string> DriveMoveFileToFolder(string fileId,
            string folderId)
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

                // Retrieve the existing parents to remove
                var getRequest = service.Files.Get(fileId);
                getRequest.Fields = "parents";
                var file = getRequest.Execute();
                var previousParents = String.Join(",", file.Parents);
                // Move the file to the new folder
                var updateRequest =
                    service.Files.Update(new Google.Apis.Drive.v3.Data.File(),
                        fileId);
                updateRequest.Fields = "id, parents";
                updateRequest.AddParents = folderId;
                updateRequest.RemoveParents = previousParents;
                file = updateRequest.Execute();

                return file.Parents;
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
                    Console.WriteLine("File or Folder not found");
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
// [END drive_move_file_to_folder]