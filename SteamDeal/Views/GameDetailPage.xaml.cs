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
        private GameInfo _selectedGame;
        private string _currentDealId;

        public string DealID
        {
            set
            {
                _currentDealId = value;
                LoadGameDetails(value);
            }
        }

        public GameDetailPage()
        {
            InitializeComponent();
        }

        private void OnAddToWishlistClicked(object sender, EventArgs e)
        {
            // This method is now handled by the ViewModel's ToggleWishlistCommand
            // Keeping it for backward compatibility, but it's no longer used
        }

        private async void LoadGameDetails(string dealId)
        {
            if (string.IsNullOrEmpty(dealId))
            {
                System.Diagnostics.Debug.WriteLine("❌ DealID is null or empty");
                await DisplayAlert("Error", "Invalid deal ID.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
                return;
            }

            var uri = $"https://www.cheapshark.com/api/1.0/deals?id={dealId}";
            using var http = new HttpClient();

            try
            {
                System.Diagnostics.Debug.WriteLine($"🔍 Loading game details for dealID: {dealId}");
                System.Diagnostics.Debug.WriteLine($"🔍 API URL: {uri}");

                var responseMessage = await http.GetAsync(uri);
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine($"📡 API Response Status: {responseMessage.StatusCode}");
                System.Diagnostics.Debug.WriteLine($"📡 API Response Content: {responseContent}");

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

                // Parse the full response which includes gameInfo wrapper
                var result = JsonSerializer.Deserialize<GameDetailResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.GameInfo == null)
                {
                    System.Diagnostics.Debug.WriteLine($"⚠️ Failed to deserialize game info for dealID: {dealId}");
                    await DisplayAlert("Unavailable", "Game details not available for this deal.", "OK");
                    await Shell.Current.GoToAsync("//MainPage");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"✅ Successfully loaded game: {result.GameInfo.Name}");

                // Pass the dealId to the ViewModel for wishlist functionality
                BindingContext = new GameDetailViewModel(result, _currentDealId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Exception loading game detail for dealID {dealId}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"❌ Stack trace: {ex.StackTrace}");
                await DisplayAlert("Error", $"Failed to load game details: {ex.Message}", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        public static readonly BindableProperty GameTitleProperty =
            BindableProperty.Create(nameof(GameTitle), typeof(string), typeof(GameCard), default(string));

        public string GameTitle
        {
            get => (string)GetValue(GameTitleProperty);
            set => SetValue(GameTitleProperty, value);
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