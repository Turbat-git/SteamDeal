// ViewModels/GameDetailViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SteamDeal.Models;
using SteamDeal.Services;

namespace SteamDeal.ViewModels
{
    public class GameDetailViewModel : INotifyPropertyChanged
    {
        private GameInfo _gameInfo;
        private string _dealId;
        private bool _isInWishlist;

        public string Title { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
        public string NormalPrice { get; set; }
        public string Savings { get; set; }

        public bool IsInWishlist
        {
            get => _isInWishlist;
            set
            {
                _isInWishlist = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WishlistButtonText));
                OnPropertyChanged(nameof(WishlistButtonColor));
            }
        }

        public string WishlistButtonText => IsInWishlist ? "Remove from Wishlist" : "Add to Wishlist";
        public string WishlistButtonColor => IsInWishlist ? "#FF6B6B" : "#4CAF50";

        public ICommand ToggleWishlistCommand { get; }

        public GameDetailViewModel(GameDetailResponse deal, string dealId = null)
        {
            _gameInfo = deal?.GameInfo;
            _dealId = dealId;

            ToggleWishlistCommand = new Command(async () => await ToggleWishlistAsync());

            InitializeViewModel();
            CheckWishlistStatus();
        }

        private void InitializeViewModel()
        {
            // Use 'Name' property instead of 'Title' since that's what the API returns
            Title = _gameInfo?.Name ?? "Unknown Game";
            Image = _gameInfo?.Thumb ?? "";

            System.Diagnostics.Debug.WriteLine($"🎮 GameDetailViewModel - Processing:");
            System.Diagnostics.Debug.WriteLine($"   Game Name: '{_gameInfo?.Name}'");
            System.Diagnostics.Debug.WriteLine($"   Raw SalePrice: '{_gameInfo?.SalePrice}'");
            System.Diagnostics.Debug.WriteLine($"   Raw RetailPrice: '{_gameInfo?.RetailPrice}'");

            //For sale price parsing
            if (decimal.TryParse(_gameInfo?.SalePrice, out var salePrice))
            {
                Price = $"${salePrice:F2}";
                System.Diagnostics.Debug.WriteLine($"   ✅ Parsed SalePrice: {Price}");
            }
            else
            {
                Price = "Price unavailable";
                System.Diagnostics.Debug.WriteLine($"   ❌ Failed to parse SalePrice: '{_gameInfo?.SalePrice}'");
            }

            //For normal pricing parsing
            if (decimal.TryParse(_gameInfo?.RetailPrice, out var retailPrice))
            {
                NormalPrice = $"Original: ${retailPrice:F2}";
                System.Diagnostics.Debug.WriteLine($"   ✅ Parsed RetailPrice: {NormalPrice}");
            }
            else
            {
                NormalPrice = "Original price unavailable";
                System.Diagnostics.Debug.WriteLine($"   ❌ Failed to parse RetailPrice: '{_gameInfo?.RetailPrice}'");
            }

            //For saving calculation
            if (decimal.TryParse(_gameInfo?.RetailPrice, out var retail) &&
                decimal.TryParse(_gameInfo?.SalePrice, out var sale) &&
                retail > 0)
            {
                var calculatedSavings = ((retail - sale) / retail) * 100;
                Savings = $"You save {calculatedSavings:F0}%!";
                System.Diagnostics.Debug.WriteLine($"   ✅ Calculated Savings: {Savings}");
            }
            else
            {
                Savings = "Savings unavailable";
                System.Diagnostics.Debug.WriteLine($"   ❌ Failed to calculate savings");
            }
        }

        private void CheckWishlistStatus()
        {
            var identifier = _dealId ?? _gameInfo?.SteamAppID;
            IsInWishlist = WishlistService.Instance.IsInWishlist(identifier);
        }

        private async Task ToggleWishlistAsync()
        {
            try
            {
                if (_gameInfo == null)
                    return;

                bool success;
                if (IsInWishlist)
                {
                    var identifier = _dealId ?? _gameInfo.SteamAppID;
                    success = await WishlistService.Instance.RemoveFromWishlistAsync(identifier);
                    if (success)
                    {
                        IsInWishlist = false;
                        await Application.Current.MainPage.DisplayAlert("Wishlist",
                            $"{_gameInfo.Name} removed from your wishlist!", "OK");
                    }
                }
                else
                {
                    success = await WishlistService.Instance.AddToWishlistAsync(_gameInfo, _dealId);
                    if (success)
                    {
                        IsInWishlist = true;
                        await Application.Current.MainPage.DisplayAlert("Wishlist",
                            $"{_gameInfo.Name} added to your wishlist!", "OK");
                    }
                }

                if (!success)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Failed to update wishlist. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error toggling wishlist: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    "An error occurred while updating your wishlist.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}