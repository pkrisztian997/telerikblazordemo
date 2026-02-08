namespace SHD.OrganizationalFramework.BL.Workflow.Commands
{
    public abstract class AAsyncCommandItem : IAsyncCommandItem
    {
        public CommandResult CommandResult { get; private set; }

        public abstract ValueTask ExecuteAsync();

        protected void SetCommandResult(CommandResult commandResult)
        {
            CommandResult = commandResult;
        }
    }
}
