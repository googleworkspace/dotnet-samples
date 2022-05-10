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

// [START classroom_add_student]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Net;
using Google;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom Create Student API
    public class AddStudent
    {
        public static Student ClassroomAddStudent(string courseId, string enrollmentCode)
        {
            try
            {
                /* Load pre-authorized user credentials from the environment.
                 TODO(developer) - See https://developers.google.com/identity for 
                 guides on implementing OAuth2 for your application. */
                GoogleCredential credential = GoogleCredential.GetApplicationDefault()
                    .CreateScoped(ClassroomService.Scope.ClassroomRosters);
                var service = new ClassroomService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Classroom API .NET Quickstart"
                });

                var student = new Student
                {
                    UserId = "me"
                };

                var request = service.Courses.Students.Create(student, courseId);
                request.EnrollmentCode = enrollmentCode;
                student = request.Execute();
                Console.WriteLine(
                    "User '{0}' was enrolled  as a student in the course with ID '{1}'.\n",
                    student.Profile.Name.FullName, courseId);
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
                    Console.WriteLine("Failed to Add the Student. Error message: {0}", e.Message);
                }
                else
                {
                    throw;
                }
            }

            // [END classroom_add_student]
            return null;
        }
    }
    
}