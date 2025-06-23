namespace SteamDeal
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            CheckTheme();
        }

        private void CheckTheme()
        {
            bool isDarkMode = Preferences.Get("AppDarkTheme", false);
            UserAppTheme = isDarkMode ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
