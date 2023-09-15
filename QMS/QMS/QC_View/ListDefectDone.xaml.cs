using Newtonsoft.Json;
using QMS.Languages;
using QMS.Models.QcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListDefectDone : ContentPage
    {
        List<DefectPositionView> defectPositionViews = new List<DefectPositionView>();
        public ListDefectDone()
        {
            InitializeComponent();
        }
        public ListDefectDone(string QRCode)
        {
            InitializeComponent();
            Commons.GlobalDefines.QcQrCode = QRCode;
            showDefectList(QRCode);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        public async void showDefectList(string qr)
        {
            try
            {
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositionsDone/" + qr);

                var result = await response.Content.ReadAsStringAsync();
                if (response.ReasonPhrase.Contains("Not Found"))
                {
                    SewingDefectList.IsVisible = false;
                    //await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "OK");
                }
                else
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var message = JsonConvert.DeserializeObject<List<DefectPositionView>>(result);
                        if (message == null)
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("NODEFONGARMENT"), "Ok");
                        }
                        else
                        {
                            defectPositionViews = JsonConvert.DeserializeObject<List<DefectPositionView>>(result);
                            SewingDefectList.ItemsSource = defectPositionViews;
                        }
                    }
                    else
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NOPICTURE"), "OK");
                        SewingDefectList.IsVisible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "OK");
            }
        }
        private async void Image_Button(object sender, EventArgs e)
        {
            if (sender is SwipeItem button)
            {
                var param = (DefectPositionView)button.CommandParameter;
                await Navigation.PushModalAsync(new ShowImage(param.ImgBytes));
            }

        }
        private async void ckbsttus_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton)
            {
                DefectProcessStatus defect = new DefectProcessStatus();
                defect.DefectId = ((DefectPositionView)imageButton.BindingContext).xID;
                defect.Id = ((DefectPositionView)imageButton.BindingContext).Id;
                var json = JsonConvert.SerializeObject(defect);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();

                var action = await DisplayAlert(this.Translate("Msg"), this.Translate("ConfirmRework"), "Yes", "No");

                if (action)
                {
                    var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetDefectAndProcStatus?defectId=" + defect.DefectId + "&procId=" + defect.Id;
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        
                        showDefectList(Commons.GlobalDefines.QcQrCode);
                        //await DisplayAlert(this.Translate("Msg"), this.Translate("DeleteSuccess"), "OK");
                    }
                }
            }
        }
        
        private async void person_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeItem swipe)
                {
                    HttpClient client = new HttpClient();
                    var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectProcessPerson/" + swipe.CommandParameter;
                    HttpResponseMessage response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<DefectProcessPerson>>(result);
                        if (data.Count <= 0)
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("NoWorkersToFix"), "OK");
                            PersonName.Text = "";
                        }
                        else
                        {
                            PersonName.Text = data[0].FullName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        private async void refreshList_Refreshing(object sender, EventArgs e)
        {
            try
            {
                showDefectList(Commons.GlobalDefines.QcQrCode);
                refreshList.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }
    }
}