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

// [START classroom_create_course]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;

namespace ClassroomSnippets
{   
    // Class to demonstrate the use of Classroom Create Course API
    public class CreateCourse
    {
        /// <summary>
        /// Creates a new course with description.
        /// </summary>
        /// <returns>newly created course</returns>
        public static Course ClassroomCreateCourse()
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
                    ApplicationName = "Classroom API Snippets"
                });

                // Create a new course with description.
                var course = new Course
                {
                    Name = "10th Grade Biology",
                    Section = "Period 2",
                    DescriptionHeading = "Welcome to 10th Grade Biology",
                    Description = "We'll be learning about about the structure of living creatures "
                                  + "from a combination of textbooks, guest lectures, and lab work. Expect "
                                  + "to be excited!",
                    Room = "301",
                    OwnerId = "me",
                    CourseState = "PROVISIONED"
                };

                course = service.Courses.Create(course).Execute();
                // Prints the new created course Id and name.
                Console.WriteLine("Course created: {0} ({1})", course.Name, course.Id);
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
                    Console.WriteLine("OwnerId not specified.");
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
// [END classroom_create_course]