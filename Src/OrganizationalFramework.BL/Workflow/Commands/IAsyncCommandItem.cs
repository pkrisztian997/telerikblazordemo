namespace SHD.OrganizationalFramework.BL.Workflow.Commands
{
    public interface IAsyncCommandItem
    {
        CommandResult CommandResult { get; }
        ValueTask ExecuteAsync();
    }
}
