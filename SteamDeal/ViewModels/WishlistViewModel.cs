using System.Collections.ObjectModel;
using System.Windows.Input;
using SteamDeal.Models;
using SteamDeal.Services;

namespace SteamDeal.ViewModels
{
    public class WishlistViewModel
    {
        private readonly WishlistService _wishlistService;

        public ObservableCollection<WishlistItem> WishlistItems => _wishlistService.WishlistItems;

        public ICommand RemoveCommand { get; }
        public ICommand ClearWishlistCommand { get; }

        // Parameterless constructor for XAML instantiation
        public WishlistViewModel() : this(WishlistService.Instance) { }

        public WishlistViewModel(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;

            RemoveCommand = new Command<string>(async (dealId) =>
            {
                await _wishlistService.RemoveFromWishlistAsync(dealId);
            });

            ClearWishlistCommand = new Command(async () =>
            {
                await _wishlistService.ClearWishlistAsync();
            });
        }
    }
}