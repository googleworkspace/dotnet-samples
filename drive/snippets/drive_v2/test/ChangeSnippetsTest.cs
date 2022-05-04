using System;
using NUnit.Framework;

namespace dotnet
{
    public class ChangeSnippetsTest : BaseTest
    {
        ChangeSnippets snippets;

        public ChangeSnippetsTest()
        {
            this.snippets = new ChangeSnippets(this.service);
        }

        [Test]
        public void FetchStartPageToken()
        {
            string token = snippets.FetchStartPageToken();
            Assert.IsNotNull(token);
        }

        [Test]
        public void FetchChanges()
        {
            string startPageToken = snippets.FetchStartPageToken();
            CreateTestBlob();
            string newStartPageToken = snippets.FetchChanges(startPageToken);
            Assert.IsNotNull(newStartPageToken);
            Assert.AreNotEqual(startPageToken, newStartPageToken);
        }
    }
}
