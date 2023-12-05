using System;
using System.Threading.Tasks;

namespace Common.CommandsBase
{
    public class AsyncDelegateCommand : AsyncCommandBase
    {
        private readonly Func<Task> _execute;

        public AsyncDelegateCommand(Func<Task> execute, Func<object, bool> canExecute = null, Action<Exception> onException = null) : base(canExecute, onException)
        {
            _execute = execute;
        }

        protected override async Task ExecuteAsync(object parameter)
        {
            await _execute();
        }
    }
}
