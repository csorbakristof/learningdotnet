using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelHelper;

namespace ExcelHelperTest
{
    /// <summary>
    /// Summary description for StudentBasicTests
    /// </summary>
    [TestClass]
    public class StudentBasicTests
    {
        public StudentBasicTests()
        {
        }

        [TestMethod]
        public void Istantiation()
        {
            var s = new Student();
        }
    }
}
