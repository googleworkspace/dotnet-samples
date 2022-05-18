using System;
using Google.Apis.Drive.v3beta;
using Google.Apis.Drive.v3beta.Data;
using System.Collections.Generic;

namespace dotnet
{
    public class DriveSnippets
    {
        private DriveService service;

        public DriveSnippets(DriveService service)
        {
            this.service = service;
        }

        public string CreateDrive()
        {
            var driveService = service;
            // [START createDrive]
            var driveMetadata = new Drive()
            {
                Name = "Project Resources"
            };
            var requestId = System.Guid.NewGuid().ToString();
            var request = driveService.Drives.Create(driveMetadata, requestId);
            request.Fields = "id";
            var drive = request.Execute();
            Console.WriteLine("Drive ID: " + drive.Id);
            // [END createDrive]
            return drive.Id;
        }

        public IList<Drive> RecoverDrives(string realUser)
        {
            var driveService = service;
            var drives = new List<Drive>();
            // [START recoverDrives]
            // Find all shared drives without an organizer and add one.
            // Note: This example does not capture all cases. Shared drives
            // that have an empty group as the sole organizer, or an
            // organizer outside the organization are not captured. A
            // more exhaustive approach would evaluate each shared drive
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
                var request = driveService.Drives.List();
                request.UseDomainAdminAccess = true;
                request.Q = "organizerCount = 0";
                request.Fields = "nextPageToken, drives(id, name)";
                request.PageToken = pageToken;
                var result = request.Execute();
                foreach (var drive in result.Drives)
                {
                    Console.WriteLine(string.Format(
                           "Found abandoned shared drive: {0} ({1})",
                           drive.Name, drive.Id));
                    // Note: For improved efficiency, consider batching
                    // permission insert requests
                    var permissionRequest = driveService.Permissions.Create(
                      newOrganizerPermission,
                      drive.Id
                    );
                    permissionRequest.UseDomainAdminAccess = true;
                    permissionRequest.SupportsAllDrives = true;
                    permissionRequest.Fields = "id";
                    var permissionResult = permissionRequest.Execute();
                    Console.WriteLine(string.Format(
                           "Added organizer permission: {0}", permissionResult.Id));

                }
                // [START_EXCLUDE silent]
                drives.AddRange(result.Drives);
                // [END_EXCLUDE]
                pageToken = result.NextPageToken;
            } while (pageToken != null);
            // [END recoverDrives]
            return drives;
        }

    }
}
