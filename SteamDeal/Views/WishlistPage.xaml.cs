using SteamDeal.Services;
using SteamDeal.ViewModels;

namespace SteamDeal.Views;

public partial class WishlistPage : ContentPage
{
    public WishlistPage()
    {
        InitializeComponent();
        BindingContext = new WishlistViewModel(); // uses parameterless constructor
    }
    private async void OnSteamLogoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnBrowseDealsClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    public void OnHamburgerClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
}