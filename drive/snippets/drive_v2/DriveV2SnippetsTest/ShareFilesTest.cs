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
  // Unit testcase for drive v2 share file snippet
  [TestFixture]
  public class ShareFileTest : BaseTest
  {
    //TODO(developer) - Provide absolute path of the file
    private string filePath = "files/photo.jpg";

    [Test]
    public void TestShareFile()
    {
      String fileId = CreateTestBlob(filePath);
      var ids = ShareFile.DriveShareFile(fileId,
        "user@test.appsdevtesting.com",
        "test.appsdevtesting.com");
      Assert.AreNotEqual(0, ids.Count);
      DeleteFileOnCleanup(fileId);
    }
  }
}