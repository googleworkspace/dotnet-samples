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

// [START classroom_add_teacher]
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using System;
using System.Net;
using Google;

namespace ClassroomSnippets
{
    // Class to demonstrate the use of Classroom Create Teacher API
    public class AddTeacher
    {
       /// <summary>
       /// Add teacher to the Course
       /// </summary>
       /// <param name="courseId"></param>
       /// <param name="teacherEmail"></param>
       /// <returns></returns>
        public static Teacher ClassroomAddTeacher( string courseId,
                 string teacherEmail)
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
                     ApplicationName = "Classroom API Snippet"
                 });
                 // // [START AddTeacher]
                 // string courseId = "123456";
                 // string teacherEmail = "alice@example.Edu";
                 // // [START_EXCLUDE silent]
                 // courseId = _courseId;
                 // teacherEmail = _teacherEmail;
                 // // [END_EXCLUDE]
                 var teacher = new Teacher
                 {
                     UserId = teacherEmail
                 };
                 // Add the teacher to the course.
                 teacher = service.Courses.Teachers.Create(teacher, courseId).Execute();
                     Console.WriteLine(
                         "User '{0}' was added as a teacher to the course with ID '{1}'.\n",
                         teacher.Profile.Name.FullName, courseId);
                     return teacher;
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
                     Console.WriteLine("Failed to Add the teacher. Error message: {0}", e.Message);
                 }
                 else
                 {
                     throw;
                 }
             }
             // [END classroom_add_teacher]
             return null;
         }
       

    }
    
}
// [END classroom_create_course]