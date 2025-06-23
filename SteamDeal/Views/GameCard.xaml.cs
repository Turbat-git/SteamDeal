using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace SteamDeal.Views
{
    public partial class GameCard : ContentView
    {
        public GameCard()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty GameTitleProperty =
            BindableProperty.Create(nameof(GameTitle), typeof(string), typeof(GameCard), default(string));

        public string GameTitle
        {
            get => (string)GetValue(GameTitleProperty);
            set => SetValue(GameTitleProperty, value);
        }

        public static readonly BindableProperty PriceProperty =
            BindableProperty.Create(nameof(Price), typeof(string), typeof(GameCard), default(string));

        public string Price
        {
            get => (string)GetValue(PriceProperty);
            set => SetValue(PriceProperty, value);
        }

        public static readonly BindableProperty ImageUrlProperty =
            BindableProperty.Create(nameof(ImageUrl), typeof(string), typeof(GameCard), default(string));

        public string ImageUrl
        {
            get => (string)GetValue(ImageUrlProperty);
            set => SetValue(ImageUrlProperty, value);
        }

        // Add the TapCommand bindable property
        public static readonly BindableProperty TapCommandProperty =
            BindableProperty.Create(nameof(TapCommand), typeof(ICommand), typeof(GameCard), null);

        public ICommand TapCommand
        {
            get => (ICommand)GetValue(TapCommandProperty);
            set => SetValue(TapCommandProperty, value);
        }
    }
}