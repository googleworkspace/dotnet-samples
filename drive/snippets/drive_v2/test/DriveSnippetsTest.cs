using System;
using NUnit.Framework;
using Google;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace dotnet
{
    public class DriveSnippetsTest : BaseTest
    {

        DriveSnippets snippets;

        public DriveSnippetsTest()
        {
            this.snippets = new DriveSnippets(this.service);
        }

        [Test]
        public void CreateDrive()
        {
            var id = this.snippets.CreateDrive();
            Assert.IsNotNull(id);
            this.service.Drives.Delete(id).Execute();
        }

        [Test]
        public void RecoverDrives()
        {
            var id = this.CreateOrphanedDrive();
            var results = this.snippets.RecoverDrives("sbazyl@test.appsdevtesting.com");
            Assert.AreNotEqual(0, results.Count);
            this.service.Drives.Delete(id).Execute();
        }

        private string CreateOrphanedDrive()
        {
            var driveId = this.snippets.CreateDrive();
            var listRequest = this.service.Permissions.List(driveId);
            listRequest.SupportsAllDrives = true;
            var response = listRequest.Execute();

            foreach (var permission in response.Items)
            {
                var deleteRequest = this.service.Permissions.Delete(driveId, permission.Id);
                deleteRequest.SupportsAllDrives = true;
                deleteRequest.Execute();
            }
            return driveId;
        }
    }
}
