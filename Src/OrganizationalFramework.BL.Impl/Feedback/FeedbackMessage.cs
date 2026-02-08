namespace SHD.OrganizationalFramework.BL.Feedback
{
    internal class FeedbackMessage : IFeedbackMessage
    {
        public IFeedbackResource FeedbackResource { get; }
        public IEnumerable<string> Arguments { get; }

        public FeedbackMessage(IFeedbackResource feedbackResource, params string[] arguments)
        {
            FeedbackResource = feedbackResource ?? throw new ArgumentNullException(nameof(feedbackResource));
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }
    }
}
