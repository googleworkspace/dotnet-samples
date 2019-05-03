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
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // in token.json
        static string[] Scopes = { DriveActivityService.Scope.DriveActivityReadonly };
        static string ApplicationName = "Drive Activity v2 API .NET Quickstart";

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

            // Create Google Drive Activity API service.
            var service = new DriveActivityService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define parameters of request.
            QueryDriveActivityRequest requestData = new QueryDriveActivityRequest();
            requestData.PageSize = 10;
            ActivityResource.QueryRequest queryRequest = service.Activity.Query(requestData);

            // List activities.
            IList<Google.Apis.DriveActivity.v2.Data.DriveActivity> activities = queryRequest.Execute().Activities;
            Console.WriteLine("Recent activity:");
            if (activities != null && activities.Count > 0)
            {
                foreach (var activity in activities)
                {
                    string time = getTimeInfo(activity);
                    string action = getActionInfo(activity.PrimaryActionDetail);
                    List<string> actors = activity.Actors.Select(a => getActorInfo(a)).ToList();
                    List<string> targets = activity.Targets.Select(t => getTargetInfo(t)).ToList();
                    Console.WriteLine("{0}: {1}, {2}, {3}",
                            time, truncated(actors), action, truncated(targets));
                }
            }
            else
            {
                Console.WriteLine("No activity.");
            }
            Console.Read();
        }

        // Returns a string representation of the first elements in a list.
        static string truncated<T>(List<T> list, int limit = 2)
        {
            string contents = String.Join(", ", list.Take(limit));
            string more = list.Count > limit ? ", ..." : "";
            return String.Format("[{0}{1}]", contents, more);
        }

        // Returns the name of a set property in an object, or else "unknown".
        static string getOneOf(Object obj)
        {
            foreach (var p in obj.GetType().GetProperties()) {
                if (!Object.ReferenceEquals(p.GetValue(obj), null)) {
                    return p.Name;
                }
            }
            return "unknown";
        }

        // Returns a time associated with an activity.
        static string getTimeInfo(DriveActivity activity)
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
        static string getActionInfo(ActionDetail actionDetail) {
            return getOneOf(actionDetail);
        }

        // Returns user information, or the type of user if not a known user.
        static string getUserInfo(User user) {
            if (user.KnownUser != null) {
                KnownUser knownUser = user.KnownUser;
                bool isMe = knownUser.IsCurrentUser ?? false;
                return isMe ? "people/me" : knownUser.PersonName;
            }
            return getOneOf(user);
        }

        // Returns actor information, or the type of actor if not a user.
        static string getActorInfo(Actor actor) {
            if (actor.User != null) {
                return getUserInfo(actor.User);
            }
            return getOneOf(actor);
        }

        // Returns the type of a target and an associated title.
        static string getTargetInfo(Target target) {
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
            return getOneOf(target);
        }

    }
}
// [END drive_activity_v2_quickstart]
