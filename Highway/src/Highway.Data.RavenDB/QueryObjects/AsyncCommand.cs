using System.Threading.Tasks;
using Highway.Data.Interfaces;

namespace Highway.Data.QueryObjects
{
    public class AsyncCommand : IAsyncCommand
    {
        private readonly ICommand _command;

        public AsyncCommand(ICommand command)
        {
            _command = command;
        }

        #region IAsyncCommand Members

        public Task Execute(IDataContext context)
        {
            return new Task(() =>
                {
                    lock (context)
                    {
                        _command.Execute(context);
                    }
                });
        }

        #endregion
    }
}