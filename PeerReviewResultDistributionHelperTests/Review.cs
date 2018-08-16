using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Email;

namespace PeerReviewResultDistributionHelperTests
{
    internal class Review
    {
        public Review()
        {
        }

        public string Text { get; internal set; }
        public int OverallScore { get; internal set; }
        public string ReviewerNeptunCode { get; internal set; }
        public string PresenterEmail { get; internal set; }

        internal EmailMessage GetPresenterEmail()
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(PresenterEmail));
            e.Body = $"Összesített pontszám: {OverallScore}\n\n{Text}";
            return e;
        }

        internal EmailMessage GetAdvisorEmail(ISupervisionLookup lookup)
        {
            var e = new EmailMessage();
            var s = lookup.GetSupervision(ReviewerNeptunCode);
            e.To.Add(new EmailRecipient(s.AdvisorEmail));
            e.Body = $"Kedves {s.AdvisorName}!\n\n"
                + $"Értékelő hallgató: {s.StudentName}({s.StudentNeptunCode}, {s.StudentEmail}) írta:\n\n"
                + $"Összesített pontszám: {OverallScore}\n\n"
                + $"{Text}\n\n";
            return e;
        }

        internal static EmailMessage GetCollectedPresenterEmail(string presenterEmail, List<Review> allReviews, SupervisionLookupBase supervisionLookup)
        {
            var reviews = allReviews.Where(r => r.PresenterEmail == presenterEmail);
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(presenterEmail));
            StringBuilder sb = new StringBuilder();
            sb.Append("Kedves Hallgatónk!\n\n" +
                "A beszámolódhoz az alábbi értékelések gyűltek össze:\n\n");
            foreach(var r in reviews)
                sb.Append($"----- Értékelés - összesített pontszám: {r.OverallScore}\n\n{r.Text}\n\n");
            e.Body = sb.ToString();
            return e;
        }
    }
}
