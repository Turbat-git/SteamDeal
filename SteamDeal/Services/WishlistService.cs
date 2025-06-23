// Services/WishlistService.cs
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using SteamDeal.Models;

namespace SteamDeal.Services
{
    public class WishlistService
    {
        private const string WishlistKey = "GameWishlist";
        private static WishlistService _instance;
        public static WishlistService Instance => _instance ??= new WishlistService();

        public ObservableCollection<WishlistItem> WishlistItems { get; private set; }

        private WishlistService()
        {
            WishlistItems = new ObservableCollection<WishlistItem>();
            LoadWishlist();
        }

        public async Task<bool> AddToWishlistAsync(GameDeal game)
        {
            if (game == null || IsInWishlist(game.DealID))
                return false;

            var wishlistItem = new WishlistItem
            {
                DealID = game.DealID,
                SteamAppID = game.SteamAppID,
                Title = game.Title,
                Thumb = game.Thumb,
                SalePrice = game.SalePrice,
                NormalPrice = game.NormalPrice,
                Savings = game.Savings,
                DateAdded = DateTime.Now
            };

            WishlistItems.Add(wishlistItem);
            await SaveWishlistAsync();
            return true;
        }

        public async Task<bool> AddToWishlistAsync(GameInfo gameInfo, string dealId = null)
        {
            if (gameInfo == null || IsInWishlist(dealId ?? gameInfo.SteamAppID))
                return false;

            var wishlistItem = new WishlistItem
            {
                DealID = dealId ?? gameInfo.SteamAppID,
                SteamAppID = gameInfo.SteamAppID,
                Title = gameInfo.Name,
                Thumb = gameInfo.Thumb,
                SalePrice = gameInfo.SalePrice,
                NormalPrice = gameInfo.RetailPrice,
                Savings = gameInfo.Savings,
                DateAdded = DateTime.Now
            };

            WishlistItems.Add(wishlistItem);
            await SaveWishlistAsync();
            return true;
        }

        public async Task<bool> RemoveFromWishlistAsync(string identifier)
        {
            var item = WishlistItems.FirstOrDefault(x =>
                x.DealID == identifier ||
                x.SteamAppID == identifier);

            if (item == null)
                return false;

            WishlistItems.Remove(item);
            await SaveWishlistAsync();
            return true;
        }

        public bool IsInWishlist(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
                return false;

            return WishlistItems.Any(x =>
                x.DealID == identifier ||
                x.SteamAppID == identifier);
        }

        public async Task ClearWishlistAsync()
        {
            WishlistItems.Clear();
            await SaveWishlistAsync();
        }

        private async Task SaveWishlistAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(WishlistItems.ToList());
                Preferences.Set(WishlistKey, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving wishlist: {ex.Message}");
            }
        }

        private void LoadWishlist()
        {
            try
            {
                var json = Preferences.Get(WishlistKey, string.Empty);
                if (!string.IsNullOrEmpty(json))
                {
                    var items = JsonSerializer.Deserialize<List<WishlistItem>>(json);
                    if (items != null)
                    {
                        WishlistItems.Clear();
                        foreach (var item in items)
                        {
                            WishlistItems.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading wishlist: {ex.Message}");
            }
        }
    }
}