using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class GetCourseTest : BaseTest
{
  [Test]
  public void TestGetCourse()
  {
    var course = GetCourse.ClassroomGetCourse(this.TestCourse.Id);
    Assert.IsNotNull(course, "Course not returned.");
    Assert.AreEqual(this.TestCourse.Id, course.Id, "Wrong course returned.");
  }
}