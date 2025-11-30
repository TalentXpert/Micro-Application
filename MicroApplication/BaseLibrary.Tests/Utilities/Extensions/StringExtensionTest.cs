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
    }
}
