namespace SteamDeal
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }
        private async void MainPage_Loaded(object sender, EventArgs e)
        {
            if (BindingContext is ViewModels.MainViewModel vm)
                await vm.LoadDealsAsync();
        }

        //TODO: Add logic to this User Icon Clicked event
        public void OnUserIconClicked()
        {

        }

        //TODO: Add logic to this Hamburger Clicked event
        public void OnHamburgerClicked()
        {

        }
    }

}
