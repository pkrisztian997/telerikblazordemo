namespace SHD.OrganizationalFramework.BL.Feedback
{
    public interface IFeedbackCollector
    {
        bool IsOkay { get; }
        IEnumerable<IFeedbackMessage> FeedbackMessages { get; }

        void AddFeedback(IFeedbackResource feedbackMessage, params string[] feedbackArguments);
    }
}
