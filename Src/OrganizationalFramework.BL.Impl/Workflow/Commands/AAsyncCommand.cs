namespace SHD.OrganizationalFramework.BL.Workflow.Commands
{
    public abstract class AAsyncCommand : IAsyncCommand
    {
        public IEnumerable<IAsyncCommandItem> CommandItems => _commandItems;

        public CommandResult CommandResult => CommandItems
            .Any(ci => ci.CommandResult < CommandResult.Succeeded)
            ? CommandResult.Failed
            : CommandResult.Succeeded;

        private readonly ICollection<IAsyncCommandItem> _commandItems;

        protected AAsyncCommand()
        {
            _commandItems = new List<IAsyncCommandItem>();
        }

        public async ValueTask ExecuteAsync()
        {
            foreach (var command in CommandItems)
            {
                await command.ExecuteAsync();
            }
        }

        protected void AddCommandItem(IAsyncCommandItem item)
        {
            ArgumentNullException.ThrowIfNull(item);

            _commandItems.Add(item);
        }
    }
}
