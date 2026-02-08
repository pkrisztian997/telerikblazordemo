namespace SHD.OrganizationalFramework.BL.Feedback
{
    public interface IFeedbackResource
    {
        Severity Severity { get; }
        string MessageKey { get; }
    }
}
