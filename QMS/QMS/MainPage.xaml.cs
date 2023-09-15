using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using QMS.Models.PermitModels;
using System.Threading;
using Xamarin.CommunityToolkit.Helpers;
using QMS.Languages;
using System.Threading.Tasks;
using QMS.Commons;
using QMS.Models.QcModels;

namespace QMS
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // lbltime.Text += DateTime.Now.ToString("T") + " " + DateTime.Now.ToString("dd/MM/yyyy");
            lbltime.Text +=  DateTime.Now.ToString("dd/MM/yyyy");
            
        }

        private void ResizeLogo()
        {
            if (Width > Height)
            {
                PanalLogo.HeightRequest = 80;
                ImgLogo.HeightRequest = 80;

                StackLoginForm.WidthRequest = Height;
            }
            else
            {
                PanalLogo.HeightRequest = 150;
                ImgLogo.HeightRequest = 150;
                StackLoginForm.WidthRequest = Width;
            }
        }        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
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
        private async void Btnlogin_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                if (CheckInternet())
                {
                    var uid = txtuser.Text;
                    var pwd = txtpassword.Text;
                    if (txtuser.Text == null || txtuser.Text.Contains("'") || txtuser.Text.Contains(" ") || txtuser.Text == "" || txtpassword.Text=="")
                    {
                        txtuser.Text = "";
                        await DisplayAlert(this.Translate("ERROR"), this.Translate("ACCOUNT_PW"), "OK");
                        txtuser.Focus();
                    }
                    else
                    {
                        var hashPwd = Commons.Encryptor.MD5Hash(pwd);
                        var authorCode = Commons.Encryptor.MD5Hash(uid + hashPwd);
                        using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                        {
                            var api = $"/login/{authorCode}";
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
                                    }
                                }
                                Application.Current.MainPage = new Setting();
                            }
                            else
                            {
                                await DisplayAlert(this.Translate("Msg"), this.Translate("Invalid Username or Password"), "OK");
                            }
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
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
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

        private bool CheckInternet()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }

            return true;
        }

        private void ShowPass(object sender, EventArgs e)
        {
            txtpassword.IsPassword = txtpassword.IsPassword ? false : true;
        }

        private void ContentPage_SizeChanged(object sender, EventArgs e)
        {
            ResizeLogo();
        }

        private async void btnloginQR_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ZXingScanLogin());
        }

        private async void BtnEnglish_Clicked(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                LocalizationResourceManager.Current.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                Commons.GlobalDefines.Language = "en";
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void BtnVietnamese_Clicked(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("vi-VN");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("vi-VN");
                LocalizationResourceManager.Current.CurrentCulture = new System.Globalization.CultureInfo("vi-VN");
                Commons.GlobalDefines.Language = "vn";
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void BtnTaiwanese_Clicked(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-TW");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-TW");
                LocalizationResourceManager.Current.CurrentCulture = new System.Globalization.CultureInfo("zh-TW");
                Commons.GlobalDefines.Language = "tw";
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void BtnChinese_Clicked(object sender, EventArgs e)
        {
            try
            {
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
                LocalizationResourceManager.Current.CurrentCulture = new System.Globalization.CultureInfo("zh-CN");
                Commons.GlobalDefines.Language = "cn";
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }
    }
}
