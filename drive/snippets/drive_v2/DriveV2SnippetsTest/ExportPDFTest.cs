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

using System.Text;
using DriveV2Snippets;
using NUnit.Framework;

namespace DriveV2SnippetsTest
{

  // Unit testcase for drive v2 export pdf snippet
  [TestFixture]
  public class ExportPdfTest : BaseTest
  {
    // TODO(developer) - Provide Absolute path of file.
    private string filePath = "files/document.txt";
    [Test]
    public void TestExportPdf()
    {
      var id = CreateTestDocument(filePath);
      var fileStream = ExportPdf.DriveExportPdf(id);
      var content = Encoding.UTF8.GetString(fileStream.ToArray());
      Assert.AreNotEqual(0, content.Length);
      Assert.AreEqual("%PDF", content.Substring(0, 4));
    }
  }
}