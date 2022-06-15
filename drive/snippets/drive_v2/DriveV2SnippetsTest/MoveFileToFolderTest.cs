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
  // Unit testcase for drive v2 move file to folder snippet
  [TestFixture]
  public class MoveFileToFolderTest : BaseTest
  {
    //TODO(developer) - Provide absolute path of the file
    private string filePath = "files/document.txt";
    [Test]
    public void TestMoveFileToFolder()
    {
      var fileId = CreateTestBlob(filePath);
      var folderId = CreateFolder.DriveCreateFolder();
      IList<string> parents = MoveFileToFolder.DriveMoveFileToFolder(
        fileId, folderId);
      Assert.IsTrue(parents.Contains(folderId));
      Assert.AreEqual(1, parents.Count);
      DeleteFileOnCleanup(folderId);
    }
  }
}