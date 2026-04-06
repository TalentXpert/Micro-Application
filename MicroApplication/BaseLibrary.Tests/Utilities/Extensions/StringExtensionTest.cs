using BaseLibrary.DTOs;
using BaseLibrary.Utilities;
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
            var fields = ReflectionExtension.GetAllPublicStaticFields<DatabaseOption>();
            Assert.AreEqual(1, fields.Count());
        }

        [TestMethod]
        public void Test2() 
        {
            var tuples = new List<Tuple<string, string>>
            {
                Tuple.Create("Dosimetry Urine Collection(Dosimetry Phase)10","Dosimetry Urine Collection"),
                Tuple.Create("Dosimetry Blood Collection (Dosimetry Phase)9","Dosimetry Blood Collection")

            };
            foreach (var tuple in tuples) 
            {
                var result = X.Extension.String.IsEndWithNumber(tuple.Item1);
                Assert.AreEqual(true, result);
                var cleanText = X.Extension.String.RemoveEndNumber(tuple.Item1);
                cleanText = X.Extension.String.RemoveEveryThingBetweenParanthesis(cleanText);
                Assert.AreEqual(cleanText, tuple.Item2);
            }
            

        }
        [TestMethod]
        public void Test3()
        {
            var text = "Standard of care imaging (e.g., contrast CT scan of chest, abdomen, pelvis)\"";
            //var result = X.Extension.String.IsEndWithNumber(text);
            //Assert.AreEqual(true, result);
            var cleanText = X.Extension.String.RemoveEndNumber(text);
            cleanText = X.Extension.String.RemoveEveryThingBetweenParanthesis(cleanText);
            Assert.AreEqual(cleanText, "Standard of care imaging");
        }
    }
}
