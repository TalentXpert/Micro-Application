using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Tests.Utilities.Extensions
{
    [TestClass]
    public class StringExtensionTest
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
