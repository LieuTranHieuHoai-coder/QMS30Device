using Newtonsoft.Json;
using Plugin.Media;
using QMS.Languages;
using QMS.Models;
using QMS.Models.QcModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchDefect : ContentPage
    {
        private List<xDefectCause> xDefects = new List<xDefectCause>();
        public List<DefectPositionView> defectPositionViews = new List<DefectPositionView>();
        public SearchDefect()
        {
            InitializeComponent();
            //BindingContext = new InOutSideView();
            getdata();
            showDefectList(Commons.GlobalDefines.QcQrCode);
        }

        
        private async void getdata()
        {
            try
            {
                listPosition.BindingContext = new InOutSideView();
                Commons.GlobalDefines.ImageID = "Front";
                
                HttpClient client = new HttpClient();

                //Get workno
                string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetParentDefect?site=" + Commons.GlobalDefines.WorkingSite;
                HttpResponseMessage message = await client.GetAsync(apiUrl);

                string result = await message.Content.ReadAsStringAsync();
                List<xDefectCause> info = new List<xDefectCause>(JsonConvert.DeserializeObject<List<xDefectCause>>(result));
                if (Commons.GlobalDefines.Language.Contains("cn"))
                {
                    for (int i = 0; i < info.Count; i++)
                    {
                        info[i].viVN = info[i].zhCN;
                    }
                }
                else
                {
                    if (Commons.GlobalDefines.Language.Contains("tw"))
                    {
                        for (int i = 0; i < info.Count; i++)
                        {
                            info[i].viVN = info[i].zhTW;
                        }
                    }
                    else
                    {
                        if (Commons.GlobalDefines.Language.Contains("en"))
                        {
                            for (int i = 0; i < info.Count; i++)
                            {
                                info[i].viVN = info[i].enUS;
                            }
                        }
                    }

                }
                xDefects = info;
            }
            catch (Exception)
            {

                throw;
            }
            
            
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                var defectCause = (xDefectCause)button.CommandParameter;
                Commons.GlobalDefines.DefectID = defectCause.ParentID.ToString();
                Commons.GlobalDefines.ReasonID = defectCause.xID.ToString();
                Commons.GlobalDefines.ImageID = "Front";
                button.BackgroundColor = Color.Green;
                button.TextColor = Color.White;
                btnPosition.IsEnabled = true;
                inside.IsEnabled = true;
                outside.IsEnabled = true;
                
            }
            
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupFunction());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                Commons.GlobalDefines.PartID = button.CommandParameter.ToString();
                try
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NOPICTURE"), "OK");
                        return;
                    }

                    var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        SaveToAlbum = true,
                        DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
                    });

                    if (file == null)
                        return;

                    var bytes = new byte[file.GetStream().Length];
                    await file.GetStream().ReadAsync(bytes, 0, (int)file.GetStream().Length);
                    string base64 = System.Convert.ToBase64String(bytes);

                    DefectPointData defect = new DefectPointData()
                    {
                        OperatorFactory = Commons.GlobalDefines.WorkingSite,
                        Floor = Commons.GlobalDefines.WorkingFactory,
                        SewingLine = Commons.GlobalDefines.WorkingLine,
                        Qrcode = Commons.GlobalDefines.QcQrCode,
                        DefectID = Commons.GlobalDefines.DefectID,
                        ReasonID = Commons.GlobalDefines.ReasonID,
                        PartID = Commons.GlobalDefines.PartID,
                        PartStatus = "",
                        ImageID = Commons.GlobalDefines.ImageID,
                        ScaleX = 0.0,
                        ScaleY = 0.0,
                        ImgType = "JPG",
                        ImgBytes = base64,
                        Status = "A",
                        CreateUser = Commons.GlobalDefines.LoggedUser.Uid,
                        CreateDated = DateTime.Now,
                        DirectID = Commons.GlobalDefines.DirectionID,
                        Pcode = Commons.GlobalDefines.Pcode
                    };
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(defect);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefect", data);
                    if (response.IsSuccessStatusCode)
                    {
                        Commons.GlobalDefines.DefectID = "";
                        Commons.GlobalDefines.PartID = "";
                        Commons.GlobalDefines.ReasonID = "";
                        Commons.GlobalDefines.DirectionID = 8;
                        checkBottom.IsChecked = false;
                        checkCenterLeft.IsChecked = false;
                        checkCenterRight.IsChecked = false;
                        checkLeft.IsChecked = false;
                        checkMid.IsChecked = false;
                        checkTop.IsChecked = false;
                        checkRight.IsChecked = false;
                       
                        showDefectList(Commons.GlobalDefines.QcQrCode);
                        
                    }
                    else
                    {
                        await DisplayAlert("Status", response.StatusCode.ToString(), "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert(this.Translate("Msg"), this.Translate("permissiontothedevice"), "OK");
                }
            }
        }

        public async void showDefectList(string qr)
        {
            try
            {
                defectView.IsVisible = true;
                HttpClient client = new HttpClient();

                var response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + qr);

                var result = await response.Content.ReadAsStringAsync();
                if (response.ReasonPhrase.Contains("Not Found"))
                {
                    defectView.IsVisible = false;
                }
                else
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var message = JsonConvert.DeserializeObject<List<DefectPositionView>>(result);
                        if (message == null)
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("NODEFONGARMENT"), "OK");
                        }
                        else
                        {
                            defectPositionViews = JsonConvert.DeserializeObject<List<DefectPositionView>>(result);

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

                            defectView.ItemsSource = defectPositionViews.Where(x => x.Status.Equals("A"));
                        }
                    }
                    else
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NODEFONGARMENT"), "OK");
                        defectView.IsVisible = false;
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        private void inside_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.ImageID = "Back";
            
            outside.TextColor = Color.Black;
            outside.BackgroundColor = Color.White;
            inside.TextColor = Color.White;
            inside.BackgroundColor = Color.Green;
            
        }

        private void outside_Clicked(object sender, EventArgs e)
        {
            Commons.GlobalDefines.ImageID = "Front";
            
            inside.TextColor = Color.Green;
            inside.BackgroundColor = Color.White;
            outside.TextColor = Color.White;
            outside.BackgroundColor = Color.Black;
            
        }

        [Obsolete]
        private  void btndefectFab_Clicked(object sender, EventArgs e)
        {
            btnPosition.IsEnabled = true;
            listPosition.IsEnabled = true;
            PopupNavigation.PushAsync(new AllDefect(47));
        }

        [Obsolete]
        private void btndefectWork_Clicked(object sender, EventArgs e)
        {
            btnPosition.IsEnabled = true;
            listPosition.IsEnabled = true;
            PopupNavigation.PushAsync(new AllDefect(48));
        }

        [Obsolete]
        private void btndefectClea_Clicked(object sender, EventArgs e)
        {
            btnPosition.IsEnabled = true;
            listPosition.IsEnabled = true;
            PopupNavigation.PushAsync(new AllDefect(49));
        }

        [Obsolete]
        private void btndefectLogo_Clicked(object sender, EventArgs e)
        {
            btnPosition.IsEnabled = true;
            listPosition.IsEnabled = true;
            PopupNavigation.PushAsync(new AllDefect(50));
        }

        [Obsolete]
        private void btndefectSeam_Clicked(object sender, EventArgs e)
        {
            btnPosition.IsEnabled = true;
            listPosition.IsEnabled = true;
            PopupNavigation.PushAsync(new AllDefect(51));
        }

        private void checkLeft_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 1;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            //checkLeft.IsChecked = true;
            checkRight.IsChecked = false;
            checkLeftRight.IsChecked = false;;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkCenterRight.IsChecked = false;
        }

        private void checkTop_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 3;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            //checkTop.IsChecked = true;
            checkCenterLeft.IsChecked = false;
            checkLeftRight.IsChecked = false;;
            checkCenterRight.IsChecked = false;
        }

        private void checkBottom_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 4;
            checkMid.IsChecked = false;
            //checkBottom.IsChecked = true;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkLeftRight.IsChecked = false;;
            checkCenterRight.IsChecked = false;
        }

        private void checkRight_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 2;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            //checkRight.IsChecked = true;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkLeftRight.IsChecked = false;;
            checkCenterRight.IsChecked = false;
        }

        private void checkMid_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 5;
            //checkMid.IsChecked = true;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkCenterRight.IsChecked = false;
            checkLeftRight.IsChecked = false;;
        }

        private void checkCenterLeft_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 6;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            checkTop.IsChecked = false;
            //checkCenterLeft.IsChecked = true;
            checkLeftRight.IsChecked = false;;
            checkCenterRight.IsChecked = false;
        }

        private void checkCenterRight_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 7;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkLeftRight.IsChecked = false;;
            //checkCenterRight.IsChecked = true;
        }

        private void checkLeftRight_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Commons.GlobalDefines.DirectionID = 9;
            checkMid.IsChecked = false;
            checkBottom.IsChecked = false;
            checkLeft.IsChecked = false;
            checkRight.IsChecked = false;
            checkTop.IsChecked = false;
            checkCenterLeft.IsChecked = false;
            checkCenterRight.IsChecked = false;
        }

        private void Continue_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new InputDefect());
        }

        private void modify_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ProductionDefect(Commons.GlobalDefines.QcQrCode,"search"));
        }
    }
}