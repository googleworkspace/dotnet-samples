using System;
using NUnit.Framework;
using Google;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace dotnet
{
    public class TeamDriveSnippetsTest : BaseTest
    {

        TeamDriveSnippets snippets;

        public TeamDriveSnippetsTest()
        {
            this.snippets = new TeamDriveSnippets(this.service);
        }

        [Test]
        public void CreateTeamDrive()
        {
            var id = this.snippets.CreateTeamDrive();
            Assert.IsNotNull(id);
            this.service.Teamdrives.Delete(id).Execute();
        }

        [Test]
        public void RecoverTeamDrives()
        {
            var id = this.CreateOrphanedTeamDrive();
            var results = this.snippets.RecoverTeamDrives("sbazyl@test.appsdevtesting.com");
            Assert.AreNotEqual(0, results.Count);
            this.service.Teamdrives.Delete(id).Execute();
        }

        private string CreateOrphanedTeamDrive()
        {
            var teamDriveId = this.snippets.CreateTeamDrive();
            var listRequest = this.service.Permissions.List(teamDriveId);
            listRequest.SupportsTeamDrives = true;
            var response = listRequest.Execute();

            foreach (var permission in response.Items)
            {
                var deleteRequest = this.service.Permissions.Delete(teamDriveId, permission.Id);
                deleteRequest.SupportsTeamDrives = true;
                deleteRequest.Execute();
            }
            return teamDriveId;
        }
    }
}
