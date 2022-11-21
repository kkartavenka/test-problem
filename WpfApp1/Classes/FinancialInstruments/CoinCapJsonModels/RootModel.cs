using System.Text.Json.Serialization;

namespace WpfApp1.Classes.FinancialInstruments.CoinCapJsonModels; 
public record class RootModel {
    [JsonPropertyName("data")]
    public SeriesModel[]? Series { get; init; }
}
