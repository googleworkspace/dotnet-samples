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

// [START admin_sdk_groups_settings_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Groupssettings.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using Groups = Google.Apis.Groupssettings.v1.Data.Groups;

namespace AdminSDKGroupsSettingsQuickstart
{
	class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/groupssettings_v1-dotnet-quickstart.json
        static string[] Scopes = { "https://www.googleapis.com/auth/apps.groups.settings"};
        static string ApplicationName = "Groups Settings API .NET Quickstart";

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
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: {0}", credPath);
            }

            // Create Directory API service.
            var service = new GroupssettingsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            
            // Service ready to use

            if (args.Length == 0)
            {
                Console.WriteLine("No group email specified.");
                return;
            }
            
            String groupEmail = args[0];
            try
            {
                Groups settings = service.Groups.Get(groupEmail).Execute();
                Console.Write("Description: {0}", settings.Description);
            }
            catch (Exception err)
            {
                // TODO(developer) - handle exception
                Console.Error.WriteLine(err);
            }
            
        }
    }
}
// [END admin_sdk_groups_settings_quickstart]
