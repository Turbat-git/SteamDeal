using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SteamDeal.Models;
using SteamDeal.Services;

namespace SteamDeal.ViewModels
{
    public class WishlistViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WishlistItem> WishlistItems { get; }
        public ICommand RemoveFromWishlistCommand { get; }
        public ICommand ClearWishlistCommand { get; }
        public ICommand GameSelectedCommand { get; }

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        public WishlistViewModel()
        {
            WishlistItems = WishlistService.Instance.WishlistItems;

            RemoveFromWishlistCommand = new Command<WishlistItem>(OnRemoveFromWishlist);
            ClearWishlistCommand = new Command(OnClearWishlist);
            GameSelectedCommand = new Command<WishlistItem>(OnGameSelected);

            // Subscribe to collection changes to update IsEmpty property
            WishlistItems.CollectionChanged += (s, e) => IsEmpty = !WishlistItems.Any();
            IsEmpty = !WishlistItems.Any();
        }

        private async void OnRemoveFromWishlist(WishlistItem item)
        {
            if (item == null)
                return;

            try
            {
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Remove from Wishlist",
                    $"Remove '{item.Title}' from your wishlist?",
                    "Remove", "Cancel");

                if (result)
                {
                    var success = await WishlistService.Instance.RemoveFromWishlistAsync(item.DealID);
                    if (success)
                    {
                        await Application.Current.MainPage.DisplayAlert("Wishlist",
                            $"{item.Title} removed from your wishlist!", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error",
                            "Failed to remove item from wishlist.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing from wishlist: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    "An error occurred while removing the item.", "OK");
            }
        }

        private async void OnClearWishlist()
        {
            if (!WishlistItems.Any())
                return;

            try
            {
                var result = await Application.Current.MainPage.DisplayAlert(
                    "Clear Wishlist",
                    "Are you sure you want to remove all items from your wishlist?",
                    "Clear All", "Cancel");

                if (result)
                {
                    await WishlistService.Instance.ClearWishlistAsync();
                    await Application.Current.MainPage.DisplayAlert("Wishlist",
                        "Your wishlist has been cleared!", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing wishlist: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    "An error occurred while clearing your wishlist.", "OK");
            }
        }

        private async void OnGameSelected(WishlistItem selectedItem)
        {
            if (selectedItem == null)
                return;

            try
            {
                // Navigate to game detail page using the deal ID
                await Shell.Current.GoToAsync($"///GameDetailPage?dealID={selectedItem.DealID}");
                System.Diagnostics.Debug.WriteLine($"NAVIGATED TO: {selectedItem?.Title}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    "Unable to view game details.", "OK");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}