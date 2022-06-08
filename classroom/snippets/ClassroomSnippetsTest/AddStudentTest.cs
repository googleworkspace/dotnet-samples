using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class AddStudentTest : BaseTest
{
  [Test]
  public void TestAddStudent()
  {
    var course = this.CreateTestCourse(this.OtherUser);
    var student = AddStudent.ClassroomAddStudent(course.Id, course.EnrollmentCode);
    DeleteCourse(course.Id);
    Assert.IsNotNull(student, "Student not returned.");
    Assert.AreEqual(course.Id, student.CourseId, "Student added to wrong course.");
  }
}