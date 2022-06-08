using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class PatchCourseTest : BaseTest
{
  [Test]
  public void TestPatchCourse()
  {
    var course = PatchUpdate.ClassroomPatchUpdate(this.TestCourse.Id);
    Assert.IsNotNull(course, "Course not returned.");
    Assert.AreEqual(this.TestCourse.Id, course.Id, "Wrong course returned.");
  }
}