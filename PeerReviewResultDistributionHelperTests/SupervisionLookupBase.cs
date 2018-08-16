using System.Collections.Generic;
using System.Linq;

namespace PeerReviewResultDistributionHelperTests
{
    internal class SupervisionLookupBase : ISupervisionLookup
    {

        private Dictionary<string, Supervision> supervisions = new Dictionary<string, Supervision>();

        public Supervision GetSupervision(string studentNeptunCode)
        {
            return supervisions[studentNeptunCode];
        }

        internal void Add(string reviewerNeptunCode, Supervision s)
        {
            supervisions.Add(reviewerNeptunCode, s);
        }

        internal string GetAdvisorName(string advisorEmail)
        {
            return supervisions.Values.
                First(s => s.AdvisorEmail == advisorEmail).AdvisorName;
        }
    }
}
