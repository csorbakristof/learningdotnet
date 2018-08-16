using System.Collections.Generic;

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
    }
}
