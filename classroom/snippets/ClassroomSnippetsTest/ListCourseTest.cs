using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class ListCourseTest : BaseTest
{
  [Test]
  public void TestListCourse()
  {
    var courses = ListCourses.ClassroomListCourses(); 
    Assert.IsTrue(courses.Count > 0, "No courses returned.");
  }
}