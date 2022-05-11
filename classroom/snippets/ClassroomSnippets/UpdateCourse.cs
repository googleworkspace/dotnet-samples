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

// [START classroom_update_course]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Net;
using Google;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom Update Course API
    public class UpdateCourse
    {
        /// <summary>
        /// Update one field of course 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        /// <exception cref="GoogleApiException"></exception>
        public static Course ClassroomUpdateCourse(string courseId)
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
                    ApplicationName = "Classroom API Snippet"
                });
              
                Course course = service.Courses.Get(courseId).Execute();
                course.Section = "Period 3";
                course.Room = "302";
                course = service.Courses.Update(course, courseId).Execute();
                Console.WriteLine("Course '{0}' updated.\n", course.Name);
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
                    Console.WriteLine("Failed to update the course. Error message: {0}", e.Message);
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
// [END classroom_update_course]