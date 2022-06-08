// using NUnit.Framework;
// using ClassroomSnippets;
//
// namespace ClassroomSnippetsTest;
//
// public class AddTeacherTest : BaseTest
// {
//   [Test]
//   public void TestAddTeacher()
//   {
//     var teacher = AddTeacher.ClassroomAddTeacher( this.TestCourse.Id, this.OtherUser);
//     Assert.IsNotNull(teacher, "Teacher not returned.");
//     Assert.AreEqual(this.TestCourse.Id, teacher.CourseId, "Teacher added to wrong course.");
//   }
// }