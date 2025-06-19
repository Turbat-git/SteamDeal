namespace SteamDeal.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
        ThemeSwitch.IsToggled = App.Current.UserAppTheme == AppTheme.Dark;
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {
        var newTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        App.Current.UserAppTheme = newTheme;

        Preferences.Set("UserTheme", newTheme.ToString());

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