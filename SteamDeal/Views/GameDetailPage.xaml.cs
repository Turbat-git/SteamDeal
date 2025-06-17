using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SteamDeal.Models;
using SteamDeal.ViewModels;

namespace SteamDeal.Views
{
    [QueryProperty(nameof(SerializedGame), "SelectedGame")]
    public partial class GameDetailPage : ContentPage
    {
        private GameDeal _selectedGame;

        public string SerializedGame
        {
            get => null;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    try
                    {
                        _selectedGame = JsonSerializer.Deserialize<GameDeal>(Uri.UnescapeDataString(value));
                        BindingContext = new GameDetailViewModel(_selectedGame);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deserializing game: {ex.Message}");
                    }
                }
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
    }
}
