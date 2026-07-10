using BaseLibrary.DTOs;
using BaseLibrary.Utilities;
using BaseLibrary.Utilities.Files;

namespace BaseLibrary.Tests.Utilities.Extensions
{
    [TestClass]
    public class RegExExtensionTest
    {
        [TestMethod]
        public void Test()
        {
            var r = new RegExExtension();
            var text =  @"BioMeta Intelligent Data Management Plan (DMP) Master Template\r\nEnterprise master template for AI-assisted Data Management Plans.\r\nSections marked <<BIOMETA:...>> are intended for automatic population from protocol-specific information.\r\nCover Page\r\nAI-populated fields:\r\n•\t<<BIOMETA: Protocol Title>>\r\n•\t<<BIOMETA: Protocol Number>>\r\n•\t<<BIOMETA: Sponsor>>\r\n•\t<<BIOMETA: Phase>>\r\n\r\n__________________________________________\r\nRevision History\r\nAI-populated fields:\r\n•\tRevision log\r\n•\tApproval signatures\r\n\r\n__________________________________________\r\nScope\r\nAI-populated fields:\r\n•\t<<BIOMETA: Study Design Summary>>\r\n\r\n__________________________________________\r\n\r\nStudy Overview\r\nAI-populated fields:\r\n•\t<<BIOMETA: Indication>>\r\n•\t<<BIOMETA: Objectives>>\r\n•\t<<BIOMETA: Planned Enrollment>>\r\n•\t<<BIOMETA: Countries>>\r\n•\t<<BIOMETA: Sites>>";
            var result =  r.GetAllOccuranceOfWordMatchingStartAndEndPatter(text, "<<BIOMETA:", ">>");
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        //[TestMethod]
        //public void Test2() 
        //{
        //    var r = new RegExExtension();
        //    var text = DocxFileReader.ConvertDocxFileInToText(@"D:\BioMeta_Intelligent_DMP_Master_Template_v1.0.docx");
        //    var result = r.GetAllOccuranceOfWordMatchingStartAndEndPatter(text, "<<BIOMETA:", ">>");
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(1, result.Count());
        //}
    }
}
