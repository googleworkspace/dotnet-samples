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

// [START classroom_get_course]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom Get Course API
    public class GetCourse
    {
        /// <summary>
        /// Retrieve a single course's metadata.
        /// </summary>
        /// <param name="courseId">Id of the course.</param>
        /// <returns>a course, null otherwise.</returns>
        public static Course ClassroomGetCourse(string courseId)
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

                // Get the course details using course id
                Course course = service.Courses.Get(courseId).Execute();
                Console.WriteLine("Course '{0}' found.\n", course.Name);
                return course;
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
                else
                {
                    throw;
                }
            }
            return null;
        }
    }
}
// [END classroom_get_course]