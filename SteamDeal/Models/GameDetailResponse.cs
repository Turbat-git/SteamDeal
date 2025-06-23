using System.Text.Json.Serialization;

namespace SteamDeal.Models;

public class GameDetailResponse
{
    [JsonPropertyName("gameInfo")]
    public GameInfo GameInfo { get; set; }
}

public class GameInfo
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("thumb")]
    public string Thumb { get; set; }

    [JsonPropertyName("salePrice")]
    public string SalePrice { get; set; }

    [JsonPropertyName("dealRating")]
    public string DealRating { get; set; }

    [JsonPropertyName("retailPrice")]
    public string RetailPrice { get; set; }

    [JsonPropertyName("savings")]
    public string Savings { get; set; }

    [JsonPropertyName("steamAppID")]
    public string SteamAppID { get; set; }

    [JsonPropertyName("dealID")]
    public string DealID { get; set; }
}