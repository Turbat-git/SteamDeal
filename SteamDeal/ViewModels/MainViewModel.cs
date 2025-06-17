using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using SteamDeal.Models;
using SteamDeal.Views;

namespace SteamDeal.ViewModels
{
    public class MainViewModel
    {
        public ObservableCollection<GameDeal> TopDeals { get; set; } = new();

        public async Task LoadDealsAsync()
        {
            try
            {
                var uri = new Uri("https://www.cheapshark.com/api/1.0/deals?storeID=1&sortBy=dealRating");

                using var http = new HttpClient();
                var allDeals = await http.GetFromJsonAsync<List<GameDeal>>(uri);

                var top10 = allDeals?
                    .OrderByDescending(d => double.TryParse(d.DealRating, out var r) ? r : 0)
                    .Take(10);

                if (top10 != null)
                {
                    TopDeals.Clear();
                    foreach (var deal in top10)
                        TopDeals.Add(deal);
                }
            }
            catch (Exception ex)
            {
                // Handle or log the error
            }
        }

        private async void OnGameSelected(GameDeal selectedGame)
        {
            if (selectedGame == null)
                return;

            await Shell.Current.GoToAsync(nameof(GameDetailPage), true, new Dictionary<string, object>
            {
                { "SelectedGame", selectedGame }
            });
        }
    }
}
