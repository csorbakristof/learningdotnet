
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeerReviewResultDistributionHelperTests
{
    [TestClass]
    public partial class BasicReviewTests
    {
        [TestMethod]
        public void CanSetProperties()
        {
            CreateTestReview();
        }

        const string ReviewText = "Arbitrary review message";
        const int ReviewOverallScore = 5;
        const string ReviewerName = "Ertekelo Elek";
        const string ReviewerNeptunCode = "ABC123";
        const string PresenterEmail = "presenter@example.com";
        const string AdvisorName = "Konzulens Klára";
        const string AdvisorEmail = "advisor@example.com";

        private Review CreateTestReview()
        {
            var review = new Review();
            review.Text = ReviewText;
            review.OverallScore = ReviewOverallScore;
            review.ReviewerNeptunCode = ReviewerNeptunCode;
            review.PresenterEmail = PresenterEmail;
            return review;
        }

        [TestMethod]
        public void GeneratesPresenterEmailCorrectly()
        {
            var r = CreateTestReview();
            var m = r.GetPresenterEmail();
            Assert.AreEqual(PresenterEmail, m.To[0].Address);
            Assert.IsTrue(m.Body.Contains($"pontszám: {ReviewOverallScore}"));
            Assert.IsTrue(m.Body.Contains(ReviewText));
            Assert.IsFalse(m.Body.Contains(AdvisorName));
            Assert.IsFalse(m.Body.Contains(ReviewerName));
            Assert.IsFalse(m.Body.Contains(ReviewerNeptunCode));
        }


        [TestMethod]
        public void SupervisionLookupTest()
        {
            var lookup = new SupervisionLookupBase();
            var s = CreatePresetSupervision();
            lookup.Add(ReviewerNeptunCode, s);
            var result = lookup.GetSupervision(ReviewerNeptunCode);
            Assert.AreSame(s, result);
        }

        [TestMethod]
        public void GeneratesAdvisorEmailCorrectly()
        {
            var r = CreateTestReview();

            var lookup = new SupervisionLookupBase();
            var s = CreatePresetSupervision();
            lookup.Add(ReviewerNeptunCode, s);

            var m = r.GetAdvisorEmail(lookup);
            Assert.AreEqual(AdvisorEmail, m.To[0].Address);
            Assert.IsFalse(m.Body.Contains(AdvisorEmail));
            Assert.IsTrue(m.Body.Contains(ReviewText));
            Assert.IsTrue(m.Body.Contains($"pontszám: {ReviewOverallScore}"));
            Assert.IsTrue(m.Body.Contains(ReviewerName));
            Assert.IsTrue(m.Body.Contains(ReviewerNeptunCode));
            Assert.IsTrue(m.Body.Contains(AdvisorName));
        }

        Supervision CreatePresetSupervision()
        {
            return new Supervision() { StudentName = ReviewerName, StudentNeptunCode = ReviewerNeptunCode, AdvisorName = AdvisorName, AdvisorEmail = AdvisorEmail };
        }

        Supervision CreateDummySupervision()
        {
            return new Supervision() {
                StudentName = "DummyStudent",
                StudentNeptunCode = "DEF456",
                AdvisorName = "DummyAdvisor",
                AdvisorEmail = "dummyAdvisor@example.com"
            };
        }


        [TestMethod]
        public void SupervisionLookupBaseWithMultipleEntries()
        {
            var lookup = new SupervisionLookupBase();
            lookup.Add(ReviewerNeptunCode, CreatePresetSupervision());
            lookup.Add("DEF456", CreateDummySupervision());
            Assert.AreEqual(AdvisorName, lookup.GetSupervision(ReviewerNeptunCode).AdvisorName);
        }
    }
}
