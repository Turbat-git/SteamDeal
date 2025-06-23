using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamDeal.Models;

namespace SteamDeal.ViewModels;

public class GameDetailViewModel
{
    public string GameTitle { get; set; }  
    public string ImageUrl { get; set; }
    public string Price { get; set; }
    public string NormalPrice { get; set; }
    public string Savings { get; set; }

    public GameDetailViewModel(GameDetailResponse deal)
    {
        var gameInfo = deal?.GameInfo;

        GameTitle = gameInfo?.Title ?? "Unknown Game";
        ImageUrl = gameInfo?.Thumb ?? "";

        System.Diagnostics.Debug.WriteLine($"🎮 GameDetailViewModel - Processing:");
        System.Diagnostics.Debug.WriteLine($"   Raw SalePrice: '{gameInfo?.SalePrice}'");
        System.Diagnostics.Debug.WriteLine($"   Raw RetailPrice: '{gameInfo?.RetailPrice}'");
        System.Diagnostics.Debug.WriteLine($"   Raw Savings: '{gameInfo?.Savings}'");


        //For sale price parsing
        if (decimal.TryParse(gameInfo?.SalePrice, out var salePrice))
        {
            Price = $"${salePrice:F2}";
            System.Diagnostics.Debug.WriteLine($"   ✅ Parsed SalePrice: {Price}");
        }
        else
        {
            Price = "Price unavailable";
            System.Diagnostics.Debug.WriteLine($"   ❌ Failed to parse SalePrice: '{gameInfo?.SalePrice}'");
        }

        //For normal pricing parcing
        if (decimal.TryParse(gameInfo?.RetailPrice, out var retailPrice))
        {
            NormalPrice = $"Original: ${retailPrice:F2}";
            System.Diagnostics.Debug.WriteLine($"   ✅ Parsed RetailPrice: {NormalPrice}");
        }
        else
        {
            NormalPrice = "Original price unavailable";
            System.Diagnostics.Debug.WriteLine($"   ❌ Failed to parse RetailPrice: '{gameInfo?.RetailPrice}'");
        }

        //For saving calculation
        if (!string.IsNullOrWhiteSpace(gameInfo?.Savings) && decimal.TryParse(gameInfo.Savings, out var savingsPercent))
        {
            Savings = $"You save {savingsPercent:F0}%!";
            System.Diagnostics.Debug.WriteLine($"   ✅ Parsed Savings: {Savings}");
        }
        else if (
            decimal.TryParse(gameInfo?.RetailPrice, out var retail) &&
            decimal.TryParse(gameInfo?.SalePrice, out var sale) &&
            retail > 0
        )
        {
            var calculatedSavings = ((retail - sale) / retail) * 100;
            Savings = $"You save {calculatedSavings:F0}%!";
            System.Diagnostics.Debug.WriteLine($"   🔄 Fallback Savings: {Savings}");
        }
        else
        {
            Savings = "Savings unavailable";
            System.Diagnostics.Debug.WriteLine($"   ❌ Failed to parse Savings: '{gameInfo?.Savings}'");
        }
    }
}