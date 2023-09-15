using Newtonsoft.Json;
using QMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QCReceiving : ContentPage
    {
        private string QRCodeNumber { get; set; }
        public StickerInformation QrCodeData { get; set; }
        public StickerInformation StickerInfo { get; set; }

        public QCReceiving()
        {
            InitializeComponent();
        }
        public QCReceiving(string codeNumber)
        {
            InitializeComponent();
            QRCodeNumber = codeNumber;
        }

        private async Task GetStickerInformation()
        {
            try
            {
                if (!string.IsNullOrEmpty(QRCodeNumber))
                {
                    using (HttpClient client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.ApiBaseUrl) })
                    {
                        string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";

                        //form-data
                        ScanData data = new ScanData
                        {
                            qrcode = QRCodeNumber,
                            site = Commons.GlobalDefines.WorkingSite,
                            sewingline = Commons.GlobalDefines.WorkingLine
                        };
                        FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","SelectOrderQRCodeData"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("da[qrcode]",data.qrcode),
                                new KeyValuePair<string, string>("da[site]",data.site),
                                new KeyValuePair<string, string>("da[sewingline]",data.sewingline),

                                });


                        HttpResponseMessage response = await client.PostAsync(apiUrl, content);



                        string result = await response.Content.ReadAsStringAsync();

                        var resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                        if (response.IsSuccessStatusCode)
                        {

                            if (resData.rescode == 200)
                            {

                                if (resData.data.Count > 0)
                                {
                                   
                                    lblCustomer.Text = resData.data[0].Customer;
                                    lblstyle.Text = resData.data[0].Style;
                                    lblOrder.Text = resData.data[0].BuyMonth;
                                    lblSeason.Text = resData.data[0].Season;
                                    lblcolor.Text = resData.data[0].ColorName;
                                    lblSize.Text = resData.data[0].SizeName;
                                   
                                    lblPO.Text = resData.data[0].PoNo;
                                    lblnumberQR.Text = resData.data[0].CodeId;
                                    if (!string.IsNullOrEmpty(resData.data[0].BundleStatus))
                                    {
                                        if (resData.data[0].BundleStatus.Equals("1"))
                                        {
                                            lblMessage.Text = "Mã QR đã scan";
                                            btnSave.IsVisible = false;
                                            return;
                                        }
                                    }
                                    QrCodeData = resData.data[0];
                                    lblMessage.Text = "";
                                }
                                else
                                {
                                    btnSave.IsVisible = false;
                                    lblMessage.Text = "Không lấy được thông tin";
                                }
                                //resData = new QmsApiResponseData();
                            }
                            if (resData.rescode == 412)
                            {
                                btnSave.IsEnabled = false;
                                lblMessage.Text = "Mã QRCode Đã Scan";
                                // await DisplayAlert("Cảnh Báo", "Mã QRCode đã được đóng gói", "Close");
                                btnSave.IsVisible = false;
                            }
                            if (resData.rescode == 416)
                            {
                                btnSave.IsEnabled = false;
                                lblMessage.Text = " QRCode không khả dụng";
                                btnSave.IsVisible = false;
                                //await DisplayAlert("Cảnh Báo", "Mã QRCode được thông qua", "Close");
                            }
                            if (resData.rescode == 414)
                            {
                                btnSave.IsEnabled = false;
                                lblMessage.Text = " Mã QR được thông qua";
                                btnSave.IsVisible = false;
                                //await DisplayAlert("Cảnh Báo", "Mã QRCode không khả dụng", "Close");

                            }
                            if (resData.rescode == 421)
                            {
                                btnSave.IsEnabled = false;
                                lblMessage.Text = "Lỗi thao tác, vui lòng thử lại";
                                btnSave.IsVisible = false;
                                //await DisplayAlert("Cảnh Báo", "Chuyền may của người dùng bị lỗi", "Close");
                            }
                            if (resData.rescode == 423)
                            {
                                btnSave.IsEnabled = false;
                                lblMessage.Text = "Mã không đúng chuyền may.";
                                btnSave.IsVisible = false;
                            }
                        }
                        else
                        {
                            btnSave.IsVisible = false;
                            await DisplayAlert("Error", response.ReasonPhrase, "Close");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                await DisplayAlert(ex.Source, ex.Message, "Close");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblMessage.Text = "Đang truy vấn dữ liệu...";
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetStickerInformation();

            });

        }
        private async void btnSaved_Clicked(object sender, EventArgs e)
        {
            var client = new HttpClient(new HttpClientHandler());
            if (!string.IsNullOrEmpty(QRCodeNumber))
            {
                string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";

                ScanData data = new ScanData
                {
                    qrcode = QRCodeNumber,
                    site = Commons.GlobalDefines.WorkingSite,
                    sewingline = Commons.GlobalDefines.WorkingLine
                };

                // Scan áo đôi
                if (QrCodeData.BreakDownType.Equals("P") && !QrCodeData.WorkType.Equals(""))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdateSOPrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype",QrCodeData.BreakDownType),
                                new KeyValuePair<string, string>("worktype",QrCodeData.WorkType),
                                new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
                                new KeyValuePair<string, string>("customer",QrCodeData.Customer),
                                new KeyValuePair<string, string>("season",QrCodeData.Season),
                                new KeyValuePair<string, string>("buymonth",QrCodeData.BuyMonth),
                                new KeyValuePair<string, string>("style",QrCodeData.Style),
                                new KeyValuePair<string, string>("color",QrCodeData.ColorName),
                                new KeyValuePair<string, string>("size",QrCodeData.SizeName),

                                new KeyValuePair<string, string>("oemno","choose"),
                                new KeyValuePair<string, string>("qrcode",data.qrcode),
                                new KeyValuePair<string, string>("site",QrCodeData.Factory),
                                new KeyValuePair<string, string>("floor",Commons.GlobalDefines.WorkingFactory),
                                new KeyValuePair<string, string>("sewingline",data.sewingline),
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.Uid),
                                });
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                    // await DisplayAlert("Success", resData.rescode.ToString(), "Close");
                    if (response.IsSuccessStatusCode)
                    {
                        if (resData.rescode == 200)
                        {
                            await Navigation.PopModalAsync();
                            //btnSave.IsVisible = false;
                            // lblMessage.Text = demSL++.ToString();
                        }
                        if (resData.rescode == 411)
                        {
                            btnSave.IsVisible = false;
                            lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }

                // By Ship Dest
                if (QrCodeData.BreakDownType.Equals("S"))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdateSOPrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype","O"),
                                new KeyValuePair<string, string>("worktype",""),
                                new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
                                new KeyValuePair<string, string>("customer",QrCodeData.Customer),
                                new KeyValuePair<string, string>("season",QrCodeData.Season),
                                new KeyValuePair<string, string>("buymonth",QrCodeData.BuyMonth),
                                new KeyValuePair<string, string>("style",QrCodeData.Style),
                                new KeyValuePair<string, string>("color",QrCodeData.ColorName),
                                new KeyValuePair<string, string>("size",QrCodeData.SizeName),

                                new KeyValuePair<string, string>("oemno","choose"),
                                new KeyValuePair<string, string>("qrcode",data.qrcode),
                                new KeyValuePair<string, string>("site",QrCodeData.Factory),
                                new KeyValuePair<string, string>("floor",Commons.GlobalDefines.WorkingFactory),
                                new KeyValuePair<string, string>("sewingline",data.sewingline),
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.Uid),
                                });
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                    // await DisplayAlert("Success", resData.rescode.ToString(), "Close");
                    if (response.IsSuccessStatusCode)
                    {
                        if (resData.rescode == 200)
                        {
                            await Navigation.PopModalAsync();
                            //btnSave.IsVisible = false;
                            // lblMessage.Text = demSL++.ToString();
                        }
                        if (resData.rescode == 411)
                        {
                            btnSave.IsVisible = false;
                            lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }
                // by SO ta dao

                if (QrCodeData.BreakDownType.Equals("O"))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdateSOPrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype",QrCodeData.BreakDownType),
                                new KeyValuePair<string, string>("worktype",""),
                                new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
                                new KeyValuePair<string, string>("customer",QrCodeData.Customer),
                                new KeyValuePair<string, string>("season",QrCodeData.Season),
                                new KeyValuePair<string, string>("buymonth",QrCodeData.BuyMonth),
                                new KeyValuePair<string, string>("style",QrCodeData.Style),
                                new KeyValuePair<string, string>("color",QrCodeData.ColorName),
                                new KeyValuePair<string, string>("size",QrCodeData.SizeName),

                                new KeyValuePair<string, string>("oemno","choose"),
                                new KeyValuePair<string, string>("qrcode",data.qrcode),
                                new KeyValuePair<string, string>("site",QrCodeData.Factory),
                                new KeyValuePair<string, string>("floor",Commons.GlobalDefines.WorkingFactory),
                                new KeyValuePair<string, string>("sewingline",data.sewingline),
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.Uid),
                                });
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                    // await DisplayAlert("Success", resData.rescode.ToString(), "Close");
                    if (response.IsSuccessStatusCode)
                    {
                        if (resData.rescode == 200)
                        {
                            await Navigation.PopModalAsync();
                            //btnSave.IsVisible = false;
                            // lblMessage.Text = demSL++.ToString();
                        }
                        if (resData.rescode == 411)
                        {
                            btnSave.IsVisible = false;
                            lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }

                // by PO
                else
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdatePrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype",QrCodeData.BreakDownType),
                                new KeyValuePair<string, string>("worktype",""),
                                new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
                                new KeyValuePair<string, string>("customer",QrCodeData.Customer),
                                new KeyValuePair<string, string>("season",QrCodeData.Season),
                                new KeyValuePair<string, string>("buymonth",QrCodeData.BuyMonth),
                                new KeyValuePair<string, string>("style",QrCodeData.Style),
                                new KeyValuePair<string, string>("color",QrCodeData.ColorName),
                                new KeyValuePair<string, string>("size",QrCodeData.SizeName),
                                new KeyValuePair<string, string>("pono",QrCodeData.PoNo),
                                new KeyValuePair<string, string>("oemno","choose"),
                                new KeyValuePair<string, string>("qrcode",data.qrcode),
                                new KeyValuePair<string, string>("site",QrCodeData.Factory),
                                new KeyValuePair<string, string>("floor",Commons.GlobalDefines.WorkingFactory),
                                new KeyValuePair<string, string>("sewingline",data.sewingline),
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.Uid),
                                });
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                    // await DisplayAlert("Success", resData.rescode.ToString(), "Close");
                    if (response.IsSuccessStatusCode)
                    {
                        if (resData.rescode == 200)
                        {
                            await Navigation.PopModalAsync();
                            //btnSave.IsVisible = false;
                            // lblMessage.Text = demSL++.ToString();
                        }
                        if (resData.rescode == 411)
                        {
                            btnSave.IsVisible = false;
                            lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }
            }
        }

        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            btnSave.IsEnabled = true;
            await Navigation.PopModalAsync(); //close (remove) result page -> back to scan page, don't use push (open new scan page)
        }      

        private async void btnDefect_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ProductionDefect(QRCodeNumber));
        }

       
    }
}