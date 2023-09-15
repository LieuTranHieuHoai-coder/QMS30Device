using Newtonsoft.Json;
using QMS.Languages;
using QMS.Models.PermitModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BarcodeScanner.Mobile;
using QMS.Commons;
using QMS.Models.QcModels;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZXingScanLogin : ContentPage
    {
        public ZXingScanLogin()
        {
            InitializeComponent();
            Methods.AskForRequiredPermission();
            Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //zxing.IsScanning = true;
            //zxing.Options.UseFrontCameraIfAvailable = true;
            //zxing.Options.UseFrontCameraIfAvailable = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //zxing.IsScanning = false;
        }


        private bool CheckInternet()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }

        private async void btnback_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MainPage());
        }

        private void ResizeLogo()
        {
            if (Width > Height)
            {
                frmlogin.HeightRequest = 300;

                frmlogin.WidthRequest = Height;
            }
            else
            {
                frmlogin.HeightRequest = 400;
                frmlogin.WidthRequest = Width;
            }
        }
        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            ResizeLogo();
        }

        private async Task GetPermision(int id)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                {
                    var api = $"/getperm/{id}";
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            var perms = JsonConvert.DeserializeObject<List<AdminGroupPerm>>(content);
                            Commons.GlobalDefines.Permisions.AddRange(perms);
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SetWorkPlace(string uid)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(GlobalDefines.NewApiUrl) })
                {
                    var api = "/qc/master/GetUserWorkPlace/" + uid;
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            var place = JsonConvert.DeserializeObject<UserWorkPlace>(content);
                            if (place != null)
                            {
                                GlobalDefines.WorkingFactory = place.WorkingFact;
                                GlobalDefines.WorkingSite = place.WorkingSite;
                                GlobalDefines.WorkingLine = place.WorkingLine;
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("SetWorkPlace", response.ReasonPhrase, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
        {
            try
            {
                GVScanner.IsScanning = false;
                string qrcode = e.BarcodeResults[0].DisplayValue;

                if (CheckInternet())
                {
                    using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                    {
                        var api = $"/login/{qrcode}";
                        var response = await client.GetAsync(api);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(content))
                            {
                                var user = JsonConvert.DeserializeObject<Models.Common.xUser>(content);
                                if (user != null)
                                {
                                    GlobalDefines.LoggedUser = user;
                                    await GetPermision(user.Id);
                                    await SetWorkPlace(user.Uid);
                                    Application.Current.MainPage = new Setting();
                                }
                            }
                            
                        }
                        else
                        {
                            await DisplayAlert("Error", response.ReasonPhrase, "OK");
                            GVScanner.IsScanning = true;
                        }
                    }
                }
                else
                {
                    await DisplayAlert(this.Translate("Msg"), this.Translate("CheckInternet"), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private void btnswitch_Clicked(object sender, EventArgs e)
        {
            if (GVScanner.CameraFacing == CameraFacing.Front)
            {
                GVScanner.CameraFacing = CameraFacing.Back;
            }
            else
            {
                GVScanner.CameraFacing = CameraFacing.Front;
            }
            
        }
    }
}