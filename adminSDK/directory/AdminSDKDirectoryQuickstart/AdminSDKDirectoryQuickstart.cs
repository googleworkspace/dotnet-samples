// Copyright 2018 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// [START admin_sdk_directory_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AdminSDKDirectoryQuickstart
{
	class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/admin-directory_v1-dotnet-quickstart.json
        static string[] Scopes = { DirectoryService.Scope.AdminDirectoryUserReadonly };
        static string ApplicationName = "Directory API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Directory API service.
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            UsersResource.ListRequest request = service.Users.List();
            request.Customer = "my_customer";
            request.MaxResults = 10;
            request.OrderBy = UsersResource.ListRequest.OrderByEnum.Email;

            // List users.
            IList<User> users = request.Execute().UsersValue;
            Console.WriteLine("Users:");
            if (users != null && users.Count > 0)
            {
                foreach (var userItem in users)
                {
                    Console.WriteLine("{0} ({1})", userItem.PrimaryEmail,
                        userItem.Name.FullName);
                }
            }
            else
            {
                Console.WriteLine("No users found.");
            }
            Console.Read();
        }
    }
}
// [END admin_sdk_directory_quickstart]
