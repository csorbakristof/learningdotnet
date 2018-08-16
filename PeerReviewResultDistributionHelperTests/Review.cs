using System;
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
    }
}
