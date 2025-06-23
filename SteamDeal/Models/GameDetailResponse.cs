using System.Text.Json.Serialization;

namespace SteamDeal.Models;

public class GameDetailResponse
{
    [JsonPropertyName("gameInfo")]
    public GameInfo GameInfo { get; set; }

    [JsonPropertyName("cheaperStores")]
    public object[] CheaperStores { get; set; }

    [JsonPropertyName("cheapestPrice")]
    public CheapestPrice CheapestPrice { get; set; }
}

public class GameInfo
{
    [JsonPropertyName("storeID")]
    public string StoreID { get; set; }

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

    [JsonPropertyName("steamRatingText")]
    public string SteamRatingText { get; set; }

    [JsonPropertyName("steamRatingPercent")]
    public string SteamRatingPercent { get; set; }

    [JsonPropertyName("steamRatingCount")]
    public string SteamRatingCount { get; set; }

    [JsonPropertyName("metacriticScore")]
    public string MetacriticScore { get; set; }

    [JsonPropertyName("metacriticLink")]
    public string MetacriticLink { get; set; }

    [JsonPropertyName("releaseDate")]
    public long ReleaseDate { get; set; }

    [JsonPropertyName("publisher")]
    public string Publisher { get; set; }

    [JsonPropertyName("steamworks")]
    public object Steamworks { get; set; }

    // Legacy properties for backward compatibility
    [JsonIgnore]
    public string Title => Name;

    [JsonIgnore]
    public string DealRating => "0"; // Not provided in detail API

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

public class CheapestPrice
{
    [JsonPropertyName("date")]
    public long Date { get; set; }
}