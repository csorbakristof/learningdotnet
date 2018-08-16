
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PeerReviewResultDistributionHelperTests
{
    [TestClass]
    public partial class BasicReviewTests
    {
        class TestCase
        {
            public string ReviewText;
            public int ReviewOverallScore;
            public string ReviewerName;
            public string ReviewerNeptunCode;
            public string PresenterEmail;
            public string AdvisorName;
            public string AdvisorEmail;
        }

        private TestCase[] testCases;

        public BasicReviewTests()
        {
            // Note:
            //  all 3 reviewers are different
            //  case 1 has same presenter as case 0
            //  case 2 has same advisor as case 0

            testCases = new TestCase[3];
            testCases[0] = new TestCase()
            {
                ReviewText = "Arbitrary review message 1",
                ReviewOverallScore = 1,
                ReviewerName = "Ertekelo Elek",
                ReviewerNeptunCode = "ABC123",
                PresenterEmail = "presenter@example.com",
                AdvisorName = "Konzulens Klára",
                AdvisorEmail = "advisor@example.com"
            };
            testCases[1] = new TestCase()
            {
                ReviewText = "Arbitrary review message 2",
                ReviewOverallScore = 2,
                ReviewerName = "Ertekelo Ferenc",
                ReviewerNeptunCode = "DEF456",
                PresenterEmail = testCases[0].PresenterEmail,
                AdvisorName = "Konzulens Kázmér",
                AdvisorEmail = "advisor@example.com"
            };
            testCases[2] = new TestCase()
            {
                ReviewText = "Arbitrary review message 3",
                ReviewOverallScore = 3,
                ReviewerName = "Dummy Dénes",
                ReviewerNeptunCode = "DDD777",
                PresenterEmail = "otherPresenter@example.com",
                AdvisorName = testCases[0].AdvisorName,
                AdvisorEmail = testCases[0].AdvisorEmail
            };
        }

        private Review CreateTestReview(TestCase testCase)
        {
            var review = new Review();
            review.Text = testCase.ReviewText;
            review.OverallScore = testCase.ReviewOverallScore;
            review.ReviewerNeptunCode = testCase.ReviewerNeptunCode;
            review.PresenterEmail = testCase.PresenterEmail;
            return review;
        }

        [TestMethod]
        public void GeneratesPresenterEmailCorrectly()
        {
            var r = CreateTestReview(testCases[0]);
            var m = r.GetPresenterEmail();
            Assert.AreEqual(testCases[0].PresenterEmail, m.To[0].Address);
            AssertEmailBodyForPresenter(m, 0);
        }


        [TestMethod]
        public void SupervisionLookupTest()
        {
            var lookup = new SupervisionLookupBase();
            var s = CreateTestSupervision(testCases[0]);
            lookup.Add(testCases[0].ReviewerNeptunCode, s);
            var result = lookup.GetSupervision(testCases[0].ReviewerNeptunCode);
            Assert.AreSame(s, result);
        }

        [TestMethod]
        public void GeneratesAdvisorEmailCorrectly()
        {
            var r = CreateTestReview(testCases[0]);

            var lookup = new SupervisionLookupBase();
            var s = CreateTestSupervision(testCases[0]);
            lookup.Add(testCases[0].ReviewerNeptunCode, s);

            var m = r.GetAdvisorEmail(lookup);
            Assert.AreEqual(testCases[0].AdvisorEmail, m.To[0].Address);
            Assert.IsFalse(m.Body.Contains(testCases[0].AdvisorEmail));
            Assert.IsTrue(m.Body.Contains(testCases[0].ReviewText));
            Assert.IsTrue(m.Body.Contains($"pontszám: {testCases[0].ReviewOverallScore}"));
            Assert.IsTrue(m.Body.Contains(testCases[0].ReviewerName));
            Assert.IsTrue(m.Body.Contains(testCases[0].ReviewerNeptunCode));
            Assert.IsTrue(m.Body.Contains(testCases[0].AdvisorName));
        }

        Supervision CreateTestSupervision(TestCase testCase)
        {
            return new Supervision() {
                StudentName = testCase.ReviewerName,
                StudentNeptunCode = testCase.ReviewerNeptunCode,
                AdvisorName = testCase.AdvisorName,
                AdvisorEmail = testCase.AdvisorEmail };
        }

        [TestMethod]
        public void SupervisionLookupBaseWithMultipleEntries()
        {
            var lookup = new SupervisionLookupBase();
            lookup.Add(testCases[0].ReviewerNeptunCode, CreateTestSupervision(testCases[0]));
            lookup.Add(testCases[1].ReviewerNeptunCode, CreateTestSupervision(testCases[1]));
            Assert.AreEqual(testCases[0].AdvisorName, lookup.GetSupervision(testCases[0].ReviewerNeptunCode).AdvisorName);
        }

        [TestMethod]
        public void MultipleReviewsForSamePresenter_EmailCollectsReviews()
        {
            (var reviews, var s) = CreateFullTestCase();

            var mail = Review.GetCollectedPresenterEmail(testCases[0].PresenterEmail, reviews, s);
            // Note: test cases 0 and 1 have same presenter
            Assert.AreEqual(testCases[0].PresenterEmail, mail.To[0].Address);
            foreach (int idx in new int[] { 0, 1 })
                AssertEmailBodyForPresenter(mail, idx);
        }

        private (List<Review> reviews, SupervisionLookupBase s) CreateFullTestCase()
        {
            var reviews = new List<Review>();
            var s = new SupervisionLookupBase();
            foreach (var tc in testCases)
            {
                reviews.Add(CreateTestReview(tc));
                s.Add(tc.ReviewerNeptunCode, CreateTestSupervision(tc));
            }
            return (reviews, s);
        }

        private void AssertEmailBodyForPresenter(Windows.ApplicationModel.Email.EmailMessage mail, int idx)
        {
            Assert.IsTrue(mail.Body.Contains($"pontszám: {testCases[idx].ReviewOverallScore}"));
            Assert.IsTrue(mail.Body.Contains(testCases[idx].ReviewText));
            Assert.IsFalse(mail.Body.Contains(testCases[idx].AdvisorName));
            Assert.IsFalse(mail.Body.Contains(testCases[idx].ReviewerName));
            Assert.IsFalse(mail.Body.Contains(testCases[idx].ReviewerNeptunCode));
        }
    }
}
