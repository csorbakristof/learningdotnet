using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turkmite;
using OpenCvSharp;

namespace TurkmiteTests
{
    /// <summary>
    /// Summary description for OriginalTurkmiteTests
    /// </summary>
    [TestClass]
    public class OriginalTurkmiteTests
    {
        TestOriginalTurkmite turkmite = new TestOriginalTurkmite(new Mat(10, 10, MatType.CV_8UC3, new Scalar(0, 0, 0)));

        [TestMethod]
        public void BlackField_Correct()
        {
            var result = turkmite.GetNextColorAndUpdateDirection(turkmite.Black);
            Assert.AreEqual(turkmite.White, result.newColor);
            Assert.AreEqual(1, result.deltaDirection);
        }

        [TestMethod]
        public void WhiteField_Correct()
        {
            var result = turkmite.GetNextColorAndUpdateDirection(turkmite.White);
            Assert.AreEqual(turkmite.Black, result.newColor);
            Assert.AreEqual(-1, result.deltaDirection);
        }

        private class TestOriginalTurkmite : OriginalTurkmite
        {
            public new (Vec3b newColor, int deltaDirection) GetNextColorAndUpdateDirection(Vec3b currentColor)
            {
                return base.GetNextColorAndUpdateDirection(currentColor);
            }

            public Vec3b White { get { return white; } }
            public Vec3b Black { get { return black; } }

            public TestOriginalTurkmite(Mat image) : base(image)
            {
            }
        }
    }
}
