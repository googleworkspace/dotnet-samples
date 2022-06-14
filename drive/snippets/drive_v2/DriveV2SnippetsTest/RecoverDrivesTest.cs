// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using DriveV2Snippets;
using NUnit.Framework;

namespace DriveV2SnippetsTest
{
  // Unit testcase for drive v2 recover drives snippet
  [TestFixture]
  public class RecoverDrivesTest : BaseTest
  {
    [Test]
    public void TestRecoverDrives()
    {
      var id = CreateOrphanedDrive();
      var results = RecoverDrives.DriveRecoverDrives("gduser1@workspacesamples.dev");
      Assert.AreNotEqual(0, results.Count);
      this.service.Drives.Delete(id).Execute();
    }

    private string CreateOrphanedDrive()
    {
      var driveId = CreateDrive.DriveCreateDrive();
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