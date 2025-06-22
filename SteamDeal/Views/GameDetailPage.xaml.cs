using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SteamDeal.Models;
using SteamDeal.ViewModels;

namespace SteamDeal.Views
{
    [QueryProperty(nameof(DealID), "dealID")]
    public partial class GameDetailPage : ContentPage
    {
        private GameDeal _selectedGame;

        public string DealID
        {
            set
            {
                LoadGameDetails(value);
            }
        }

        public GameDetailPage()
        {
            InitializeComponent();
        }

        private void OnAddToWishlistClicked(object sender, EventArgs e)
        {
            // TODO: Add logic for wishlisting a game
            DisplayAlert("Wishlist", "Game added to your wishlist!", "OK");
        }

        private async void LoadGameDetails(string dealId)
        {
            var uri = $"https://www.cheapshark.com/api/1.0/deals?id={dealId}";
            using var http = new HttpClient();

            try
            {
                var responseMessage = await http.GetAsync(uri);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        System.Diagnostics.Debug.WriteLine($"⚠️ 404 - Deal not found for dealID: {dealId}");
                        await DisplayAlert("Unavailable", "This deal is no longer available.", "OK");
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"❌ API error: {responseMessage.StatusCode}");
                        await DisplayAlert("Error", "Unable to load game details.", "OK");
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                    return;
                }
                var result = await responseMessage.Content.ReadFromJsonAsync<GameDeal>();

                if (result == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ No deal data returned for dealID: {dealId}");
                    await DisplayAlert("Unavailable", "Game details not available for this deal.", "OK");
                    await Shell.Current.GoToAsync("//MainPage");
                    return;
                }

                // Create a GameDetailResponse from the GameDeal for compatibility
                var gameDetailResponse = new GameDetailResponse
                {
                    GameInfo = new GameInfo
                    {
                        Title = result.Title,
                        SalePrice = result.SalePrice,
                        RetailPrice = result.NormalPrice,
                        Savings = result.Savings,
                        Thumb = result.Thumb
                    }
                };

                BindingContext = new GameDetailViewModel(gameDetailResponse);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception loading game detail for dealID {dealId}: {ex.Message}");
                await DisplayAlert("Error", $"❌ Exception loading game detail for dealID {dealId}: {ex.Message}", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        private async void OnSteamLogoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
        public void OnHamburgerClicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }
    }
}
