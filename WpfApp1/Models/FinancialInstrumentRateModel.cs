using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Classes.FinancialInstruments;

namespace WpfApp1.Models;

public class FinancialInstrumentRateModel
{
    private const string _stateRunning = "Stop";
    private const string _stateStopped = "Start";

    private const string _apiEndPoint = @"http://api.coincap.io/v2/assets";
    private readonly string[] _tickersOfInterst = new[] { "bitcoin", "ethereum", "dogecoin" };

    private readonly TimeSpan _updateFrequency = TimeSpan.FromSeconds(10);
    private readonly TimeSpan _httpRequestTimeout = TimeSpan.FromSeconds(100);

    private Task? _backgroundRunner;
    private bool _isRunning;

    private CancellationTokenSource _cts = new();

    public string StateCaption { get; private set; } = _stateStopped;

    public FinancialInstrumentRateModel(int updateFrequencyS) {
        _updateFrequency = TimeSpan.FromSeconds(updateFrequencyS);
    }

    private async Task Runner(Action onUpdateRates) {
        var timer = new PeriodicTimer(_updateFrequency);

        try {
            do {
                // ToDo: Should be iterator here, which accepts multipler crypto rate providers
                CryptoProvider provider = new CoinCapRatesProvider(
                    cts: _cts.Token,
                    timeout: _httpRequestTimeout,
                    logger: null,
                    url: _apiEndPoint,
                    tickers: _tickersOfInterst);

                MonitoredRates = new ReadOnlyCollection<RateModel>(await provider.GetRates());

                onUpdateRates();

            } while (await timer.WaitForNextTickAsync(_cts.Token));
        } catch (OperationCanceledException) {
            // ToDo: Cancellation
        }
    }

    public void ChangeUpdaterState(Action onUpdaterState, Action onUpdateRates) {
        if (_isRunning)
            _cts.Cancel();
        else {
            _backgroundRunner = new Task(async () => {
                _cts.Dispose();
                _cts = new();
                await Runner(onUpdateRates);
            });
            _backgroundRunner.Start();
        }

        _isRunning = !_isRunning;
        StateCaption = _isRunning ? _stateRunning : _stateStopped;

        onUpdaterState();
    }

    public ReadOnlyCollection<RateModel>? MonitoredRates { get; private set; }
}
