using System.Windows;
using System.Windows.Input;

namespace SmsCenter.UI.Shared.Commands;

public abstract class CommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);
}

public abstract class CommandBaseAsync : ICommand
{
    private bool _isExecuting;

    // ReSharper disable once MemberCanBePrivate.Global
    internal bool IsExecuting
    {
        get => _isExecuting;
        set
        {
            _isExecuting = value;
            OnCanExecuteChanged();
        }
    }

    public event EventHandler? CanExecuteChanged;

    public virtual bool CanExecute(object? parameter) => !IsExecuting;

    public async void Execute(object? parameter)
    {
        try
        {
            IsExecuting = true;
            await ExecuteAsync(parameter!);
            IsExecuting = false;
        }
        catch (Exception e)
        {
            MessageBox.Show("Command base async error: " + e.Message);
        }
    }

    protected abstract Task ExecuteAsync(object parameter);

    protected void OnCanExecuteChanged() =>
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}