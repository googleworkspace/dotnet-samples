using System.Collections.Generic;
using NUnit.Framework;
using ClassroomSnippets;

namespace ClassroomSnippetsTest;

public class BatchAddStudentsTest : BaseTest
{
  [Test]
  public void TestBatchAddStudents()
  {
    var studentEmails = new List<string>()
    {
      "erics@homeroomacademy.com",
      "zach@homeroomacademy.com"
    };
    BatchAddStudents.ClassroomBatchAddStudents(this.TestCourse.Id, studentEmails);
  }
}