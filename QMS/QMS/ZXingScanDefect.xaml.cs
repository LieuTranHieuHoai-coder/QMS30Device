using BarcodeScanner.Mobile;
using Newtonsoft.Json;
using QMS.Languages;
using QMS.Models.QcModels;
using QMS.QC_View;
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
    public partial class ZXingScanDefect : ContentPage
    {
        public ZXingScanDefect()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            CameraScanner.IsScanning = true;
        }

        private async void CameraScanner_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
        {
            try
            {
                var result = e.BarcodeResults[0].DisplayValue;
                CameraScanner.IsScanning = false;
                if (result != null)
                {
                    // QC Recevice 
                    if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("qc (qc receiving)"))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //zxing.IsScanning = false;
                            //Methods.SetIsScanning(false); //disable scanning
                            Commons.GlobalDefines.QcQrCode = result;
                            HttpClient client = new HttpClient();
                            HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + result); // Get info Qrcode ProductionProcessQvn
                            var response_exist = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + result); // Check defect in Process
                            var resultAPI = await response.Content.ReadAsStringAsync();
                            var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);

                            var checkQrcode = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDetailQcQrCode/" + result);

                            if (checkQrcode.ReasonPhrase.Contains("Not Found") || response.ReasonPhrase.Contains("Not Found"))
                            {
                                CameraScanner.IsScanning = false;

                                await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "Ok");
                                CameraScanner.IsScanning = true;
                            }
                            else
                            {
                                var resultQcScan = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/report/UpdateQcScanner?qrCode=" + result + "&uid=" + Commons.GlobalDefines.LoggedUser.Uid);
                                await Navigation.PushModalAsync(new ProductionDefect(result,"recheck"));
                            }

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
            }
        }

        private void btnswitch_Clicked(object sender, EventArgs e)
        {
            if (CameraScanner.CameraFacing == CameraFacing.Front)
            {
                CameraScanner.CameraFacing = CameraFacing.Back;
            }
            else
            {
                CameraScanner.CameraFacing = CameraFacing.Front;
            }
        }

        private async void btnBack_Clicked(object sender, System.EventArgs e)
        {
            //zxing.IsVisible = false;
            await Navigation.PopModalAsync();
        }

        private async void btnscan_Clicked(object sender, System.EventArgs e)
        {
            if (txtqrcode.Text == null)
            {
                //await DisplayAlert("Msg", " Nhập Mã QR", "OK");
                await DisplayAlert(this.Translate("Msg"), this.Translate("InputQR"), "OK");
                txtqrcode.Focus();
            }
            else if (txtqrcode.Text != null)
            {
                string result = txtqrcode.Text;

                // QC Check Code
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("check qrcode (qc)"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        Commons.GlobalDefines.QcQrCode = result;

                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + result);
                        var response_exist = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + result);
                        var resultAPI = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);
                        if (response_exist.ReasonPhrase.Contains("Not Found") && response.ReasonPhrase.Contains("Not Found"))
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "OK");
                            CameraScanner.IsScanning = false;
                            CameraScanner.IsScanning = true;
                        }
                        else
                        {
                            if (data.Broken == 1)
                            {
                                await DisplayAlert(this.Translate("Msg"), this.Translate("C grade"), "OK");
                            }
                            if (data.Pass == 1)
                            {
                                await DisplayAlert(this.Translate("Msg"), this.Translate("product passed"), "OK");
                            }
                            else
                            {
                                await Navigation.PushModalAsync(new CheckQRCode(result));
                            }
                        }


                    });
                }
                // QC Recevice 
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("qc (qc receiving)"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Commons.GlobalDefines.QcQrCode = result;
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + result); // Get info Qrcode ProductionProcessQvn
                        var response_exist = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + result); // Check defect in Process
                        var resultAPI = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);

                        var checkQrcode = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDetailQcQrCode/" + result);

                        if (checkQrcode.ReasonPhrase.Contains("Not Found"))
                        {
                            CameraScanner.IsScanning = false;

                            await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "Ok");
                            await Navigation.PushModalAsync(new InputDefect());
                        }
                        else
                        {
                            if (response_exist.ReasonPhrase.Contains("Not Found") && response.ReasonPhrase.Contains("Not Found"))
                            {

                                xProductionProcess process = new xProductionProcess()
                                {
                                    WorkNo = "",
                                    OriginalFactory = Commons.GlobalDefines.WorkingSite,
                                    OperatorFactory = Commons.GlobalDefines.WorkingSite,
                                    Floor = Commons.GlobalDefines.WorkingFactory,
                                    Customer = Commons.GlobalDefines.Customer,
                                    BuyMonth = Commons.GlobalDefines.BuyMonth,
                                    Season = Commons.GlobalDefines.Season,
                                    OrderNo = "",
                                    SewingLine = Commons.GlobalDefines.WorkingLine,
                                    StyleID = 1,
                                    Defect = 1,
                                    Style = Commons.GlobalDefines.Style,
                                    ColorName = Commons.GlobalDefines.ColorName,
                                    SizeName = Commons.GlobalDefines.SizeName,
                                    Qrcode = Commons.GlobalDefines.QcQrCode,
                                    CreateUser = Commons.GlobalDefines.LoggedUser.Uid,
                                    BundleUser = Commons.GlobalDefines.LoggedUser.Uid
                                };
                                var json = JsonConvert.SerializeObject(process);
                                var apiData = new StringContent(json, Encoding.UTF8, "application/json");
                                var newRow = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/InsertxProductionProcess", apiData);

                                if (newRow.IsSuccessStatusCode)
                                {
                                    var resultNewRow = await newRow.Content.ReadAsStringAsync();
                                    var dataNewRow = JsonConvert.DeserializeObject<DetailQcQrCode>(resultNewRow);

                                    DetailQcQrCode qrCode = new DetailQcQrCode()
                                    {
                                        Pcode = dataNewRow.Pcode,
                                        QrCode = Commons.GlobalDefines.QcQrCode,
                                        UId = Commons.GlobalDefines.LoggedUser.Uid,
                                        CreateBy = Commons.GlobalDefines.LoggedUser.Uid,
                                        Status = "Defect"
                                    };
                                    var jsonDetail = JsonConvert.SerializeObject(qrCode);
                                    var apiDetail = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                                    var newDetail = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/DetailQcQrCode", apiDetail);
                                    if (newDetail.IsSuccessStatusCode)
                                    {
                                        await Navigation.PushModalAsync(new SearchDefect());
                                        CameraScanner.IsScanning = false;
                                    }
                                    else
                                    {
                                        CameraScanner.IsScanning = false;

                                        await DisplayAlert(this.Translate("Msg"), this.Translate("Add DetaiQcQrCode failed"), "Ok");
                                        CameraScanner.IsScanning = true;

                                    }
                                }
                                else
                                {
                                    CameraScanner.IsScanning = false;

                                    await DisplayAlert(this.Translate("Msg"), this.Translate("Add ProductionProcess failed"), "Ok");
                                    CameraScanner.IsScanning = true;

                                }

                                //CameraScanner.IsScanning = true;
                            }
                            else
                            {
                                if (data.Broken == 1)
                                {
                                    var action = await DisplayAlert(this.Translate("Msg"), this.Translate("Shirt C. Do you want to Recycle ?"), "Yes", "No");
                                    if (action)
                                    {
                                        var resultQcScan = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/report/UpdateQcScanner?qrCode=" + result + "&uid=" + Commons.GlobalDefines.LoggedUser.Uid);

                                        await Navigation.PushModalAsync(new ProductionDefect(result));
                                        var reworkAPI = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/rework/ReQC?qr=" + result + "&user=" + Commons.GlobalDefines.LoggedUser.Uid);
                                    }
                                    else
                                    {
                                        CameraScanner.IsScanning = true;
                                    }
                                }
                                if (data.Pass == 1)
                                {
                                    var action = await DisplayAlert(this.Translate("Msg"), "Áo Đã Pass. Bạn có muốn sửa lại ?", "Yes", "No");
                                    if (action)
                                    {
                                        var resultQcScan = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/report/UpdateQcScanner?qrCode=" + result + "&uid=" + Commons.GlobalDefines.LoggedUser.Uid);

                                        await Navigation.PushModalAsync(new ProductionDefect(result));
                                        var reworkAPI = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/rework/ReQC?qr=" + result + "&user=" + Commons.GlobalDefines.LoggedUser.Uid);
                                    }
                                    else
                                    {
                                        CameraScanner.IsScanning = true;
                                    }
                                }
                                else
                                {
                                    var resultQcScan = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/report/UpdateQcScanner?qrCode=" + result + "&uid=" + Commons.GlobalDefines.LoggedUser.Uid);

                                    await Navigation.PushModalAsync(new ProductionDefect(result));
                                }
                            }
                        }

                    });
                }
                // FG
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("finish"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new ResultQR(result));

                    });
                }
                // Semi Good
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("semi"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new ResultSemiGood(result));

                    });
                }

                // Packing
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("packing"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        //zxing.IsScanning = false;
                        //Methods.SetIsScanning(false); //disable scanning
                        await Navigation.PushModalAsync(new ResultQR(result));

                    });
                }

                txtqrcode.Text = "";
            }

        }
    }
}