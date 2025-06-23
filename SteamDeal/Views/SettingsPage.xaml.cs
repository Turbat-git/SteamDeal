namespace SteamDeal.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
        SetThemeToggle();
    }

    private void OnThemeToggled(object sender, ToggledEventArgs e)
    {

        if (Application.Current is App myapp)
        {
            myapp.UserAppTheme = ThemeSwitch.IsToggled ? AppTheme.Dark : AppTheme.Light;
            Preferences.Set("AppDarkTheme", ThemeSwitch.IsToggled);
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
    private void SetThemeToggle()
    {
        if (Application.Current is App myapp)
        {
            switch (myapp.UserAppTheme)
            {
                case AppTheme.Light:
                    ThemeSwitch.IsToggled = false;
                    break;
                case AppTheme.Dark:
                    ThemeSwitch.IsToggled = true;
                    break;
                default:
                    ThemeSwitch.IsToggled = true;
                    break;
            }
        }
    }   
}