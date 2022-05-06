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

// [START classroom_list_courses]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom List Course API
    public class ListCourses
    {
        /// <summary>
        /// Retrieves all courses with metadata.
        /// </summary>
        /// <returns>list of courses with its metadata, null otherwise.</returns>
        public static List<Course> ClassroomListCourses()
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
                var courses = new List<Course>();

                do
                {
                    var request = service.Courses.List();
                    request.PageSize = 100;
                    request.PageToken = pageToken;
                    var response = request.Execute();
                    courses.AddRange(response.Courses);
                    pageToken = response.NextPageToken;
                } while (pageToken != null);

                Console.WriteLine("Courses:");
                foreach (var course in courses)
                {
                    // Print the courses available in classroom
                    Console.WriteLine("{0} ({1})", course.Name, course.Id);
                } 
                return courses;
            }
            catch (Exception e)
            {
                // TODO(developer) - handle error appropriately
                if (e is AggregateException)
                {
                    Console.WriteLine("Credential Not found");
                }
                else if (e is ArgumentNullException)
                {
                    Console.WriteLine("No courses found.");
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
// [END classroom_list_courses]