using ExcelReaderStandardLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Storage;

namespace PeerReviewCommonLib
{
    public class Processor
    {
        public void LoadReviews(StorageFile reviewsFile)
        {
            AddReviews(LoadXlsIntoDictionaryList(reviewsFile, 0, 1));
        }

        public void LoadSupervisions(StorageFile supervisionsFile)
        {
            AddSupervisions(LoadXlsIntoDictionaryList(supervisionsFile, 1, 1));
        }

        public IEnumerable<EmailMessage> CreateStudentEmails()
        {
            var allPresenters = Reviews.Select(r => r.PresenterEmail).Distinct();
            var supervisionLookup = new SupervisionLookupBase();
            foreach (var s in Supervisions)
                supervisionLookup.AddIfNew(s.StudentNeptunCode, s);
            var allReviews = Reviews.ToList();
            foreach (var presenterEmail in allPresenters)
                yield return Review.GetCollectedPresenterEmail(presenterEmail, allReviews);
        }

        public IEnumerable<EmailMessage> CreateAdvisorEmails()
        {
            var supervisionLookup = new SupervisionLookupBase();
            foreach (var s in Supervisions)
                supervisionLookup.AddIfNew(s.StudentNeptunCode, s);

            var allAdvisorNames = Reviews.Select(r => supervisionLookup.GetSupervision(r.ReviewerNeptunCode).AdvisorName).Distinct();
            var allReviews = Reviews.ToList();
            foreach (var advisorName in allAdvisorNames)
            {
                var advisorEmail = GetEmailForAdvisor(advisorName);
                yield return Review.GetCollectedAdvisorEmail(advisorEmail, advisorName, allReviews);
            }
        }

        public void MatchAdvisorsToReviewers()
        {
            var supervisionLookup = new SupervisionLookupBase();
            foreach (var s in Supervisions)
                supervisionLookup.AddIfNew(s.StudentNeptunCode, s);

            foreach (var r in Reviews)
                r.ConnectReviewersSupervision(supervisionLookup);
        }

        private string GetEmailForAdvisor(string advisorName)
        {
            return advisorName; // TODO
        }

        private List<Review> Reviews = new List<Review>();
        private List<Supervision> Supervisions = new List<Supervision>();

        #region XLS loading helpers
        private void AddReviews(List<Dictionary<string, string>> dictList)
        {
            foreach (var d in dictList)
            {
                Reviews.Add(new Review()
                {
                    PresenterEmail = d["PresenterEmail"],
                    ReviewerNeptunCode = d["ReviewerNKod"],
                    OverallScore = int.Parse(d["Score"]),
                    Text = d["Text"]
                });
            }
        }

        private void AddSupervisions(List<Dictionary<string, string>> dictList)
        {
            foreach (var d in dictList)
            {
                Supervisions.Add(new Supervision()
                {
                    AdvisorName = d["Konzulens"],
                    StudentName = d["Hallgató neve"],
                    StudentNeptunCode = d["Hallg. nept"],
                    StudentEmail = null
                });
            }
        }

        private List<Dictionary<string, string>> LoadXlsIntoDictionaryList(StorageFile file, int worksheetIndex, int headerRowIndex)
        {
            var stream = file.OpenStreamForReadAsync().Result;
            var reader = new Excel2Dict();
            return reader.Read(stream, worksheetIndex, headerRowIndex);
        }
        #endregion
    }
}
