using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Models;

namespace WpfApp1.Classes.FinancialInstruments;
internal abstract class CryptoProvider {
    internal CancellationToken _cts;
    internal TimeSpan _timeout;
    private ILogger? _logger;

    internal CryptoProvider(TimeSpan timeout, ILogger? logger, CancellationToken cts) => (_cts, _timeout, _logger) = (cts, timeout, logger);
    internal abstract Task<List<RateModel>> GetRates();
}
