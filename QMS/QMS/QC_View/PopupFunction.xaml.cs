using QMS.Commons;
using QMS.Models.PermitModels;
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
    public partial class PopupFunction : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PopupFunction()
        {
            InitializeComponent();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            foreach (var per in GlobalDefines.Permisions)
            {
                if (per.Menu_id == 360)
                {
                    btnQCRECEIVING.IsVisible = true;
                    btnQCScan.IsVisible = true;
                }
                if (per.Menu_id == 361)
                {
                    btnCheckQR.IsVisible = true;
                }
                if (per.Menu_id == 359)
                {
                    btnsewingout.IsVisible = true;
                }
                if (per.Menu_id == 368)
                {
                    btndailyreport.IsVisible = true;
                }
            }
        }

       private async void close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new QC_View.MainMenu());
        }

        private async void btclose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new QC_View.MainMenu());
        }

        

        private async void btnsewingout_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.SlectedPerm = "Finish Good";
            await Navigation.PushModalAsync(new ZXingScanQR());
        }

        private async void btnQCRECEIVING_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.SlectedPerm = "qc (qc receiving)";
            //await Navigation.PushModalAsync(new ZXingScanQR());
            await Navigation.PushModalAsync(new InputDefect());
        }

        private async void btnCheckQR_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.SlectedPerm = "check qrcode (qc)";
            await Navigation.PushModalAsync(new ZXingScanQR());
        }

        private async void btndailyreport_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Reports.DailyReportPage());
        }

        private async void btnQCScan_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.SlectedPerm = "qc (qc receiving)";
            //await Navigation.PushModalAsync(new ZXingScanQR());
            await Navigation.PushModalAsync(new ZXingScanDefect());
        }
    }
}