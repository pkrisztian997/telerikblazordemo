namespace SHD.OrganizationalFramework.BL.Feedback
{
    public sealed class FeedbackResource : IFeedbackResource
    {
        public Severity Severity { get; }
        public string MessageKey { get; }

        public FeedbackResource(Severity severity, string messageKey)
        {
            Severity = severity;
            MessageKey = messageKey ?? throw new ArgumentNullException(nameof(messageKey));
        }
    }
}
