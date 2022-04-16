using Google;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomSnippets
{
    public class Courses
    {
        public static Course CreateCourse(ClassroomService service)
        {
            // [START CreateCourse]
            var course = new Course
            {
                Name = "10th Grade Biology",
                Section = "Period 2",
                DescriptionHeading = "Welcome to 10th Grade Biology",
                Description = "We'll be learning about about the structure of living creatures "
                    + "from a combination of textbooks, guest lectures, and lab work. Expect "
                    + "to be excited!",
                Room = "301",
                OwnerId = "me",
                CourseState = "PROVISIONED"
            };

            course = service.Courses.Create(course).Execute();
            Console.WriteLine("Course created: {0} ({1})", course.Name, course.Id);
            // [END CreateCourse]
            return course;
        }

        public static Course GetCourse(ClassroomService service, string _courseId)
        {
            // [START GetCourse]
            string courseId = "123456";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            // [END_EXCLUDE]
            Course course = null;
            try
            {
                course = service.Courses.Get(courseId).Execute();
                Console.WriteLine("Course '{0}' found\n.", course.Name);
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    Console.WriteLine("Course with ID '{0}' not found.\n", courseId);
                }
                else
                {
                    throw e;
                }
            }
            // [END GetCourse]
            return course;
        }

        public static List<Course> ListCourses(ClassroomService service)
        {
            // [START ListCourses]
            string pageToken = null;
            var courses = new List<Course>();

            do
            {
                var request = service.Courses.List();
                request.PageSize = 100;
                request.PageToken = pageToken;
                var response = request.Execute();
                courses.AddRange(response.Courses);
                pageToken = response.NextPageToken;
            } while (pageToken != null);

            if (courses.Count == 0)
            {
                Console.WriteLine("No courses found.");
            }
            else
            {
                Console.WriteLine("Courses:");
                foreach (var course in courses)
                {
                    Console.WriteLine("{0} ({1})\n", course.Name, course.Id);
                }
            }
            // [END ListCourses]
            return courses;
        }

        public static Course UpdateCourse(ClassroomService service, string _courseId)
        {
            // [START UpdateCourse]
            string courseId = "123456";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            // [END_EXCLUDE]
            var course = service.Courses.Get(courseId).Execute();
            course.Section = "Period 3";
            course.Room = "302";
            course = service.Courses.Update(course, courseId).Execute();
            Console.WriteLine("Course '{0}' updated.\n", course.Name);
            // [END UpdateCourse]
            return course;
        }

        public static Course PatchCourse(ClassroomService service, string _courseId)
        {
            // [START PatchCourse]
            string courseId = "123456";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            // [END_EXCLUDE]
            var course = new Course
            {
                Section = "Period 3",
                Room = "302"
            };

            var request = service.Courses.Patch(course, courseId);
            request.UpdateMask = "section,room";
            course = request.Execute();
            Console.WriteLine("Course '{0}' updated.\n", course.Name);
            // [END PatchCourse]
            return course;
        }

        public static CourseAlias CreateCourseAlias(ClassroomService service, string _courseId,
                string _alias)
        {
            // [START CreateCourseAlias]
            string courseId = "123456";
            string alias = "p:bio10p2";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            alias = _alias;
            // [END_EXCLUDE]
            var courseAlias = new CourseAlias
            {
                Alias = alias
            };

            try
            {
                courseAlias = service.Courses.Aliases.Create(courseAlias, courseId).Execute();
                Console.WriteLine("Alias '{0}' created.\n", courseAlias.Alias);
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    Console.WriteLine("Alias '{0}' is already in use.\n", alias);
                }
                else
                {
                    throw e;
                }
            }
            // [END CreateCourseAlias]
            return courseAlias;
        }

        public static List<CourseAlias> ListCourseAliases(ClassroomService service,
                string _courseId)
        {
            // [START ListCourseAliases]
            string courseId = "123456";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            // [END_EXCLUDE]
            string pageToken = null;
            var courseAliases = new List<CourseAlias>();

            do
            {
                var request = service.Courses.Aliases.List(courseId);
                request.PageSize = 100;
                request.PageToken = pageToken;
                var response = request.Execute();
                courseAliases.AddRange(response.Aliases);
                pageToken = response.NextPageToken;
            } while (pageToken != null);

            if (courseAliases.Count == 0)
            {
                Console.WriteLine("No aliases found.");
            }
            else
            {
                Console.WriteLine("Aliases:");
                foreach (var courseAlias in courseAliases)
                {
                    Console.WriteLine(courseAlias.Alias);
                }
            }
            // [END ListCourseAliases]
            return courseAliases;
        }

        public static Teacher AddTeacher(ClassroomService service, string _courseId,
                string _teacherEmail)
        {
            // [START AddTeacher]
            string courseId = "123456";
            string teacherEmail = "alice@example.Edu";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            teacherEmail = _teacherEmail;
            // [END_EXCLUDE]
            var teacher = new Teacher
            {
                UserId = teacherEmail
            };
            try
            {
                teacher = service.Courses.Teachers.Create(teacher, courseId).Execute();
                Console.WriteLine(
                        "User '{0}' was added as a teacher to the course with ID '{1}'.\n",
                        teacher.Profile.Name.FullName, courseId);
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    Console.WriteLine("User '{0}' is already a member of this course.\n",
                            teacherEmail);
                }
                else
                {
                    throw e;
                }
            }
            // [END AddTeacher]
            return teacher;
        }

        public static Student EnrollAsStudent(ClassroomService service, string _courseId,
                string _enrollmentCode)
        {
            // [START EnrollAsStudent]
            string courseId = "123456";
            string enrollmentCode = "abcdef";
            // [START_EXCLUDE silent]
            courseId = _courseId;
            enrollmentCode = _enrollmentCode;
            // [END_EXCLUDE]
            var student = new Student
            {
                UserId = "me"
            };
            try
            {
                var request = service.Courses.Students.Create(student, courseId);
                request.EnrollmentCode = enrollmentCode;
                student = request.Execute();
                Console.WriteLine(
                        "User '{0}' was enrolled  as a student in the course with ID '{1}'.\n",
                        student.Profile.Name.FullName, courseId);
            }
            catch (GoogleApiException e)
            {
                if (e.HttpStatusCode == HttpStatusCode.Conflict)
                {
                    Console.WriteLine("You are already a member of this course.\n");
                }
                else
                {
                    throw e;
                }
            }
            // [END EnrollAsStudent]
            return student;
        }

        public static void BatchAddStudents(ClassroomService service, string _courseId,
                List<string> _studentEmails)
        {
            // [START BatchAddStudents]
            string courseId = "123456";
            var studentEmails = new List<string>() { "alice@example.edu", "bob@example.edu" };
            // [START_EXCLUDE silent]
            courseId = _courseId;
            studentEmails = _studentEmails;
            // [END_EXCLUDE]
            var batch = new BatchRequest(service, "https://classroom.googleapis.com/batch");
            BatchRequest.OnResponse<Student> callback = (student, error, i, message) =>
            {
                if (error != null)
                {
                    Console.WriteLine("Error adding student to the course: {0}", error.Message);
                }
                else
                {
                    Console.WriteLine("User '{0}' was added as a student to the course.",
                        student.Profile.Name.FullName);
                }
            };
            foreach (var studentEmail in studentEmails)
            {
                var student = new Student() { UserId = studentEmail };
                var request = service.Courses.Students.Create(student, courseId);
                batch.Queue<Student>(request, callback);
            }
            Task.WaitAll(batch.ExecuteAsync());
            // [END BatchAddStudents]
        }
    }
}
