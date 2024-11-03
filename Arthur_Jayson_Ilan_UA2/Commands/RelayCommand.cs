using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Arthur_Jayson_Ilan_UA2.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute) : this(execute, null) { }
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;

        public RelayCommand(Action<T?> execute) : this(execute, null) { }
        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;

            if (parameter == null && typeof(T).IsValueType)
                return _canExecute(default!);

            return parameter is T t && _canExecute(t);
        }

        public void Execute(object? parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
                _execute(default!);
            else if (parameter is T t)
                _execute(t);
        }

        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
