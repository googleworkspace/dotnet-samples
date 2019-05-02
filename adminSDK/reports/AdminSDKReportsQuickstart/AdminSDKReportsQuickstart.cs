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

// [START admin_sdk_reports_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Admin.Reports.reports_v1;
using Google.Apis.Admin.Reports.reports_v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Linq;

namespace AdminSDKReportsQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/admin-reports_v1-dotnet-quickstart.json
        static string[] Scopes = { ReportsService.Scope.AdminReportsAuditReadonly };
        static string ApplicationName = "Reports API .NET Quickstart";

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

            // Create Reports API service.
            var service = new ReportsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            ActivitiesResource.ListRequest request = service.Activities
                .List("all", "login");
            request.MaxResults = 10;

            // List activities.
            IList<Activity> activities = request.Execute().Items;
            Console.WriteLine("Logins:");
            if (activities != null && activities.Count > 0)
            {
                foreach (var activityItem in activities)
                {
                    Console.WriteLine("{0}: {1} {2}", activityItem.Id.Time,
                        activityItem.Actor.Email,
                        activityItem.Events.First<Activity.EventsData>().Name);
                }
            }
            else
            {
                Console.WriteLine("No logins found.");
            }
            Console.Read();
        }
    }
}
// [END admin_sdk_reports_quickstart]
