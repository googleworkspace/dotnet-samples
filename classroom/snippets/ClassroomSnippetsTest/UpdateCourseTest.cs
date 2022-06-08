using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class UpdateCourseTest : BaseTest
{
  [Test]
  public void TestUpdateCourse()
  {
    var course = UpdateCourse.ClassroomUpdateCourse(this.TestCourse.Id);
    Assert.IsNotNull(course, "Course not returned.");
    Assert.AreEqual(this.TestCourse.Id, course.Id, "Wrong course returned.");
  }
}