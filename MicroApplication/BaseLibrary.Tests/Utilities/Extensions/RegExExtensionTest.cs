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

        [TestMethod]
        public void ReplaceTagsTest()
        {
            var data = GetData();
            var r = new RegExExtension();
            var result = DocxFileReader.ReplaceTags(@"D:\BioMeta_Intelligent_DMP_Master_Template_v1.0.docx", @"D:\StudyDocuments", data);
            Assert.IsTrue(result);
        }

        private Dictionary<string,string> GetData()
        {
            var result = new Dictionary<string, string>();
            result["<<BIOMETA: Protocol Title>>"] = @"Open-label, randomized, multicenter, phase IV trial comparing parenteral nutrition using Eurotubes® vs. traditional 2/3-chamber bags in subjects with metastatic or localized solid tumors requiring parenteral nutrition – The PEKANNUSS Trial";
            result["<<BIOMETA: Sponsor>>"] = @"Institut für Klinische Krebsforschung IKF GmbH at Krankenhaus Nordwest Steinbacher Hohl 2-26 60488 Frankfurt am Main";
            result["<<BIOMETA: Protocol Number>>"] = @"PEKANNUSS";
            result["<<BIOMETA: Phase>>"] = "Phase 1";
            result["<<BIOMETA: Study Design Summary>>"] = @"This is an open-label, randomized, multicenter, investigator-initiated, phase IV trial. A total number of 216 patients will be enrolled (see chapter 7 for statistical calculations).
Patients with metastatic or localized solid tumors who fulfil the eligibility criteria and who have an indication for parenteral nutrition will be enrolled.
Patients will be stratified according to ECOG (0-1 vs. 2 vs. 3), the modified Glasgow Prognostic Score (mGPS) (0-1 vs. 2) and whether the patient receives concurrent systemic anti-cancer treatment (e.g. chemotherapy, targeted therapy, immunotherapy) or not.
In a first step, patients will be randomized in a 2:1 ratio to Arm A or Arm B:
Arm A: Standard Parenteral Nutrition using Eurotubes®.
or
Arm B: Standard Parenteral Nutrition using 2/3-chamber bags.
Patients randomized to Arm B will receive PN according to the routine used by the participating site.
Patients in Arm A will be stratified again by the same criteria as listed above and randomized in a 1:1 ratio to Arm A-1 or Arm A-2:
Arm A-1: Standard Low Glucose Parenteral Nutrition using Eurotubes®.
Patients randomized to Arm A and in a second randomization to treatment Arm A-1 receive PN reduced in glucose in Eurotubes®.
or
Arm A-2: Standard Parenteral Nutrition using Eurotubes®.
Patients randomized to Arm A and in a second randomization to treatment Arm A-2 will receive standard PN in Eurotubes®.";
            return result;
        }

    }
}
