// Models/WishlistItem.cs
using System.Text.Json.Serialization;

namespace SteamDeal.Models
{
    public class WishlistItem
    {
        [JsonPropertyName("dealID")]
        public string DealID { get; set; }

        [JsonPropertyName("steamAppID")]
        public string SteamAppID { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("thumb")]
        public string Thumb { get; set; }

        [JsonPropertyName("salePrice")]
        public string SalePrice { get; set; }

        [JsonPropertyName("normalPrice")]
        public string NormalPrice { get; set; }

        [JsonPropertyName("savings")]
        public string Savings { get; set; }

        [JsonPropertyName("dateAdded")]
        public DateTime DateAdded { get; set; }

        // For display purposes
        public string FormattedSalePrice => decimal.TryParse(SalePrice, out var price) ? $"${price:F2}" : "N/A";
        public string FormattedNormalPrice => decimal.TryParse(NormalPrice, out var price) ? $"${price:F2}" : "N/A";
        public string FormattedSavings => $"-{Savings}%";
        public string DateAddedText => DateAdded.ToString("MMM dd, yyyy");
    }
}