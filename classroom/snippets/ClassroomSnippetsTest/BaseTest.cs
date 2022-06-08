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
  protected string OtherUser = "rajesh@workspacesamples.dev";

  [SetUp]
  public void SetUp()
  {
    this.Service = getService();
    this.TestCourse = CreateTestCourse("me");
    
  }

  [TearDown]
   public void TearDown()
   {
     DeleteCourse(this.TestCourse.Id);
     this.TestCourse = null;
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