using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Classes.FinancialInstruments.CoinCapJsonModels;
using WpfApp1.Models;

namespace WpfApp1.Classes.FinancialInstruments;

internal class CoinCapRatesProvider : CryptoProvider {
    
    private readonly JsonSerializerOptions _jsonOptions = new() {
        NumberHandling = JsonNumberHandling.AllowReadingFromString
    }; 
    private readonly string _url;
    private readonly int _tickerCount;
    private HttpClient? _client;

    public CoinCapRatesProvider(TimeSpan timeout, ILogger? logger, string url, string[] tickers, CancellationToken cts) : base(timeout, logger, cts) {
        _url = $"{url}?ids={string.Join(',', tickers)}";
        _tickerCount = tickers.Length;
    }

    internal override async Task<List<RateModel>> GetRates() {
        var returnRates = new List<RateModel>(_tickerCount);
        _client ??= new() {
            Timeout = _timeout
        };

        try {
            using var response = await _client.GetAsync(_url, _cts);

            if (response is { StatusCode: HttpStatusCode.OK }) {
                var deserializedResults = await response.Content
                    .ReadFromJsonAsync<RootModel>(options: _jsonOptions, cancellationToken: _cts);

                var convertedModel = deserializedResults?.Series?
                    .Select(m => new RateModel() {
                        Ticker = m.Ticker,
                        Rate = m.USDRatio
                    })
                    .Where(m => m is not null)
                    .ToList();

                if (convertedModel != null)
                    returnRates = convertedModel;
            }
        } catch (JsonException exJson) {
            // ToDo: Logger
        } catch (HttpRequestException exHttp) {
            _client.Dispose();
            // ToDo: Logger
        } catch(OperationCanceledException) {
            _client.Dispose();
            // ToDo: Cancellation exception
        } catch(Exception ex) {
            _client.Dispose();
            // ToDo: Handle unknown exceptions
        }

        return returnRates;
    }
}
