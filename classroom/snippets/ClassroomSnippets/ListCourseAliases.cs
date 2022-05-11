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

// [START classroom_list_aliases]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom List Alias API
    public class ListCourseAliases
    {
        /// <summary>
        /// Retrieve the aliases for a course.
        /// </summary>
        /// <param name="courseId">Id of the course.</param>
        /// <returns>list of course aliases, null otherwise.</returns>
        public static List<CourseAlias> ClassroomListAliases(string courseId)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for 
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(ClassroomService.Scope.ClassroomCourses);

                // Create Classroom API service.
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Classroom Snippets"
                });

                string pageToken = null;
                var courseAliases = new List<CourseAlias>();

                do
                {
                    // List of aliases of specified course
                    var request = service.Courses.Aliases.List(courseId);
                    request.PageSize = 100;
                    request.PageToken = pageToken;
                    var response = request.Execute();
                    courseAliases.AddRange(response.Aliases);
                    pageToken = response.NextPageToken;
                } while (pageToken != null);

                Console.WriteLine("Aliases:");
                foreach (var courseAlias in courseAliases)
                {
                    // Print the aliases in a course.
                    Console.WriteLine(courseAlias.Alias);
                }
                return courseAliases;
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
                    Console.WriteLine("Course does not exist.");
                }
                else if (e is ArgumentNullException)
                {
                    Console.WriteLine("No aliases found.");
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
// [END classroom_list_aliases]