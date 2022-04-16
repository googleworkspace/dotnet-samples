using System;
using Google.Apis.Drive.v3beta;
using Google.Apis.Drive.v3beta.Data;
using System.Collections.Generic;

namespace dotnet
{
    public class TeamDriveSnippets
    {
        private DriveService service;

        public TeamDriveSnippets(DriveService service)
        {
            this.service = service;
        }

        public string CreateTeamDrive()
        {
            var driveService = service;
            // [START createTeamDrive]
            var teamDriveMetadata = new TeamDrive()
            {
                Name = "Project Resources"
            };
            var requestId = System.Guid.NewGuid().ToString();
            var request = driveService.Teamdrives.Create(teamDriveMetadata, requestId);
            request.Fields = "id";
            var teamDrive = request.Execute();
            Console.WriteLine("Team Drive ID: " + teamDrive.Id);
            // [END createTeamDrive]
            return teamDrive.Id;
        }

        public IList<TeamDrive> RecoverTeamDrives(string realUser)
        {
            var driveService = service;
            var teamDrives = new List<TeamDrive>();
            // [START recoverTeamDrives]
            // Find all Team Drives without an organizer and add one.
            // Note: This example does not capture all cases. Team Drives
            // that have an empty group as the sole organizer, or an
            // organizer outside the organization are not captured. A
            // more exhaustive approach would evaluate each Team Drive
            // and the associated permissions and groups to ensure an active
            // organizer is assigned.
            string pageToken = null;
            var newOrganizerPermission = new Permission()
            {
                Type = "user",
                Role = "organizer",
                EmailAddress = "user@example.com"
            };
            // [START_EXCLUDE silent]
            newOrganizerPermission.EmailAddress = realUser;
            // [END_EXCLUDE]

            do
            {
                var request = driveService.Teamdrives.List();
                request.UseDomainAdminAccess = true;
                request.Q = "organizerCount = 0";
                request.Fields = "nextPageToken, teamDrives(id, name)";
                request.PageToken = pageToken;
                var result = request.Execute();
                foreach (var teamDrive in result.TeamDrives)
                {
                    Console.WriteLine(string.Format(
                           "Found abandoned Team Drive: {0} ({1})",
                           teamDrive.Name, teamDrive.Id));
                    // Note: For improved efficiency, consider batching
                    // permission insert requests
                    var permissionRequest = driveService.Permissions.Create(
                      newOrganizerPermission,
                      teamDrive.Id
                    );
                    permissionRequest.UseDomainAdminAccess = true;
                    permissionRequest.SupportsTeamDrives = true;
                    permissionRequest.Fields = "id";
                    var permissionResult = permissionRequest.Execute();
                    Console.WriteLine(string.Format(
                           "Added organizer permission: {0}", permissionResult.Id));

                }
                // [START_EXCLUDE silent]
                teamDrives.AddRange(result.TeamDrives);
                // [END_EXCLUDE]
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            // [END recoverTeamDrives]
            return teamDrives;
        }

    }
}
