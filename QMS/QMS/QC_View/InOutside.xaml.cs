using QMS.Models.QcModels;
using QMS.QC_View;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS.Languages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Helpers;
using Plugin.Media;
using System.Net.Http;
using Newtonsoft.Json;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InOutside : Rg.Plugins.Popup.Pages.PopupPage
    {
        public InOutside()
        {
            InitializeComponent();
            BindingContext = new InOutSideView();
        }

        public InOutside(ObservableCollection<ReturnBtn> returnBtns)
        {
            InitializeComponent();
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<ReturnBtn> search = new ObservableCollection<ReturnBtn>();
            // BindingContext = myList.Where(s => s.DefectBtn.StartsWith(e.NewTextValue));
        }

        private void btnOnside_Clicked(object sender, EventArgs e)
        {
            if (btnInside.BackgroundColor == Color.FromHex("#343a40"))
            {
               
                btnInside.BackgroundColor = Color.Green;
                BindingContext = new InOutSideView();
                //btnInside.Text = "Áo Ngoài";
                if (Commons.GlobalDefines.Language.Contains("vn"))
                {
                    btnInside.Text = "Áo Ngoài";
                }
                else
                {
                    if (Commons.GlobalDefines.Language.Contains("cn"))
                    {
                        btnInside.Text = "衣服外部";
                    }
                    else
                    {
                        if (Commons.GlobalDefines.Language.Contains("tw"))
                        {
                            btnInside.Text = "衣服外部";
                        }
                        else
                        {
                            if (Commons.GlobalDefines.Language.Contains("en"))
                            {
                                btnInside.Text = "Outside";
                            }
                        }

                    }

                }
                Commons.GlobalDefines.DotFail_In_Out = this.Translate("Outside");
                Commons.GlobalDefines.ImageID = "Front";
            }
            else
            {
                btnInside.BackgroundColor = Color.FromHex("#343a40");
                BindingContext = new InOutSideView("change");
                if (Commons.GlobalDefines.Language.Contains("vn"))
                {
                    btnInside.Text = "Áo Trong";
                }
                else
                {
                    if (Commons.GlobalDefines.Language.Contains("cn"))
                    {
                        btnInside.Text = "衣服內部";
                    }
                    else
                    {
                        if (Commons.GlobalDefines.Language.Contains("tw"))
                        {
                            btnInside.Text = "衣服內部";
                        }
                        else
                        {
                            if (Commons.GlobalDefines.Language.Contains("en"))
                            {
                                btnInside.Text = "Inside";
                            }
                        }

                    }

                }
                Commons.GlobalDefines.DotFail_In_Out = this.Translate("Inside");
                Commons.GlobalDefines.ImageID = "Back";
            }
        }

        private async void close_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                //Commons.GlobalDefines.DotFailLV_2 = button.Text.ToUpper();
                Commons.GlobalDefines.PartID = button.CommandParameter.ToString();
                //Commons.GlobalDefines.BtnPhotoStatus = true;
                //PopupNavigation.Instance.PopAsync();
                QMSInOutside.IsVisible = false;
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
                    //Lưu vô máy
                    //this.imgcamara.Source = ImageSource.FromStream(() =>
                    //{
                    //    var stream = file.GetStream();
                    //    return stream;
                    //});

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

                    };
                    var client = new HttpClient();
                    var json = JsonConvert.SerializeObject(defect);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefect", data);
                    if (response.IsSuccessStatusCode)
                    {
                        
                        await Navigation.PushModalAsync(new ProductionDefect(defect.Qrcode));
                        await PopupNavigation.Instance.PopAsync();
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
    }
}