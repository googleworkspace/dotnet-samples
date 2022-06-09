using NUnit.Framework;

namespace DriveV2SnippetsTest;

public class CreateDriveTest :  BaseTest
{
    [Test]
        public void TestCreateDrive()
        {
            var id = DriveV2Snippets.CreateDrive.DriveCreateDrive();
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

}
