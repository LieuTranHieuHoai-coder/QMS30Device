using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS.Commons;
using Rg.Plugins.Popup.Pages;
using QMS.Models.QcModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupTotalDefect : PopupPage
    {
        public PopupTotalDefect()
        {
            InitializeComponent();
            listTotalDefect.BindingContext = new TotalDefectView();
        }
        public PopupTotalDefect(string fd,string td, string sew)
        {
            InitializeComponent();
            listTotalDefect.BindingContext = new TotalDefectView(fd, td, sew);
        }
        private async void close_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
    }
}