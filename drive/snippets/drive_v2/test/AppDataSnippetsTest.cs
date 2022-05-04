using System;
using NUnit.Framework;
using Google;
using System.Collections.Generic;
using System.Text;

namespace dotnet
{
    public class AppDataSnippetsTest : BaseTest
    {

        AppDataSnippets snippets;

        public AppDataSnippetsTest()
        {
            this.snippets = new AppDataSnippets(this.service);
        }

        [Test]
        public void UploadAppData()
        {
            var id = this.snippets.UploadAppData();
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

        [Test]
        public void ListAppData()
        {
            var id = this.snippets.UploadAppData();
            DeleteFileOnCleanup(id);
            var files = this.snippets.ListAppData();
            Assert.AreNotEqual(0, files.Items.Count);
        }

        [Test]
        public void FetchAppDataFolder()
        {
            var id = this.snippets.FetchAppDataFolder();
            Assert.IsNotNull(id);
        }
    }
}
