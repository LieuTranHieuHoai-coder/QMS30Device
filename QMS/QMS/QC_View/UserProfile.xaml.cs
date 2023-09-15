using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : Rg.Plugins.Popup.Pages.PopupPage
    {
        public UserProfile()
        {
            InitializeComponent();
            uid.Text = Commons.GlobalDefines.LoggedUser.Uid;
            name.Text = Commons.GlobalDefines.LoggedUser.Fullname;
            site.Text = Commons.GlobalDefines.WorkingSite;
            factory.Text = Commons.GlobalDefines.WorkingFactory;
            sewing.Text = Commons.GlobalDefines.WorkingLine;
        }

        private void btnchange_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new Setting();
        }

        private async void close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnlogoyt_Clicked(object sender, EventArgs e)
        {
            try
            {
                Commons.GlobalDefines.ClearAll();
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
            }
        }
    }
}