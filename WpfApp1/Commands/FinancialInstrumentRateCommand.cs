using System;
using System.Windows.Input;

namespace WpfApp1.Commands;

public sealed class FinancialInstrumentRateCommand : ICommand {

    private readonly Action<object?> _action;

    public FinancialInstrumentRateCommand(Action<object?> action) => _action = action;
    
    public void Execute(object? parameter) => _action(parameter);
    
    public bool CanExecute(object? parameter) => true;

    public event EventHandler? CanExecuteChanged {
        add { }
        remove { }
    }
}
