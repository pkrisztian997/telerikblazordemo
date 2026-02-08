namespace SHD.OrganizationalFramework.BL.Workflow.Commands
{
    public interface IAsyncCommand
    {
        IEnumerable<IAsyncCommandItem> CommandItems { get; }
        CommandResult CommandResult { get; }
        ValueTask ExecuteAsync();
    }
}
