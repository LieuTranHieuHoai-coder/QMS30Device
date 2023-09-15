using Newtonsoft.Json;
using QMS.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QMS.Languages;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultSemiGood : ContentPage
    {
        private string QRCodeNumber { get; set; }
        public StickerInformation QrCodeData { get; set; }
        public ResultSemiGood()
        {
            InitializeComponent();

        }
        public ResultSemiGood(string codeNumber)
        {
            InitializeComponent();
            QRCodeNumber = codeNumber;
        }

        private async Task GetStickerInformation()
        {

            if (!string.IsNullOrEmpty(QRCodeNumber))
            {
                string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";

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


                
                HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string result = await response.Content.ReadAsStringAsync();
               
                var resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);
                if (response.IsSuccessStatusCode)
                {

                    if (resData.rescode == 200)
                    {
                        
                        if (resData.data.Count > 0)
                        {
                            lbWorkNo.Text = resData.data[0].WorkNo;
                            lblCustomer.Text = resData.data[0].Customer;
                            lblstyle.Text = resData.data[0].Style;
                            lblOrder.Text = resData.data[0].BuyMonth;
                            lblSeason.Text = resData.data[0].Season;
                            lblcolor.Text = resData.data[0].ColorName;
                            lblSize.Text = resData.data[0].SizeName;
                            //if (Commons.Method.NoPO.Equals("NoPO"))
                            //{
                            //    lblPO.Text = "";
                            //}
                            //else
                            //{
                            //    lblPO.Text = resData.data[0].PoNo;
                            //}
                            lblPO.Text = resData.data[0].PoNo;
                            lblnumberQR.Text = resData.data[0].CodeId;
                            if (!string.IsNullOrEmpty(resData.data[0].BundleStatus))
                            {
                                if (resData.data[0].BundleStatus.Equals("1"))
                                {
                                    //lblMessage.Text = "Mã QR đã scan";
                                    lblMessage.Text = this.Translate("SCANNED");
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
                            //lblMessage.Text = "Không lấy được thông tin";
                            lblMessage.Text = this.Translate("UNABLETOGETINFOR");
                        }
                        //resData = new QmsApiResponseData();
                    }
                    if (resData.rescode == 412)
                    {
                        btnSave.IsEnabled = false;
                       // lblMessage.Text = "Mã QRCode Đã Scan";
                        lblMessage.Text = this.Translate("SCANNED");
                        // await DisplayAlert("Cảnh Báo", "Mã QRCode đã được đóng gói", "Close");
                        btnSave.IsVisible = false;
                    }
                    if (resData.rescode == 416)
                    {
                        btnSave.IsEnabled = false;
                        //lblMessage.Text = " QRCode không khả dụng";
                        lblMessage.Text = this.Translate("QRNOTAVAILABLE");
                        btnSave.IsVisible = false;
                        //await DisplayAlert("Cảnh Báo", "Mã QRCode được thông qua", "Close");
                    }
                    if (resData.rescode == 414)
                    {
                        btnSave.IsEnabled = false;
                        //lblMessage.Text = " Mã QR được thông qua";
                        lblMessage.Text = this.Translate("QRPASSED");
                        btnSave.IsVisible = false;
                        //await DisplayAlert("Cảnh Báo", "Mã QRCode không khả dụng", "Close");

                    }
                    if (resData.rescode == 421)
                    {
                        btnSave.IsEnabled = false;
                        //lblMessage.Text = "Lỗi thao tác, vui lòng thử lại";
                        lblMessage.Text = this.Translate("ERROR_TRYAGAIN");
                        btnSave.IsVisible = false;
                        //await DisplayAlert("Cảnh Báo", "Chuyền may của người dùng bị lỗi", "Close");
                    }
                    if (resData.rescode == 423)
                    {
                        btnSave.IsEnabled = false;
                        //lblMessage.Text = "Mã không đúng chuyền may.";
                        lblMessage.Text = this.Translate("ERROR");
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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //lblMessage.Text = "Đang truy vấn dữ liệu...";
            lblMessage.Text = this.Translate("QUERYDATA");
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetStickerInformation();

            });

        }
        private async void btnNext_Clicked(object sender, EventArgs e)
        {
            btnSave.IsEnabled = true;
            await Navigation.PopModalAsync(); //close (remove) result page -> back to scan page, don't use push (open new scan page)
        }

        private async void btnSaved_Clicked(object sender, EventArgs e)
        {

            var client = new HttpClient(new HttpClientHandler());
            if (!string.IsNullOrEmpty(QRCodeNumber))
            {
                string apiUrl = "http://qvnqms.qve.com.vn/semidatabase.php";

                ScanData data = new ScanData
                {
                    qrcode = QRCodeNumber,
                    site = Commons.GlobalDefines.WorkingSite,
                    sewingline = Commons.GlobalDefines.WorkingLine
                };
                // by SO
                if (QrCodeData.BreakDownType.Equals("O"))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdateSOPrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype",QrCodeData.BreakDownType),
                                new KeyValuePair<string, string>("worktype",""),
                                //new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
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
                        switch (resData.rescode.ToString())
                        {
                            case "200":
                                btnSave.IsVisible = false;
                                await Navigation.PopModalAsync();

                                break;
                            case "411":
                                btnSave.IsVisible = false;
                                //lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                                lblMessage.Text = this.Translate("Exceeding_The_Amount");
                                break;
                            case "421":
                                btnSave.IsVisible = false;
                                //lblMessage.Text = "Lỗi Thao Tác, Vui Lòng Thử Lại";
                                lblMessage.Text = this.Translate("ERROR_TRYAGAIN");
                                btnSave.IsVisible = false;
                                break;
                            case "412":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            case "416":
                                btnSave.IsVisible = false;
                                // lblMessage.Text = "Lỗi QRCode Không Khả Dụng";
                                lblMessage.Text = this.Translate("QRNOTAVAILABLE");
                                break;
                            case "423":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi Chuyền May Không Đúng";
                                break;
                            case "414":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }
                
                // by SO ta dao
                if (QrCodeData.BreakDownType.Equals("S"))
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdateSOPrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype","0"),
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
                        switch (resData.rescode.ToString())
                        {
                            case "200":
                                btnSave.IsVisible = false;
                                await Navigation.PopModalAsync();

                                break;
                            case "411":
                                btnSave.IsVisible = false;
                                //lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                                lblMessage.Text = this.Translate("Exceeding_The_Amount");
                                break;
                            case "421":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi Thao Tác, Vui Lòng Thử Lại";
                                break;
                            case "412":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            case "416":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Không Khả Dụng";
                                break;
                            case "423":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi Chuyền May Không Đúng";
                                break;
                            case "414":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }

                // By PO
                else
                {
                    FormUrlEncodedContent content = new FormUrlEncodedContent(new[]{
                                new KeyValuePair<string,string>("func","UpdatePrductionProcessQRCode"),
                                //new KeyValuePair<string,string>("da",JsonConvert.SerializeObject(data))
                                new KeyValuePair<string, string>("breaktype",QrCodeData.BreakDownType),
                                new KeyValuePair<string, string>("worktype",""),
                                //new KeyValuePair<string, string>("workno",QrCodeData.WorkNo),
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
                                new KeyValuePair<string, string>("uid",Commons.GlobalDefines.LoggedUser.Uid)
                                });

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string result = await response.Content.ReadAsStringAsync();
                    QmsApiResponseData resData = JsonConvert.DeserializeObject<QmsApiResponseData>(result);

                    if (response.IsSuccessStatusCode)
                    {
                        switch (resData.rescode.ToString())
                        {
                            case "200":
                                btnSave.IsVisible = false;
                                await Navigation.PopModalAsync();

                                break;
                            case "411":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Vượt Quá SL Không Thể Lưu";
                                break;
                            case "421":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi Thao Tác, Vui Lòng Thử Lại";
                                break;
                            case "412":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            case "416":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Không Khả Dụng";
                                break;
                            case "423":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi Chuyền May Không Đúng";
                                break;
                            case "414":
                                btnSave.IsVisible = false;
                                lblMessage.Text = "Lỗi QRCode Đã Tồn Tại";
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", response.ReasonPhrase, "Close");
                    }
                }
                
                
            }
            /*
            Device.BeginInvokeOnMainThread(() =>
            {
                btnSave.IsVisible = false;
            });*/
        }

        // This method must be in a class in a platform project, even if
        // the HttpClient object is constructed in a shared project.
 
    }
}