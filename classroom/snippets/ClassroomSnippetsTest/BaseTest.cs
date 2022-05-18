using Google.Apis.Auth.OAuth2;
using Google.Apis.Classroom.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassroomSnippetsTest
{
    public class BaseTest
    {
        static string ApplicationName = "Classroom API .NET Snippet Tests";
        static string[] Scopes = {
            ClassroomService.Scope.ClassroomCourses,
            ClassroomService.Scope.ClassroomRosters
        };

        private UserCredential authorize()
        {
            ClientSecrets secrets;
            using (var stream = File.OpenRead("client_secret.json"))
            {
                secrets = GoogleClientSecrets.Load(stream).Secrets;
            }
            var credentialStorage = new FileDataStore("credentials", true);
            return GoogleWebAuthorizationBroker.AuthorizeAsync(
                secrets, Scopes, "user", CancellationToken.None, credentialStorage).Result;
        }

        protected ClassroomService getService()
        {
            var credential = authorize();
            return new ClassroomService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName,
                });
        }
    }
}
