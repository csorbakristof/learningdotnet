namespace PeerReviewResultDistributionHelperTests
{
    interface ISupervisionLookup
    {
        Supervision GetSupervision(string StudentNeptunCode);
    }
}
