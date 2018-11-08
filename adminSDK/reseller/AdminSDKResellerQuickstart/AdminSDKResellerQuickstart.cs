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

// [START admin_sdk_reseller_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Reseller.v1;
using Google.Apis.Reseller.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AdminSDKResellerQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/reseller-dotnet-quickstart.json
        static string[] Scopes = { ResellerService.Scope.AppsOrder };
        static string ApplicationName = "G Suite Reseller API .NET Quickstart";

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

            // Create G Suite Reseller API service.
            var service = new ResellerService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            SubscriptionsResource.ListRequest request = service.Subscriptions.List();
            request.MaxResults = 10;

            // List subscriptions.
            IList<Subscription> subscriptions = request.Execute().SubscriptionsValue;
            Console.WriteLine("Subscriptions:");
            if (subscriptions != null && subscriptions.Count > 0)
            {
                foreach (var subscription in subscriptions)
                {
                    Console.WriteLine("{0} ({1}, {2})", subscription.CustomerId,
                        subscription.SkuId, subscription.Plan.PlanName);
                }
            }
            else
            {
                Console.WriteLine("No subscriptions found.");
            }
            Console.Read();
        }
    }
}
// [END admin_sdk_reseller_quickstart]
