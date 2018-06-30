using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestingWithMoq
{
    [TestClass]
    public class StuffTests
    {
        [TestMethod]
        public void StuffBaseInstantiates()
        {
            var sb = new StuffBase(null,null);
        }

        [TestMethod]
        public void StuffBaseUsesSource()
        {
            var srcMock = new Moq.Mock<ISource>();
            srcMock.Setup(i => i.GetValue()).Returns(3);
            var sb = new StuffBase(srcMock.Object, null);
            Assert.AreEqual(3, sb.GetValueFromInternalSource());
        }

        [TestMethod]
        public void StuffCallsCommand()
        {
            var mockCmd = new Moq.Mock<ICommand>();
            var mockSrc = new Moq.Mock<ISource>();
            mockSrc.Setup(i => i.GetValue()).Returns(1);
            var sb = new StuffBase(mockSrc.Object, mockCmd.Object);
            sb.ExecuteCommandIfSourceIsNonzero();
            mockCmd.Verify(i => i.Execute());
        }

        [TestMethod]
        public void StuffCallsVisitor()
        {
            var mockVisitor = new Moq.Mock<IVisitor>();
            var sb = new StuffBase(null,null);
            sb.Accept(mockVisitor.Object);
            mockVisitor.Verify(i => i.Visit(sb));
        }
    }
}
