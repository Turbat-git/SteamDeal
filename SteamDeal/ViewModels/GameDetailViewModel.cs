using System;using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamDeal.Models;

namespace SteamDeal.ViewModels;

public class GameDetailViewModel
{
    public string GameTitle { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public string PriceText { get; set; }
    public string OriginalPrice { get; set; }
    public string DealRating { get; set; }

    public GameDetailViewModel(GameDeal game)
    {
        GameTitle = game.Title;
        ImageUrl = game.Thumb;
        Description = game.SteamAppID != null ? $"Steam App ID: {game.SteamAppID}" : "No description available";
        PriceText = $"${game.SalePrice}";
        OriginalPrice = $"${game.NormalPrice}";
        DealRating = $"{game.DealRating}/10";
    }
}
