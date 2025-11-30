namespace BaseLibrary.Tests.Utilities.Extensions
{
    [TestClass]
    public class ReflectionExtensionTest
    {
        [TestMethod]
        public void Test()
        {
            var strExt = new BaseLibrary.Utilities.StringExtension();
            var result = strExt.GetAllWordStartWith("FormCount FROM [dbo].[StudyForms] where studyId=@studyId");
            Assert.AreEqual("@studyId", result.First());
        }
    }
}
