using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamDeal.Models;
using SteamDeal.ViewModels;

namespace SteamDeal.Views
{
    public partial class GameDetailPage : ContentPage
    {
        public GameDetailPage(GameDeal selectedGame)
        {
            InitializeComponent();
            BindingContext = new GameDetailViewModel(selectedGame);
        }

        private void OnAddToWishlistClicked(object sender, EventArgs e)
        {
            // TODO: Add logic for wishlisting a game
            DisplayAlert("Wishlist", "Game added to your wishlist!", "OK");
        }

        private GameDeal _selectedGame;

        public GameDeal SelectedGame
        {
            get => _selectedGame;
            set
            {
                _selectedGame = value;
                BindingContext = new GameDetailViewModel(_selectedGame);
            }
        }
    }
}
