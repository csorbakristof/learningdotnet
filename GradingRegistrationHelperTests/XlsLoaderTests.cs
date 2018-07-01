using ExcelReaderStandardLibrary;
using GradingRegistrationHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradingRegistrationHelperTests
{
    [TestClass]
    public class XlsLoaderTests
    {
        [TestMethod]
        public void TestSystem()
        {
        }


        class FakeReader : ITableReader
        {
            private List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            public List<Dictionary<string, string>> Read(string filename, int worksheetIndex, int headerRowIndex)
            {
                return list;
            }

            public void AddGrading(string name, string ncode, string gradedSubject, int? grade)
            {
                var keyname = XlsLoader.GradingGradeKey;
                throw new NotImplementedException();
            }

            public void AddAdvisor(string name, string ncode, string advisor)
            {
                throw new NotImplementedException();
            }

            public void AddAttendance(string name, string ncode, string attendance)
            {
                throw new NotImplementedException();
            }
        }
    }
}



