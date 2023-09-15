using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QMS.Models;
using QMS.Models.QcModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllDefect : Rg.Plugins.Popup.Pages.PopupPage
    {
        
        public AllDefect()
        {
            InitializeComponent();
        }
        public AllDefect(int parenId)
        {
            InitializeComponent();
            getdata(parenId);
        }
        public void getdata(int id)
        {
            listDefect.BindingContext = new ButtonDefectView(id);
        }

        private async void close_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private void Defect_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var defectCause = (xDefectCause)button.CommandParameter;
                Commons.GlobalDefines.DefectID = defectCause.ParentID.ToString();
                Commons.GlobalDefines.ReasonID = defectCause.xID.ToString();
                PopupNavigation.Instance.PopAsync();
            }
        }
    }
}