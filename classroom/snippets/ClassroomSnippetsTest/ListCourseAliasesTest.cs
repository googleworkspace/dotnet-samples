using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class ListCourseAliasesTest : BaseTest
{
  [Test]
  public void TestListAliasesCourse()
  {
    var courseAliases = ListCourseAliases.ClassroomListAliases(this.TestCourse.Id);
    Assert.AreEqual(courseAliases.Count, 1, "Incorrect number of course aliases returned.");
  }
}