namespace SteamDeal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

            if (Preferences.ContainsKey("UserTheme"))
            {
                var theme = Preferences.Get("UserTheme", "Unspecified");
                App.Current.UserAppTheme = Enum.TryParse<AppTheme>(theme, out var parsedTheme)
                    ? parsedTheme
                    : AppTheme.Unspecified;
            }
        }
    }
}
