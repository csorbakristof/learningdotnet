namespace PeerReviewCommonLib
{
    internal class Supervision
    {
        public string StudentName { get; set; }
        public string StudentNeptunCode { get; set; }
        public string StudentEmail { get; set; }
        public string AdvisorName { get; set; }
        public string AdvisorEmail { get; set; }

        public override string ToString()
        {
            return $"{StudentName}({StudentNeptunCode}, {StudentEmail})-{AdvisorName}({AdvisorEmail})";
        }
    }
}
