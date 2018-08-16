using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Email;

namespace PeerReviewCommonLib
{
    public class Review
    {
        public Review()
        {
        }

        public string Text { get; set; }
        public int OverallScore { get; set; }
        public string ReviewerNeptunCode { get; set; }
        public string PresenterEmail { get; set; }

        public override string ToString()
        {
            return $"{ReviewerNeptunCode}->{PresenterEmail} : {OverallScore}, {Text}";
        }

        public EmailMessage GetPresenterEmail()
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(PresenterEmail));
            e.Body = $"Összesített pontszám: {OverallScore}\n\n{Text}";
            return e;
        }

        public EmailMessage GetAdvisorEmail(ISupervisionLookup lookup)
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

        public static EmailMessage GetCollectedPresenterEmail(string presenterEmail, List<Review> allReviews, SupervisionLookupBase supervisionLookup)
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

        public static EmailMessage GetCollectedAdvisorEmail(string advisorEmail, List<Review> reviews, SupervisionLookupBase s)
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(advisorEmail));
            var advisorName = s.GetAdvisorName(advisorEmail);
            StringBuilder sb = new StringBuilder();
            sb.Append($"Kedves {advisorName}!\n\n" +
                "Hallgatóid az alábbi értékeléseket adták le a beszámolókra:\n\n");
            foreach (var r in reviews)
            {
                var supervision = s.GetSupervision(r.ReviewerNeptunCode);
                if (supervision.AdvisorEmail == advisorEmail)
                {
                    sb.Append($"--- Értékelő hallgató: {supervision.StudentName}({supervision.StudentNeptunCode}, {supervision.StudentEmail}) írta:\n\n");
                    sb.Append($"- összesített pontszám: {r.OverallScore}\n\n{r.Text}\n\n");
                }
            }
            e.Body = sb.ToString();
            return e;
        }
    }
}
