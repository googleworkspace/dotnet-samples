using Google.Apis.Drive.v2beta;
using System;

namespace dotnet
{
    public class ChangeSnippets
    {
        private DriveService service;

        public ChangeSnippets(DriveService service)
        {
            this.service = service;
        }


        public string FetchStartPageToken()
        {
            var driveService = service;
            // [START fetchStartPageToken]
            var response = driveService.Changes.GetStartPageToken().Execute();
            Console.WriteLine("Start token: " + response.StartPageTokenValue);
            // [END fetchStartPageToken]
            return response.StartPageTokenValue;
        }

        public string FetchChanges(string savedStartPageToken)
        {
            var driveService = service;
            // [START fetchChanges]
            // Begin with our last saved start token for this user or the
            // current token from GetStartPageToken()
            string pageToken = savedStartPageToken;
            while (pageToken != null)
            {
                var request = driveService.Changes.List();
                request.PageToken = pageToken;
                request.Spaces = "drive";
                var changes = request.Execute();
                foreach (var change in changes.Items)
                {
                    // Process change
                    Console.WriteLine("Change found for file: " + change.FileId);
                }
                if (changes.NewStartPageToken != null)
                {
                    // Last page, save this token for the next polling interval
                    savedStartPageToken = changes.NewStartPageToken;
                }
                pageToken = changes.NextPageToken;
            }
            // [END fetchChanges]
            return savedStartPageToken;
        }
    }
}
