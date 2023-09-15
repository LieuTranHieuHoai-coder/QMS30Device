using Newtonsoft.Json;
using QMS.Models.QcModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using QMS.Languages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckQRCode : ContentPage
    {
        List<DefectPositionView> defectPositionViews = new List<DefectPositionView>();
        public CheckQRCode()
        {
            InitializeComponent();
        }
        public CheckQRCode(string QRCode)
        {
            InitializeComponent();
            Commons.GlobalDefines.QcQrCode = QRCode;
            //getImage();
            getInfo();
            showDefectList(QRCode);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        private void getInfo()
        {
            QrInfomation.BindingContext = new ProductionDefectInfoView(Commons.GlobalDefines.QcQrCode);
        }
        private async void getImage()
        {
            HttpClient client = new HttpClient();
            string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/qr/GetQrImage/" + Commons.GlobalDefines.QcQrCode;
            HttpResponseMessage message = await client.GetAsync(apiUrl);

            string result = await message.Content.ReadAsStringAsync();

            List<xOrderImage> defectImages = new List<xOrderImage>(JsonConvert.DeserializeObject<List<xOrderImage>>(result));

            //img_front.Source = ImageSource.FromStream(() => new MemoryStream(defectImages[0].ImgBytes));
            //img_back.Source = ImageSource.FromStream(() => new MemoryStream(defectImages[1].ImgBytes));
        }
        private async void btnback_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupFunction());
        }
        public async void showDefectList(string qr)
        {
            try
            {
                HttpClient client = new HttpClient();
                
                HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + qr);
                var responseInfo = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + Commons.GlobalDefines.QcQrCode); // Get info Qrcode ProductionProcessQvn
                var resultAPI = await responseInfo.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);
                Commons.GlobalDefines.Style = data.Style;

                var result = await response.Content.ReadAsStringAsync();
                if (response.ReasonPhrase.Contains("Not Found"))
                {
                    await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "OK");
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
                            //get style, season, buymonth
                            if (!string.IsNullOrEmpty(data.Style))
                            {
                                var temp = data.Style;
                                if (data.Style.Contains("-"))
                                {
                                    temp = data.Style.Split(new string[] { "-" }, StringSplitOptions.None)[0];
                                }
                                var sl = temp.Length;
                                if (sl > 8)
                                {
                                    temp = temp.Substring(sl - 8, 8);
                                }
                                data.Style = temp;
                            }

                            infoStyle.Text = data.Style;
                            infoSeason.Text = data.Season;
                            infoBuymonth.Text = data.BuyMonth;
                            //Commons.GlobalDefines.Style = infoStyle.Text;
                            getImage();

                            if (Commons.GlobalDefines.Language.Contains("vn"))
                            {
                                for (int i = 0; i < defectPositionViews.Count; i++)
                                {
                                    if (defectPositionViews[i].ImageID == "Front")
                                    {
                                        defectPositionViews[i].ImageID = "Áo Chính" + " " + defectPositionViews[i].PartNameVN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].viVN;
                                    }
                                    else
                                    {
                                        defectPositionViews[i].ImageID = "Áo Lót" + " " + defectPositionViews[i].PartNameVN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].viVN;
                                    }

                                }
                            }
                            if (Commons.GlobalDefines.Language.Contains("cn"))
                            {
                                for (int i = 0; i < defectPositionViews.Count; i++)
                                {
                                    defectPositionViews[i].DefectCauseVN = defectPositionViews[i].DefectCauseCN;
                                    defectPositionViews[i].PartNameVN = defectPositionViews[i].PartNameCN;
                                    defectPositionViews[i].ReasonVN = defectPositionViews[i].ReasonCN;
                                    defectPositionViews[i].viVN = defectPositionViews[i].zhCN;
                                    if (defectPositionViews[i].ImageID == "Front")
                                    {
                                        defectPositionViews[i].ImageID = "正面" + " " + defectPositionViews[i].PartNameCN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].zhCN;
                                    }
                                    else
                                    {
                                        defectPositionViews[i].ImageID = "后退" + " " + defectPositionViews[i].PartNameCN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].zhCN;
                                    }
                                }
                            }
                            else
                            {
                                if (Commons.GlobalDefines.Language.Contains("tw"))
                                {
                                    for (int i = 0; i < defectPositionViews.Count; i++)
                                    {
                                        defectPositionViews[i].DefectCauseVN = defectPositionViews[i].DefectCauseTW;
                                        defectPositionViews[i].PartNameVN = defectPositionViews[i].PartNameTW;
                                        defectPositionViews[i].ReasonVN = defectPositionViews[i].ReasonTW;
                                        defectPositionViews[i].viVN = defectPositionViews[i].zhTW;
                                        if (defectPositionViews[i].ImageID == "Front")
                                        {
                                            defectPositionViews[i].ImageID = "正面" + " " + defectPositionViews[i].PartNameTW + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].zhTW;
                                        }
                                        else
                                        {
                                            defectPositionViews[i].ImageID = "后退" + " " + defectPositionViews[i].PartNameTW + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].zhTW;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Commons.GlobalDefines.Language.Contains("en"))
                                    {
                                        for (int i = 0; i < defectPositionViews.Count; i++)
                                        {
                                            defectPositionViews[i].DefectCauseVN = defectPositionViews[i].DefectCauseEN;
                                            defectPositionViews[i].PartNameVN = defectPositionViews[i].PartNameEN;
                                            defectPositionViews[i].ReasonVN = defectPositionViews[i].ReasonEN;
                                            defectPositionViews[i].viVN = defectPositionViews[i].enUS;
                                            if (defectPositionViews[i].ImageID == "Front")
                                            {
                                                defectPositionViews[i].ImageID = "Front" + " " + defectPositionViews[i].PartNameEN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].enUS;
                                            }
                                            else
                                            {
                                                defectPositionViews[i].ImageID = "Back" + " " + defectPositionViews[i].PartNameEN + " " + defectPositionViews[i].DefineCode + " " + defectPositionViews[i].enUS;
                                            }
                                        }
                                    }
                                }

                            }


                            SewingDefectList.ItemsSource = defectPositionViews.Where(x => x.Status.Equals("A"));
                            
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
            if (sender is Frame imageButton)
            {
                DefectProcessStatus defect = new DefectProcessStatus();
                defect.ProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                defect.LastProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                defect.Id = ((DefectPositionView)imageButton.BindingContext).Id;
                defect.DefectId = ((DefectPositionView)imageButton.BindingContext).xID;
                var json = JsonConvert.SerializeObject(defect);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();

                if (((DefectPositionView)imageButton.BindingContext).ProcStatus == null)
                {
                    var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefectProcess", data);
                    showDefectList(Commons.GlobalDefines.QcQrCode); 
                }
                else
                {
                    if (((DefectPositionView)imageButton.BindingContext).ProcStatus == "")
                    {
                        var response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/UpdateDefectProcess/" + defect.Id);
                        showDefectList(Commons.GlobalDefines.QcQrCode);
                    }
                    else
                    {
                        var action = await DisplayAlert(this.Translate("Msg"), this.Translate("Dontfixthisdefect"), "Yes", "No");

                        if (action)
                        {
                            var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/DeleteDefectProcessStatus?statusId=" + defect.Id + "&userId=" + Commons.GlobalDefines.LoggedUser.Uid;
                            var response = await client.GetAsync(api);
                            if (response.IsSuccessStatusCode)
                            {
                                showDefectList(Commons.GlobalDefines.QcQrCode);
                            }
                        }
                    }
                   
                }

            }
        }
        private async void btnContinue_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
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
        private async void btnDone_Clicked(object sender, EventArgs e)
        {
            if (sender is Frame imageButton)
            {
                try
                {
                    DefectProcessStatus defect = new DefectProcessStatus();
                    defect.ProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                    defect.LastProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                    defect.Id = ((DefectPositionView)imageButton.BindingContext).Id;
                    defect.DefectId = ((DefectPositionView)imageButton.BindingContext).xID;
                    var ProcStatus = ((DefectPositionView)imageButton.BindingContext).ProcStatus;

                    if (ProcStatus == "D")
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("Qc is checking"), "OK");
                        PersonName.Text = "";
                    }
                    else
                    {
                        var json = JsonConvert.SerializeObject(defect);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpClient client = new HttpClient();
                        var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/CheckWorkerInProcess/" + defect.DefectId;
                        HttpResponseMessage checkPerson = await client.GetAsync(api);
                        if (checkPerson.IsSuccessStatusCode)
                        {
                            var result = await checkPerson.Content.ReadAsStringAsync();
                            var dataPerson = JsonConvert.DeserializeObject<List<DefectProcessPerson>>(result);
                            if (dataPerson.Count < 1)
                            {
                                var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefectProcessSone", data); //new row DefectProcess = D
                                await DisplayAlert(this.Translate("Msg"), this.Translate("ConfirmRepair"), "OK");
                                imageButton.IsEnabled = false;
                                showDefectList(Commons.GlobalDefines.QcQrCode);
                            }
                            else
                            {
                                if (dataPerson[0].ProcStatus == "")
                                {
                                    await DisplayAlert(this.Translate("Msg"), this.Translate("NoWorkersToFix"), "OK");
                                    PersonName.Text = "";
                                }
                                else
                                {
                                    var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefectProcessSone", data);
                                    await DisplayAlert(this.Translate("Msg"), this.Translate("ConfirmRepair"), "OK");
                                    imageButton.IsEnabled = false;
                                    showDefectList(Commons.GlobalDefines.QcQrCode);
                                }
                            }

                        }
                        else
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("NoWorkersToFix"), "OK");
                            PersonName.Text = "";
                        }
                    }
                    
                    
                }
                catch (Exception)
                {

                    throw;
                }
                

            }
        }
    }
}