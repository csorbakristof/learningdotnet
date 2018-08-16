using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Email;

namespace PeerReviewCommonLib
{
    internal class Review
    {
        public Review()
        {
        }

        public string Text { get; set; }
        public int OverallScore { get; set; }
        public string ReviewerNeptunCode { get; set; }
        public string PresenterEmail { get; set; }

        public Supervision ReviewersSupervision { get; set; }

        public override string ToString()
        {
            return $"{ReviewerNeptunCode}->{PresenterEmail} : {OverallScore}, {Text}";
        }

        public void ConnectReviewersSupervision(ISupervisionLookup lookup)
        {
            ReviewersSupervision = lookup.GetSupervision(ReviewerNeptunCode);
        }

        public EmailMessage GetPresenterEmail()
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(PresenterEmail));
            e.Body = $"Összesített pontszám: {OverallScore}\n\n{Text}";
            return e;
        }

        public EmailMessage GetAdvisorEmail()
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(ReviewersSupervision.AdvisorEmail));
            e.Subject = "Saját hallgatók által leadott beszámoló értékelések";
            e.Body = $"Kedves {ReviewersSupervision.AdvisorName}!\n\n"
                + $"Értékelő hallgató: {ReviewersSupervision.StudentName}({ReviewersSupervision.StudentNeptunCode}, {ReviewersSupervision.StudentEmail}) írta:\n\n"
                + $"Összesített pontszám: {OverallScore}\n\n"
                + $"{Text}\n\n";
            return e;
        }

        public static EmailMessage GetCollectedPresenterEmail(string presenterEmail, List<Review> allReviews)
        {
            var reviews = allReviews.Where(r => r.PresenterEmail == presenterEmail);
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(presenterEmail));
            e.Subject = "Beszámolóra kapott értékelések";
            StringBuilder sb = new StringBuilder();
            sb.Append("Kedves Hallgatónk!\n\n" +
                "A beszámolódhoz az alábbi értékelések gyűltek össze:\n\n");
            foreach(var r in reviews)
                sb.Append($"----- Értékelés - összesített pontszám: {r.OverallScore}\n\n{r.Text}\n\n");
            e.Body = sb.ToString();
            return e;
        }

        public static EmailMessage GetCollectedAdvisorEmail(string advisorEmail, string advisorName, List<Review> reviews)
        {
            var e = new EmailMessage();
            e.To.Add(new EmailRecipient(advisorEmail));
            StringBuilder sb = new StringBuilder();
            sb.Append($"Kedves {advisorName}!\n\n" +
                "Hallgatóid az alábbi értékeléseket adták le a beszámolókra:\n\n");
            foreach (var r in reviews)
            {
                var supervision = r.ReviewersSupervision;
                if (r.ReviewersSupervision.AdvisorEmail == advisorEmail)
                {
                    sb.Append($"--- Értékelő hallgató: {r.ReviewersSupervision.StudentName}({r.ReviewersSupervision.StudentNeptunCode}, {r.ReviewersSupervision.StudentEmail}) írta:\n\n");
                    sb.Append($"- összesített pontszám: {r.OverallScore}\n\n{r.Text}\n\n");
                }
            }
            e.Body = sb.ToString();
            return e;
        }
    }
}
