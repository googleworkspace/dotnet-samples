using NUnit.Framework;

namespace DriveV2SnippetsTest;

public class CreateFolderTest :  BaseTest
{
    [Test]
        public void TestCreateFolder()
        {
            var id = DriveV2Snippets.CreateFolder.DriveCreateFolder();
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

}
