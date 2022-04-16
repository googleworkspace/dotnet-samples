using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using ClassroomSnippets;
using System.Collections.Generic;

namespace ClassroomSnippetsTest
{
    [TestClass]
    public class CoursesTest : BaseTest
    {
        private ClassroomService service;
        private Course testCourse;
        private string otherUser = "erics@homeroomacademy.com";

        public CoursesTest()
        {
            this.service = this.getService();
        }

        [TestInitialize]
        public void SetUp()
        {
            this.testCourse = CreateTestCourse("me");
        }

        [TestCleanup]
        public void TearDown()
        {
            DeleteCourse(this.testCourse.Id);
            this.testCourse = null;
        }

        [TestMethod]
        public void TestCreateCourse()
        {
            var course = Courses.CreateCourse(this.service);
            Assert.IsNotNull(course, "Course not returned.");
            DeleteCourse(course.Id);
        }

        [TestMethod]
        public void TestGetCourse()
        {
            var course = Courses.GetCourse(this.service, this.testCourse.Id);
            Assert.IsNotNull(course, "Course not returned.");
            Assert.AreEqual(this.testCourse.Id, course.Id, "Wrong course returned.");
        }

        [TestMethod]
        public void TestListCourses()
        {
            var courses = Courses.ListCourses(this.service);
            Assert.IsTrue(courses.Count > 0, "No courses returned.");
        }

        [TestMethod]
        public void TestUpdateCourse()
        {
            var course = Courses.UpdateCourse(this.service, this.testCourse.Id);
            Assert.IsNotNull(course, "Course not returned.");
            Assert.AreEqual(this.testCourse.Id, course.Id, "Wrong course returned.");
        }

        [TestMethod]
        public void TestPatchCourse()
        {
            var course = Courses.PatchCourse(this.service, this.testCourse.Id);
            Assert.IsNotNull(course, "Course not returned.");
            Assert.AreEqual(this.testCourse.Id, course.Id, "Wrong course returned.");
        }

        [TestMethod]
        public void TestCreateCourseAlias()
        {
            String alias = "p:" + System.Guid.NewGuid().ToString();
            var courseAlias = Courses.CreateCourseAlias(this.service, this.testCourse.Id, alias);
            Assert.IsNotNull(courseAlias, "Course alias not returned.");
            Assert.AreEqual(alias, courseAlias.Alias, "Wrong course alias returned.");
        }

        [TestMethod]
        public void TestListCourseAliases()
        {
            var courseAliases = Courses.ListCourseAliases(this.service, this.testCourse.Id);
            Assert.AreEqual(courseAliases.Count, 1, "Incorrect number of course aliases returned.");
        }

        [TestMethod]
        public void TestAddTeacher()
        {
            var teacher = Courses.AddTeacher(this.service, this.testCourse.Id, this.otherUser);
            Assert.IsNotNull(teacher, "Teacher not returned.");
            Assert.AreEqual(this.testCourse.Id, teacher.CourseId, "Teacher added to wrong course.");
        }

        [TestMethod]
        public void TestEnrollAsStudent()
        {
            var course = this.CreateTestCourse(this.otherUser);
            var student = Courses.EnrollAsStudent(this.service, course.Id, course.EnrollmentCode);
            this.DeleteCourse(course.Id);
            Assert.IsNotNull(student, "Student not returned.");
            Assert.AreEqual(course.Id, student.CourseId, "Student added to wrong course.");
        }

        [TestMethod]
        public void TestBatchAddStudents() {
            var studentEmails = new List<string>()
            {
                "erics@homeroomacademy.com",
                "zach@homeroomacademy.com"
            };
            Courses.BatchAddStudents(this.service, this.testCourse.Id, studentEmails);
        }

        private Course CreateTestCourse(string ownerId)
        {
            string alias = "p:" + System.Guid.NewGuid().ToString();
            Course course = new Course()
            {
                Id = alias,
                Name = "Test Course",
                Section = "Section",
                OwnerId = ownerId
            };
            return this.service.Courses.Create(course).Execute();
        }

        private void DeleteCourse(string courseId)
        {
            this.service.Courses.Delete(courseId).Execute();
        }
    }
}
