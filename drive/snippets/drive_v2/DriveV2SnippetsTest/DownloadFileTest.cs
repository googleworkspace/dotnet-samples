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
  // Unit testcase for drive v2 download file snippet
  [TestFixture]
  public class DownloadFileTest : BaseTest
  {
    // TODO(developer) - Provide Absolute path of file.
    private string filePath = "files/photo.jpg"; 
    [Test]
    public void TestDownloadFile()
    {
      var id = CreateTestBlob(filePath);
      var fileStream = DownloadFile.DriveDownloadFile(id);
      var content = fileStream.GetBuffer();
      Assert.AreNotEqual(0, content.Length);
      Assert.AreEqual(0xFF, content[0]);
      Assert.AreEqual(0XD8, content[1]);
    }
  }
}