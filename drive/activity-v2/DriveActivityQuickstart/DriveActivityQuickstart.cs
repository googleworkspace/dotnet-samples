// Copyright 2019 Google LLC
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

// [START drive_activity_v2_quickstart]
using Google.Apis.Auth.OAuth2;
using Google.Apis.DriveActivity.v2;
using Google.Apis.DriveActivity.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace DriveActivityQuickstart
{
    // Class to demonstrate the use of Drive list activity API
    class Program
    {
        /* Global instance of the scopes required by this quickstart.
         If modifying these scopes, delete your previously saved token.json/ folder. */
        static string[] Scopes = {DriveActivityService.Scope.DriveActivityReadonly};
        static string ApplicationName = "Drive Activity v2 API .NET Quickstart";

        static void Main(string[] args)
        {
            try
            {
                UserCredential credential;
                // Load client secrets.
                using (var stream =
                       new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    /* The file token.json stores the user's access and refresh tokens, and is created
                     automatically when the authorization flow completes for the first time. */
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Console.WriteLine("Credential file saved to: " + credPath);
                }

                // Create Google Drive Activity API service.
                var service = new DriveActivityService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });

                // Define parameters of request.
                QueryDriveActivityRequest requestData = new QueryDriveActivityRequest();
                requestData.PageSize = 10;
                ActivityResource.QueryRequest queryRequest = service.Activity.Query(requestData);

                // List activities.
                IList<DriveActivity> activities = queryRequest.Execute().Activities;
                Console.WriteLine("Recent activity:");
                if (activities != null && activities.Count > 0)
                {
                    foreach (var activity in activities)
                    {
                        string time = GetTimeInfo(activity);
                        string action = GetActionInfo(activity.PrimaryActionDetail);
                        List<string> actors = activity.Actors.Select(GetActorInfo).ToList();
                        List<string> targets = activity.Targets.Select(GetTargetInfo).ToList();
                        Console.WriteLine("{0}: {1}, {2}, {3}",
                            time, Truncated(actors), action, Truncated(targets));
                    }
                }
                else
                {
                    Console.WriteLine("No activity.");
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }

            // Returns a string representation of the first elements in a list.
            static string Truncated<T>(List<T> list, int limit = 2)
            {
                string contents = String.Join(", ", list.Take(limit));
                string more = list.Count > limit ? ", ..." : "";
                return String.Format("[{0}{1}]", contents, more);
            }

            // Returns the name of a set property in an object, or else "unknown".
            static string GetOneOf(Object obj)
            {
                foreach (var p in obj.GetType().GetProperties()) {
                    if (!ReferenceEquals(p.GetValue(obj), null)) {
                        return p.Name;
                    }
                }
                return "unknown";
            }

            // Returns a time associated with an activity.
            static string GetTimeInfo(DriveActivity activity)
            {
                if (activity.Timestamp != null) {
                    return activity.Timestamp.ToString();
                }

                if (activity.TimeRange != null) {
                    return activity.TimeRange.EndTime.ToString();
                }
                return "unknown";
            }

            // Returns the type of action.
            static string GetActionInfo(ActionDetail actionDetail) {
                return GetOneOf(actionDetail);
            }

            // Returns user information, or the type of user if not a known user.
            static string GetUserInfo(User user) {
                if (user.KnownUser != null) {
                    KnownUser knownUser = user.KnownUser;
                    bool isMe = knownUser.IsCurrentUser ?? false;
                    return isMe ? "people/me" : knownUser.PersonName;
                }
                return GetOneOf(user);
            }

            // Returns actor information, or the type of actor if not a user.
            static string GetActorInfo(Actor actor) {
                if (actor.User != null) {
                    return GetUserInfo(actor.User);
                }
                return GetOneOf(actor);
            }

            // Returns the type of a target and an associated title.
            static string GetTargetInfo(Target target) {
                if (target.DriveItem != null) {
                    return "driveItem:\"" + target.DriveItem.Title + "\"";
                }
                if (target.Drive != null) {
                    return "drive:\"" + target.Drive.Title + "\"";
                }
                if (target.FileComment != null) {
                    DriveItem parent = target.FileComment.Parent;
                    if (parent != null) {
                        return "fileComment:\"" + parent.Title + "\"";
                    }
                    return "fileComment:unknown";
                }
                return GetOneOf(target);
            }
        }
    }
}
// [END drive_activity_v2_quickstart]
