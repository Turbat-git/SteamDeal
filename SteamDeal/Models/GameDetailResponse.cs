namespace SteamDeal.Models;

public class GameDetailResponse
{
    public GameInfo GameInfo { get; set; }
}

public class GameInfo
{
    public string Title { get; set; }
    public string SalePrice { get; set; }
    public string RetailPrice { get; set; }
    public string Savings { get; set; }
    public string Thumb { get; set; }
}