using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenCvSharp;
using Turkmite;

namespace TurkmiteTests
{
    [TestClass]
    public class TurkmiteBaseTests
    {
        private TestTurkmiteBase turkmite = new TestTurkmiteBase(new Mat(10, 10, MatType.CV_8UC3, new Scalar(0,0,0)));

        [TestMethod]
        public void GetNextColorAndUpdateDirection_IsCalled()
        {
            turkmite.Step();
            Assert.IsTrue(turkmite.GetNextColorAndUpdateDirectionInvoked);
        }

        [TestMethod]
        public void PerformMove_DirectionCorrect()
        {
            turkmite.X = 5;
            turkmite.Y = 5;
            turkmite.D = 0;
            turkmite.PerformMove(0);
            AssertTurkmiteState(5, 4, 0);
            turkmite.PerformMove(1);
            AssertTurkmiteState(6, 4, 1);
            turkmite.PerformMove(1);
            AssertTurkmiteState(6, 5, 2);
            turkmite.PerformMove(1);
            AssertTurkmiteState(5, 5, 3);
            turkmite.PerformMove(1);
            AssertTurkmiteState(5, 4, 0);
            turkmite.PerformMove(-1);
            AssertTurkmiteState(4, 4, 3);
        }

        [TestMethod]
        public void ImageBoundaryWorks()
        {
            AssertMove(5, 0, 0, 5, 0);
            AssertMove(5, 9, 2, 5, 9);
            AssertMove(9, 5, 1, 9, 5);
            AssertMove(0, 5, 3, 0, 5);
        }

        [TestMethod]
        public void StepUpdatesCorrectly()
        {
            turkmite.X = 5;
            turkmite.Y = 5;
            turkmite.D = 0;
            turkmite.Step();
            var indexer = turkmite.Image.GetGenericIndexer<Vec3b>();
            var newColor = indexer[5, 5];
            Assert.AreEqual(turkmite.ReturnedColor, newColor);
            // Note: mock returns "turn right".
            AssertTurkmiteState(6, 5, 1);
        }

        private void AssertMove(int startX, int startY, int direction, int finalX, int finalY)
        {
            turkmite.X = startX;
            turkmite.Y = startY;
            turkmite.D = direction;
            turkmite.PerformMove(0);
            AssertTurkmiteState(finalX, finalY, direction);
        }

        private void AssertTurkmiteState(int x, int y, int d)
        {
            Assert.AreEqual(x, turkmite.X);
            Assert.AreEqual(y, turkmite.Y);
            Assert.AreEqual(d, turkmite.D);
        }

        class TestTurkmiteBase : TurkmiteBase
        {
            public int X { get { return this.x; } set { this.x = value; } }
            public int Y { get { return this.y; } set { this.y = value; } }
            public int D { get { return this.direction; } set { this.direction = value; } }
            public bool GetNextColorAndUpdateDirectionInvoked = false;

            public new void PerformMove(int deltaDirection)
            {
                base.PerformMove(deltaDirection);
            }

            public readonly Vec3b ReturnedColor = new Vec3b(1,1,1);

            protected override (Vec3b newColor, int deltaDirection) GetNextColorAndUpdateDirection(Vec3b currentColor)
            {
                // Mocked to monitor invocation
                GetNextColorAndUpdateDirectionInvoked = true;
                return (ReturnedColor, 1);
            }

            public TestTurkmiteBase(Mat img) : base(img)
            {
            }

            public override int PreferredIterationCount => 0;
        }
    }
}
