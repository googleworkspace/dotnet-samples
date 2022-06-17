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
  // Unit testcase for drive v2 list appData snippet
  [TestFixture]
  public class ListAppDataTest : BaseTest
  {
    //TODO(developer) - Provide absolute path of the file
    private string filePath = "files/config.json"; 

    [Test]
    public void TestListAppData()
    {
      var id = UploadAppData.DriveUploadAppData(filePath);
      DeleteFileOnCleanup(id);
      var files = ListAppData.DriveListAppData();
      Assert.AreNotEqual(0, files.Items.Count);
    }
  }
}