// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using NUnit.Framework;

namespace ClassroomSnippetsTest;

public class BaseTest
{
  protected ClassroomService Service;
  protected Course TestCourse;
  protected string OtherUser = "gduser1@workspacesamples.dev";

  [SetUp]
  public void SetUp()
  {
    this.Service = getService();
    this.TestCourse = CreateTestCourse("me");
    
  }
  
  protected ClassroomService getService()
  {
  
    /* Load pre-authorized user credentials from the environment.
      TODO(developer) - See https://developers.google.com/identity for 
      guides on implementing OAuth2 for your application. */
    GoogleCredential credential = GoogleCredential.GetApplicationDefault()
      .CreateScoped(ClassroomService.Scope.ClassroomRosters,
        ClassroomService.Scope.ClassroomCourses);

    return new ClassroomService(new BaseClientService.Initializer()
    {
      HttpClientInitializer = credential,
      ApplicationName = "Classroom API .NET Snippet Tests",
    });
  }
  public Course CreateTestCourse(string ownerId)
  {
    string alias = "p:" + System.Guid.NewGuid().ToString();
    Course course = new Course()
    {
      Id = alias,
      Name = "Test Course",
      Section = "Section",
      OwnerId = ownerId
    };
    return this.Service.Courses.Create(course).Execute();
  }
  protected void DeleteCourse(string courseId)
  {
    this.Service.Courses.Delete(courseId).Execute();
  }
}