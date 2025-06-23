using System.Text.Json.Serialization;

namespace SteamDeal.Models;

public class GameDetailResponse
{
    [JsonPropertyName("gameInfo")]
    public GameInfo GameInfo { get; set; }
}

public class GameInfo
{
    [JsonPropertyName("gameID")]
    public string GameID { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("steamAppID")]
    public string SteamAppID { get; set; }

    [JsonPropertyName("thumb")]
    public string Thumb { get; set; }

    [JsonPropertyName("salePrice")]
    public string SalePrice { get; set; }

    [JsonPropertyName("retailPrice")]
    public string RetailPrice { get; set; }

    // Legacy properties for backward compatibility
    [JsonIgnore]
    public string Title => Name;

    [JsonIgnore]
    public string NormalPrice => RetailPrice;

    [JsonIgnore]
    public string Savings
    {
        get
        {
            if (decimal.TryParse(RetailPrice, out var retail) &&
                decimal.TryParse(SalePrice, out var sale) &&
                retail > 0)
            {
                var savingsPercent = ((retail - sale) / retail) * 100;
                return savingsPercent.ToString("F0");
            }
            return "0";
        }
    }

    [JsonIgnore]
    public string DealID { get; set; } // This will be set separately when needed
}