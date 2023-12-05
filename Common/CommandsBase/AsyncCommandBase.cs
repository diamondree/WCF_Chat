using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.CommandsBase
{
    public abstract class AsyncCommandBase : ICommand
    {
        private readonly Action<Exception> _onException;

        private bool _isExecuting = false;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public AsyncCommandBase(Func<object, bool> canExecute, Action<Exception> onException)
        {
            _canExecute = canExecute;
            _onException = onException;
        }

        public bool CanExecute(object parameter)
        {
            if (_isExecuting)
                return false;
            if (_canExecute == null || _canExecute(parameter))
                return true;
            return false;
        }

        public async void Execute(object parameter)
        {
            _isExecuting = true;

            try
            {
                await ExecuteAsync(parameter);
            }
            catch (Exception ex)
            {
                _onException?.Invoke(ex);
            }

            _isExecuting = false;
        }

        protected abstract Task ExecuteAsync(object parameter);
    }
}
