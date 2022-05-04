using System;
using NUnit.Framework;
using Google;
using System.Collections.Generic;
using System.Text;

namespace dotnet
{
    [TestFixture]
    public class FileSnippetsTest : BaseTest
    {

        FileSnippets snippets;

        public FileSnippetsTest()
        {
            this.snippets = new FileSnippets(this.service);
        }

        [Test]
        public void UploadBasic()
        {
            var id = this.snippets.UploadBasic();
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

        [Test]
        public void UploadToFolder()
        {
            var folderId = this.snippets.CreateFolder();
            DeleteFileOnCleanup(folderId);
            var file = this.snippets.UploadToFolder(folderId);
            Assert.IsNotNull(file);
            DeleteFileOnCleanup(file.Id);
        }

        [Test]
        public void UploadWithConversion()
        {
            var id = this.snippets.UploadWithConversion();
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

        [Test]
        public void ExportPdf()
        {
            // TODO - Currently broken on .NET due to client bug
            var id = this.CreateTestDocument();
            var fileStream = this.snippets.ExportPdf(id);
            var content = Encoding.UTF8.GetString(fileStream.ToArray());
            Assert.AreNotEqual(0, content.Length);
            Assert.AreEqual("%PDF", content.Substring(0, 4));
        }

        [Test]
        public void DownloadFile()
        {
            var id = this.CreateTestBlob();
            var fileStream = this.snippets.DownloadFile(id);
            var content = fileStream.GetBuffer();
            Assert.AreNotEqual(0, content.Length);
            Assert.AreEqual(0xFF, content[0]);
            Assert.AreEqual(0XD8, content[1]);
        }

        [Test]
        public void CreateShortcut()
        {
            var id = this.snippets.CreateShortcut();
            DeleteFileOnCleanup(id);
            Assert.IsNotNull(id);
        }

        [Test]
        public void TouchFile()
        {
            var id = this.CreateTestBlob();
            var now = DateTime.Now;
            var modifiedTime = this.snippets.TouchFile(id, now);
            Assert.AreEqual(now.ToString(), modifiedTime.Value.ToString());
        }

        [Test]
        public void CreateFolder()
        {
            var id = this.snippets.CreateFolder();
            DeleteFileOnCleanup(id);
            Assert.IsNotNull(id);
        }

        [Test]
        public void MoveFileToFolder()
        {
            var fileId = this.CreateTestBlob();
            var folderId = this.snippets.CreateFolder();
            DeleteFileOnCleanup(folderId);
            IList<string> parents = this.snippets.MoveFileToFolder(
                                        fileId, folderId);
            Assert.IsTrue(parents.Contains(folderId));
            Assert.AreEqual(1, parents.Count);
        }

        [Test]
        public void SearchFiles()
        {
            this.CreateTestBlob();
            var files = this.snippets.SearchFiles();
            Assert.AreNotEqual(0, files.Count);
        }

        [Test]
        public void ShareFile()
        {
            String fileId = this.CreateTestBlob();
            var ids = this.snippets.ShareFile(fileId,
                                              "user@test.appsdevtesting.com",
                                              "test.appsdevtesting.com");
            Assert.AreNotEqual(0, ids.Count);
        }
    }
}
