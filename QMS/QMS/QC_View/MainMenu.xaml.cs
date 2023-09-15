using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : ContentPage
    {
        public MainMenu()
        {
            InitializeComponent();
            btnloginsystem.Text = Commons.GlobalDefines.LoggedUser.Fullname;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Commons.GlobalDefines.LoggedUser != null)
            {
                foreach (var per in Commons.GlobalDefines.Permisions)
                {
                    if (per.Menu_id == 358)
                    {
                        btnFG.IsVisible = true;
                    }
                    if (per.Menu_id == 363)
                    {
                        BtnPacking.IsVisible = true;
                    }
                    if (per.Menu_id == 454)
                    {
                        BtnSemi.IsVisible = true;
                    }
                }
            }
            else
            {
                Application.Current.MainPage = new MainPage();
            }
        }

        private async void btnloginsystem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserProfile());
        }

        private async void btnFG_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupFunction());
        }
    }
}