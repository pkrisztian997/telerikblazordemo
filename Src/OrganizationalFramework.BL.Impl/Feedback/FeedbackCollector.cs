namespace SHD.OrganizationalFramework.BL.Feedback
{
    internal class FeedbackCollector : IFeedbackCollector
    {
        public bool IsOkay => FeedbackMessages.Any(fm => fm.FeedbackResource.Severity < Severity.Warning) ? false : true;
        public IEnumerable<IFeedbackMessage> FeedbackMessages => _feedbackMessages;
        private readonly ICollection<IFeedbackMessage> _feedbackMessages;

        public FeedbackCollector()
        {
            _feedbackMessages = new List<IFeedbackMessage>();
        }

        public void AddFeedback(IFeedbackMessage feedbackMessage)
        {
            _feedbackMessages.Add(feedbackMessage);
        }

        public void AddFeedback(IFeedbackResource feedbackResource, params string[] feedbackArguments)
        {
            _feedbackMessages.Add(new FeedbackMessage(feedbackResource, feedbackArguments));
        }
    }
}
