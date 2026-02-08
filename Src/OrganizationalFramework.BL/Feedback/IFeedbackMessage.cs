namespace SHD.OrganizationalFramework.BL.Feedback
{
    public interface IFeedbackMessage
    {
        IFeedbackResource FeedbackResource { get; }
        IEnumerable<string> Arguments { get; }
    }
}
