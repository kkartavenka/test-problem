using System.Text.Json.Serialization;

namespace WpfApp1.Classes.FinancialInstruments.CoinCapJsonModels; 
public record class SeriesModel {

    [JsonPropertyName("name")]
    public string Ticker { get; init; } = string.Empty;


    [JsonPropertyName("priceUsd")]
    public double USDRatio { get; init; }
}
