using DriveV2Snippets;
using NUnit.Framework;

namespace DriveV2SnippetsTest;

public class UploadWithConversionTest :  BaseTest
{
    //TODO(developer) - Provide absolute path of the file
    private string filePath = "files/report.csv"; 
    [Test]
        public void TestUploadWithConversifon()
        {
            var id = UploadWithConversion.DriveUploadWithConversion(filePath);
            Assert.IsNotNull(id);
            DeleteFileOnCleanup(id);
        }

}
