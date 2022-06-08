using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class CreateCourseTest : BaseTest
{
  [Test]
  public void TestCreateCourse()
  {
    var course = CreateCourse.ClassroomCreateCourse();
    Assert.IsNotNull(course, "Course not returned.");
    //DeleteCourse(course.Id);
  }

}