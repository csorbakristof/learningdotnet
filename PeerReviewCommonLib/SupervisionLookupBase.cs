using System.Collections.Generic;
using System.Linq;

namespace PeerReviewCommonLib
{
    public class SupervisionLookupBase : ISupervisionLookup
    {

        private Dictionary<string, Supervision> supervisions = new Dictionary<string, Supervision>();

        public Supervision GetSupervision(string studentNeptunCode)
        {
            return supervisions[studentNeptunCode];
        }

        public void AddIfNew(string reviewerNeptunCode, Supervision s)
        {
            if (!supervisions.ContainsKey(reviewerNeptunCode))
                supervisions.Add(reviewerNeptunCode, s);
        }

        public string GetAdvisorName(string advisorEmail)
        {
            return supervisions.Values.
                First(s => s.AdvisorEmail == advisorEmail).AdvisorName;
        }
    }
}
