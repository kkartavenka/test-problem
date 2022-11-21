using Service;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Windows.Input;
using WpfApp1.Classes.WindowsServices;
using WpfApp1.Commands;
using WpfApp1.Models;

namespace WpfApp1.ViewModels; 

public class CryptoAppViewModel : INotifyPropertyChanged {

    private const int _updateFrequencyS = 10;
    private readonly FinancialInstrumentRateModel _financialRateModel = new(_updateFrequencyS);

    public ObservableCollection<WindowsServiceModel> WindowsServices { get; private set; } = new();

    public ObservableCollection<RateModel> FinancialInstrumentRates {
        get => _financialRateModel.MonitoredRates != null ? new(_financialRateModel.MonitoredRates) : new();
    }
    
    public string CryptoUpdaterState { 
        get => _financialRateModel.StateCaption; 
    }

    public CryptoAppViewModel() => GetWindowsServices();

    private void GetWindowsServices() {
        var statusesOfInterest = new ServiceControllerStatus[] { ServiceControllerStatus.Running };
        WindowsServiceProvider serviceProvider = new DefaultWindowsServiceProvider(statusesOfInterest, null);
        var servicesOfInterest = serviceProvider.GetServices();
        if (servicesOfInterest != null)
            WindowsServices = new ObservableCollection<WindowsServiceModel>(servicesOfInterest);
    }

    public ICommand ChangeUpdaterState => new FinancialInstrumentRateCommand(_ => _financialRateModel.ChangeUpdaterState(OnUpdaterState, OnUpdateRates));

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

    private void OnUpdaterState() => OnPropertyChanged(nameof(CryptoUpdaterState));
    private void OnUpdateRates() => OnPropertyChanged(nameof(FinancialInstrumentRates));
}
