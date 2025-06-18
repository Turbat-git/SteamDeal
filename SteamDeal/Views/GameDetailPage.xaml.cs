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
            try
            {
                var uri = $"https://www.cheapshark.com/api/1.0/deals?id={dealId}";
                using var http = new HttpClient();
                var result = await http.GetFromJsonAsync<GameDetailResponse>(uri);

                if (result != null)
                    BindingContext = new GameDetailViewModel(result);
            }
            catch
            {
                await DisplayAlert("Error", "Failed to load game details.", "OK");
            }
        }
    }
}
