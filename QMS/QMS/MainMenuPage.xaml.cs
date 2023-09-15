using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuPage : ContentPage
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private async void ButtonFGMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Setting());
        }
    }
}