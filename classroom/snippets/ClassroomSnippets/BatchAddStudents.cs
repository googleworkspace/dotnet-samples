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

// [START classroom_batch_add_students]
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Requests;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom Batch Add Students API
    public class BatchAddStudents
    {
        /// <summary>
        /// Add multiple students in a specified course.
        /// </summary>
        /// <param name="courseId">Id of the course to add students.</param>
        /// <param name="studentEmails">Email address of the students.</param>
        public static void ClassroomBatchAddStudents(string courseId,
            List<string> studentEmails)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for 
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(ClassroomService.Scope.ClassroomRosters);

                // Create Classroom API service.
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Classroom Snippets"
                });

                var batch = new BatchRequest(service, "https://classroom.googleapis.com/batch");
                BatchRequest.OnResponse<Student> callback = (student, error, i, message) =>
                {
                    if (error != null)
                    {
                        Console.WriteLine("Error adding student to the course: {0}", error.Message);
                    }
                    else
                    {
                        Console.WriteLine("User '{0}' was added as a student to the course.",
                            student.Profile.Name.FullName);
                    }
                };
                foreach (var studentEmail in studentEmails)
                {
                    var student = new Student() {UserId = studentEmail};
                    var request = service.Courses.Students.Create(student, courseId);
                    batch.Queue<Student>(request, callback);
                }

                Task.WaitAll(batch.ExecuteAsync());
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
        }
    }
}
// [END classroom_batch_add_students]