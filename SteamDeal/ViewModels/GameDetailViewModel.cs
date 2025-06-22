using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamDeal.Models;

namespace SteamDeal.ViewModels;

public class GameDetailViewModel
{
    public string Title { get; set; }
    public string Image { get; set; }
    public string Price { get; set; }
    public string NormalPrice { get; set; }
    public string Savings { get; set; }

    public GameDetailViewModel(GameDetailResponse deal)
    {
        var gameInfo = deal?.GameInfo;

        Title = gameInfo?.Title ?? "Unknown Game";
        Image = gameInfo?.Thumb ?? "";

        if (decimal.TryParse(gameInfo?.SalePrice, out var salePrice))
        {
            Price = $"${salePrice:F2}";
        }
        else
        {
            Price = "Price unavailable";
        }

        if (decimal.TryParse(gameInfo?.RetailPrice, out var retailPrice))
        {
            NormalPrice = $"Original: ${retailPrice:F2}";
        }
        else
        {
            NormalPrice = "Original price unavailable";
        }

        if (decimal.TryParse(gameInfo?.Savings, out var savingsPercent))
        {
            Savings = $"You save {savingsPercent:F0}%!";
        }
        else
        {
            Savings = "Savings unavailable";
        }        
    }
}
