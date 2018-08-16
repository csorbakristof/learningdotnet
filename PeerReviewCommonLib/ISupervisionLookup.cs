namespace PeerReviewCommonLib
{
    internal interface ISupervisionLookup
    {
        Supervision GetSupervision(string StudentNeptunCode);
    }
}
