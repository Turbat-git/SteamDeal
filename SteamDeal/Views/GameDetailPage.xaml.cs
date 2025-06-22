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

                // Debug output to see what we got
                System.Diagnostics.Debug.WriteLine($"📦 Received data:");
                System.Diagnostics.Debug.WriteLine($"   Title: '{result.Title}'");
                System.Diagnostics.Debug.WriteLine($"   Thumb: '{result.Thumb}'");
                System.Diagnostics.Debug.WriteLine($"   SalePrice: '{result.SalePrice}'");
                System.Diagnostics.Debug.WriteLine($"   NormalPrice: '{result.NormalPrice}'");
                System.Diagnostics.Debug.WriteLine($"   Savings: '{result.Savings}'");

                // Create a GameDetailResponse from the GameDeal for compatibility
                var gameDetailResponse = new GameDetailResponse
                {
                    GameInfo = new GameInfo
                    {
                        Title = result.Title ?? "Unknown Game",
                        SalePrice = result.SalePrice ?? "0",
                        RetailPrice = result.NormalPrice ?? "0",
                        Savings = result.Savings ?? "0",
                        Thumb = result.Thumb ?? ""
                    }
                };

                // Debug the created response
                System.Diagnostics.Debug.WriteLine($"🔧 Created GameInfo:");
                System.Diagnostics.Debug.WriteLine($"   Title: '{gameDetailResponse.GameInfo.Title}'");
                System.Diagnostics.Debug.WriteLine($"   Thumb: '{gameDetailResponse.GameInfo.Thumb}'");
                System.Diagnostics.Debug.WriteLine($"   SalePrice: '{gameDetailResponse.GameInfo.SalePrice}'");
                System.Diagnostics.Debug.WriteLine($"   RetailPrice: '{gameDetailResponse.GameInfo.RetailPrice}'");
                System.Diagnostics.Debug.WriteLine($"   Savings: '{gameDetailResponse.GameInfo.Savings}'");

                var viewModel = new GameDetailViewModel(gameDetailResponse);

                // Debug the view model
                System.Diagnostics.Debug.WriteLine($"🎯 Created ViewModel:");
                System.Diagnostics.Debug.WriteLine($"   Title: '{viewModel.Title}'");
                System.Diagnostics.Debug.WriteLine($"   Image: '{viewModel.Image}'");
                System.Diagnostics.Debug.WriteLine($"   Price: '{viewModel.Price}'");
                System.Diagnostics.Debug.WriteLine($"   NormalPrice: '{viewModel.NormalPrice}'");
                System.Diagnostics.Debug.WriteLine($"   Savings: '{viewModel.Savings}'");

                BindingContext = viewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception loading game detail for dealID {dealId}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                await DisplayAlert("Error", $"Failed to load game details: {ex.Message}", "OK");
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