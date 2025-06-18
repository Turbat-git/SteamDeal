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
        Title = deal.GameInfo?.Title ?? "Unknown";
        Image = deal.GameInfo?.Thumb;
        Price = $"${deal.GameInfo?.SalePrice}";
        NormalPrice = $"Original: ${deal.GameInfo?.RetailPrice}";
        Savings = $"You save {deal.GameInfo?.Savings}%!";
    }
}
