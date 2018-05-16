
using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Classroom.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassroomQuickstart
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/classroom.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { ClassroomService.Scope.ClassroomCoursesReadonly };
        static string ApplicationName = "Classroom API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/classroom.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Classroom API service.
            var service = new ClassroomService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            CoursesResource.ListRequest request = service.Courses.List();
            request.PageSize = 10;

            // List courses.
            ListCoursesResponse response = request.Execute();
            Console.WriteLine("Courses:");
            if (response.Courses != null && response.Courses.Count > 0)
            {
                foreach (var course in response.Courses)
                {
                    Console.WriteLine("{0} ({1})", course.Name, course.Id);
                }
            }
            else
            {
                Console.WriteLine("No courses found.");
            }
            Console.Read();

        }
    }
}