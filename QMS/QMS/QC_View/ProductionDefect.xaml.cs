using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS.Models.QcModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using SkiaSharp;
using TouchTracking;
using SkiaSharp.Views.Forms;
using Xamarin.Essentials;
using Plugin.Media;
using QMS.QC_View;
using System.IO;
using QMS.Languages;

namespace QMS
{

    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ProductionDefect : ContentPage
    {

        //public virtual void GetLocationOnScreen(int[] outLocation) { }
        //Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();
        //List<SKPath> completedPaths = new List<SKPath>();
        //Dictionary<long, SKPath> inProgressPaths2 = new Dictionary<long, SKPath>();
        //List<SKPath> completedPaths2 = new List<SKPath>();
        //SKPaint paint = new SKPaint
        //{
        //    Style = SKPaintStyle.Stroke,
        //    Color = SKColors.Blue,
        //    StrokeWidth = 10,
        //    StrokeCap = SKStrokeCap.Round,
        //    StrokeJoin = SKStrokeJoin.Round
        //};
        //SKPaint paint2 = new SKPaint
        //{
        //    Style = SKPaintStyle.Stroke,
        //    Color = SKColors.Red,
        //    StrokeWidth = 10,
        //    StrokeCap = SKStrokeCap.Round,
        //    StrokeJoin = SKStrokeJoin.Round
        //};
        private string QRCodeNumber { get; set; }
        private string _key { get; set; }
        public List<DefectPositionView> defectPositionViews = new List<DefectPositionView>();
        public ProductionDefect()
        {
            InitializeComponent();
            //getImage();
            //SKPath path = new SKPath();
            //float x1 = 0;
            //float y1 = 0;
            //float x2 = 200;
            //float y2 = 100;
            //float w = (float)(DeviceDisplay.MainDisplayInfo.Width / 2);
            //float h = w;

            //SKPath path2 = new SKPath();
            //path.AddCircle((float)(((x2 - x1) / h) * w), (float)(((y2 - y1) / w) * h), 5);
            //path.AddCircle(50, 150, 5);
            //inProgressPaths.Add(1, path);
            //canvasView.InvalidateSurface();


            //path.AddCircle(200, 50, 5);
            //path.AddCircle(100, 50, 5);
            //path.AddCircle(50, 50, 5);

            //inProgressPaths.Add(1, path);
            //canvasView.InvalidateSurface();
            //getInfo();

            //getDefectInfo();
            //showDefectList();
            //canvasView.CanvasSize.ToPoint();

        }
        public ProductionDefect(string qrcodedata)
        {
            InitializeComponent();
            //  btnSave.IsVisible = false;
            // photo.IsVisible = false;
            Commons.GlobalDefines.DotFailLV_0 = null;
            Commons.GlobalDefines.DotFailLV_1 = null;
            Commons.GlobalDefines.DotFail_In_Out = null;
            Commons.GlobalDefines.BtnPhotoStatus = false;
            QRCodeNumber = qrcodedata;
            
            //getInfo();
            CountRework();
            //getDefectInfo();
            showDefectList(QRCodeNumber);
            //getImage();
        }
        public ProductionDefect(string qrcodedata,string key)
        {
            InitializeComponent();
            //  btnSave.IsVisible = false;
            // photo.IsVisible = false;
            Commons.GlobalDefines.DotFailLV_0 = null;
            Commons.GlobalDefines.DotFailLV_1 = null;
            Commons.GlobalDefines.DotFail_In_Out = null;
            Commons.GlobalDefines.BtnPhotoStatus = false;
            QRCodeNumber = qrcodedata;
            _key = key;
            //getInfo();
            CountRework();
            //getDefectInfo();
            showDefectList(QRCodeNumber);
            //getImage();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
      
        private async void getImage()
        {
            HttpClient client = new HttpClient();
            string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/qr/GetQrImage/" + Commons.GlobalDefines.Style;
            HttpResponseMessage message = await client.GetAsync(apiUrl);

            string result = await message.Content.ReadAsStringAsync();

            List<xOrderImage> defectImages = new List<xOrderImage>(JsonConvert.DeserializeObject<List<xOrderImage>>(result));

            //img_front.Source = ImageSource.FromStream(() => new MemoryStream(defectImages[0].ImgBytes));
            //img_back.Source = ImageSource.FromStream(() => new MemoryStream(defectImages[1].ImgBytes));
        }

        private void btnSave_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void btnback_Clicked(object sender, EventArgs e)
        {
            if (_key == "recheck")
            {
                await Navigation.PushModalAsync(new ZXingScanDefect());
            }
            if (_key == "search")
            {
                await Navigation.PushModalAsync(new SearchDefect());
            }
            else
            {
                await Navigation.PushModalAsync(new PopupFunction());
            }
            
        }

        private void btnDefect_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }
        public void getDefectInfo()
        {

            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                if (Commons.GlobalDefines.BtnPhotoStatus)
                {
                    photo_Clicked();
                    return false;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (Commons.GlobalDefines.DotFailLV_0 != null && Commons.GlobalDefines.DotFailLV_1 != null && Commons.GlobalDefines.DotFail_In_Out != null)
                        {
                            DotFailLV_0.Text = Commons.GlobalDefines.DotFailLV_0 + " + ";
                            DotFailLV_1.Text = Commons.GlobalDefines.DotFailLV_1 + " + ";
                            DotFail_In_Out.Text = Commons.GlobalDefines.DotFail_In_Out;

                        }

                    });
                }

                return true;
            });

        }

        public async void showDefectList(string qr)
        {
            try
            {
                defectView.IsVisible = true;
                HttpClient client = new HttpClient();
                var responseInfo = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + Commons.GlobalDefines.QcQrCode); // Get info Qrcode ProductionProcessQvn
                var resultAPI = await responseInfo.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);
                Commons.GlobalDefines.Style = data.Style;

                var response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + qr);
                var result = await response.Content.ReadAsStringAsync();

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

                            defectView.ItemsSource = defectPositionViews.Where(x=>x.Status.Equals("A"));
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
        private async void photo_Clicked()
        {
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
                    Qrcode = QRCodeNumber,
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
                    defectView.IsVisible = true;
                    showDefectList(QRCodeNumber);

                }
                else
                {
                    await DisplayAlert("Status", response.StatusCode.ToString(), "OK");
                }

            }
            catch (Exception ex)
            {
                showDefectList(QRCodeNumber);
            }
        }
        private async void Image_Button(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeItem button)
                {
                    var param = (DefectPositionView)button.CommandParameter;
                    await Navigation.PushModalAsync(new ShowImage(param.ImgBytes));
                    PersonName.Text = "";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
            }

        }
        private async void status_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeItem SwipeItem)
                {
                    //var param = (DefectPositionView)button.CommandParameter;
                    var defect = (DefectPositionView)SwipeItem.CommandParameter;
                    if (defect.ProcStatus == null || defect.ProcStatus == "")
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NoWorkersToFix"), "Ok");
                    }
                    else
                    {
                        var action = await DisplayAlert(this.Translate("Msg"), this.Translate("YesToConfirm"), "Yes", "No");
                        if (action)
                        {
                            string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";
                            CookieContainer cookies = new CookieContainer();
                            if (Commons.GlobalDefines.Language.Equals("vn"))
                            {
                                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "vn"));
                            }
                            else
                            {
                                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", Commons.GlobalDefines.Language));
                            }

                            FormUrlEncodedContent form = new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string,string>("func","DefectPositionPointDelByxID"),
                            new KeyValuePair<string, string>("uid", Commons.GlobalDefines.LoggedUser.Uid),
                            new KeyValuePair<string,string>("sid",Commons.GlobalDefines.LoggedUser.Id.ToString()),
                            new KeyValuePair<string, string>("da[xid]", defect.xID.ToString()),
                        });
                            HttpClientHandler handler = new HttpClientHandler();
                            handler.CookieContainer = cookies;
                            HttpClient client = new HttpClient(handler);
                            HttpResponseMessage message = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetDefectPointF?xID=" + defect.xID +"&user=" + Commons.GlobalDefines.LoggedUser.Uid);
                            var setDoneProc = client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetDefectProcessStatusIsDone/" + defect.Id);
                            PersonName.Text = "";
                            showDefectList(QRCodeNumber);
                        }
                    }
                    

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }
        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    //var param = (DefectPositionView)button.CommandParameter;
                    var id = button.CommandParameter.ToString();
                    var action = await DisplayAlert(this.Translate("Msg"), this.Translate("YesToPass"), "Yes", "No");
                    if (action)
                    {
                        string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";
                        CookieContainer cookies = new CookieContainer();
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "vn"));
                        FormUrlEncodedContent form = new FormUrlEncodedContent(new[] {
                            new KeyValuePair<string,string>("func","UpdatePrductionProcessStatusByQRCode"),
                            new KeyValuePair<string, string>("uid", Commons.GlobalDefines.LoggedUser.Uid),
                            new KeyValuePair<string,string>("sid",Commons.GlobalDefines.LoggedUser.Id.ToString()),
                            new KeyValuePair<string, string>("da[pass]", "1"),
                            new KeyValuePair<string, string>("da[defect]", "0"),
                            new KeyValuePair<string, string>("da[broken]", "0"),
                            new KeyValuePair<string, string>("da[qrcode]", Commons.GlobalDefines.QcQrCode),
                        });
                        HttpClientHandler handler = new HttpClientHandler();
                        handler.CookieContainer = cookies;
                        HttpClient client = new HttpClient(handler);
                        HttpResponseMessage message = await client.PostAsync(apiUrl, form);
                        await Navigation.PopModalAsync();
                    }
                    //await DisplayAlert("Thông báo", "Đã sửa chữa", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private void Image_Button_Clicked(object sender, EventArgs e)
        {

        }

        private async void refreshList_Refreshing(object sender, EventArgs e)
        {
            try
            {
                showDefectList(QRCodeNumber);
                CountRework();
                refreshList.IsRefreshing = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void btnContinue_Clicked(object sender, EventArgs e)
        {  
            await Navigation.PushModalAsync(new SearchDefect());
        }

        private async void DeleteDefect_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is SwipeItem swipe)
                {
                    var param = (DefectPositionView)swipe.CommandParameter;
                    HttpClient client = new HttpClient();
                    var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/DeleteDefectPosition/" + param.xID;
                    await client.GetAsync(api);
                    showDefectList(QRCodeNumber);
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "OK");
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
                            if (data[0].ProcStatus == null || data[0].ProcStatus == "")
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

            }
            catch (Exception ex)
            {
                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        private async void ckbsttus_Clicked(object sender, EventArgs e)
        {
            if (sender is ImageButton imageButton)
            {
                DefectProcessStatus defect = new DefectProcessStatus();
                defect.ProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                defect.LastProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                defect.DefectId = ((DefectPositionView)imageButton.BindingContext).xID;
                var json = JsonConvert.SerializeObject(defect);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();

                if (((DefectPositionView)imageButton.BindingContext).HasProcessed > 0)
                {
                    var action = await DisplayAlert(this.Translate("Msg"), this.Translate("Dontfixthisdefect"), "Yes", "No");

                    if (action)
                    {
                        var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/DeleteDefectProcessStatus/" + defect.DefectId;
                        var response = await client.GetAsync(api);
                        if (response.IsSuccessStatusCode)
                        {
                            showDefectList(QRCodeNumber);
                            //await DisplayAlert(this.Translate("Msg"), this.Translate("DeleteSuccess"), "OK");
                        }
                    }
                }
                else
                {
                    var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefectProcess", data);
                    showDefectList(QRCodeNumber);
                }

            }
        }

        private async void Pass_Clicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                
                var action = await DisplayAlert(this.Translate("Msg"), this.Translate("YesToConfirm"), "Yes", "No");

                if (action)
                {
                    var checkQrcode = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDetailQcQrCode/" + Commons.GlobalDefines.QcQrCode);
                    if (checkQrcode.ReasonPhrase.Contains("Not Found"))
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "Ok");
                    }
                    else
                    {
                        for (int i = 0; i < defectPositionViews.Count; i++)
                        {
                            if (defectPositionViews[i].Status == "A" && defectPositionViews[i].ProcStatus == "D")
                            {
                                HttpResponseMessage message = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetDefectPointF?xID=" + defectPositionViews[i].xID + "&user=" + Commons.GlobalDefines.LoggedUser.Uid);
                                //var setDoneProc = client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetDefectProcessStatusIsDone/" + defectPositionViews[i].Id);

                                var newRow = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetPcode/" + Commons.GlobalDefines.QcQrCode);
                                if (newRow.IsSuccessStatusCode)
                                {
                                    var resultNewRow = await newRow.Content.ReadAsStringAsync();
                                    var dataNewRow = JsonConvert.DeserializeObject<List<xProductionProcess>>(resultNewRow);

                                    DetailQcQrCode qrCode = new DetailQcQrCode()
                                    {
                                        Pcode = dataNewRow[0].Pcode.ToString(),
                                        QrCode = Commons.GlobalDefines.QcQrCode,
                                        UId = Commons.GlobalDefines.LoggedUser.Uid,
                                        CreateBy = Commons.GlobalDefines.LoggedUser.Uid,
                                        Status = "Pass"
                                    };
                                    var jsonDetail = JsonConvert.SerializeObject(qrCode);
                                    var apiDetail = new StringContent(jsonDetail, Encoding.UTF8, "application/json");
                                    var newDetail = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/DetailQcQrCode", apiDetail);

                                }
                                else
                                {
                                    await DisplayAlert(this.Translate("Msg"), this.Translate("NotFoundQRCode"), "Ok");
                                }
                            }
                        }

                        var responseAgain = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + Commons.GlobalDefines.QcQrCode);
                        var resultAgain = await responseAgain.Content.ReadAsStringAsync();
                        
                        if (responseAgain.ReasonPhrase.Contains("Not Found"))
                        {
                            var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetPassStatus?qr=" + Commons.GlobalDefines.QcQrCode + "&user=" + Commons.GlobalDefines.LoggedUser.Uid;
                            var response = await client.GetAsync(api);

                            if (response.IsSuccessStatusCode)
                            {
                                await Navigation.PushModalAsync(new ZXingScanDefect());
                                //await DisplayAlert(this.Translate("Msg"), this.Translate("DeleteSuccess"), "OK");
                            }
                        }
                        else
                        {
                            showDefectList(Commons.GlobalDefines.QcQrCode);
                        }
                        
                        
                    }
                }
                
            }
            catch (Exception ex)
            {

                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        private async void CGrade_Clicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();

                var action = await DisplayAlert(this.Translate("Msg"), this.Translate("YesToConfirm"), "Yes", "No");

                if (action)
                {
                    var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/SetBrokenStatus?qr=" + QRCodeNumber + "&user=" + Commons.GlobalDefines.LoggedUser.Uid;
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        await Navigation.PushModalAsync(new ZXingScanQR());
                        //await DisplayAlert(this.Translate("Msg"), this.Translate("DeleteSuccess"), "OK");
                    }
                }

            }
            catch (Exception ex)
            {

                await DisplayAlert("error", ex.Message, "OK");
            }
        }

        private void ListDone_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ListDefectDone(QRCodeNumber));
        }

        private async void CountRework()
        {
            HttpClient client = new HttpClient();
            var api = Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + QRCodeNumber;
            var response = await client.GetAsync(api);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<xProductionProcess>(result);
                countRework.Text = data.Rework.ToString();
                //await DisplayAlert(this.Translate("Msg"), this.Translate("DeleteSuccess"), "OK");
            }
        }

        private async void btnReturn_Clicked(object sender, EventArgs e)
        {
            if (sender is Frame imageButton)
            {
                try
                {
                    //var param = (DefectPositionView)button.CommandParameter;
                    var dataBtn = ((DefectPositionView)imageButton.BindingContext).ProcStatus;
                    DefectProcessStatus defect = new DefectProcessStatus();
                    defect.ProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                    defect.LastProcessPerson = Commons.GlobalDefines.LoggedUser.Uid;
                    defect.Id = ((DefectPositionView)imageButton.BindingContext).Id;
                    defect.DefectId = ((DefectPositionView)imageButton.BindingContext).xID;
                    if (dataBtn == null || dataBtn == "")
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("NoWorkersToFix"), "Ok");
                    }
                    else
                    {
                        if (dataBtn == "P")
                        {
                            await DisplayAlert(this.Translate("Msg"), this.Translate("worker is fixing"), "Ok");
                        }
                        else
                        {
                            var json = JsonConvert.SerializeObject(defect);
                            var data = new StringContent(json, Encoding.UTF8, "application/json");
                            HttpClient client = new HttpClient();
                            var response = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/SaveDefectProcess", data);
                            if (response.IsSuccessStatusCode)
                            {
                                showDefectList(Commons.GlobalDefines.QcQrCode);
                            }
                            else
                            {
                                await DisplayAlert(this.Translate("Msg"), this.Translate("Return failed")
                                    , "Ok");
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            
            if (sender is Frame frame)
            {
                var action = await DisplayAlert(this.Translate("Msg"), this.Translate("YesToConfirm"), "Yes", "No");
                if (action)
                {
                    var xID = (DefectPositionView)frame.BindingContext;
                    try
                    {
                        HttpClient client = new HttpClient();
                        var response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/UpdatePartStatus?defectId=" + xID.xID);
                        showDefectList(Commons.GlobalDefines.QcQrCode);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                
            }
            
        }



        //Image 1
        //void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        //{
        //    switch (args.Type)
        //    {
        //        case TouchActionType.Pressed:
        //            if (!inProgressPaths.ContainsKey(args.Id))
        //            {
        //                Task.Delay(500);
        //                SKPath path = new SKPath();

        //                path.AddCircle(ConvertToPixel(args.Location).X, ConvertToPixel(args.Location).Y, 5);
        //                inProgressPaths.Add(args.Id, path);
        //                canvasView.InvalidateSurface();
        //            }
        //            break;

        //        case TouchActionType.Moved:

        //            break;

        //        case TouchActionType.Released:
        //            if (inProgressPaths.ContainsKey(args.Id))
        //            {
        //                completedPaths.Add(inProgressPaths[args.Id]);
        //                inProgressPaths.Remove(args.Id);
        //                canvasView.InvalidateSurface();
        //            }
        //            break;

        //        case TouchActionType.Cancelled:
        //            if (inProgressPaths.ContainsKey(args.Id))
        //            {
        //                inProgressPaths.Remove(args.Id);
        //                canvasView.InvalidateSurface();
        //            }
        //            break;
        //        case TouchActionType.Entered:
        //            break;
        //        case TouchActionType.Exited:
        //            break;

        //    }
        //}

        //void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        //{
        //    SKCanvas canvas = args.Surface.Canvas;
        //    canvas.Clear();

        //    foreach (SKPath path in completedPaths)
        //    {
        //        canvas.DrawPath(path, paint);
        //    }

        //    foreach (SKPath path in inProgressPaths.Values)
        //    {
        //        canvas.DrawPath(path, paint);
        //    }
        //}
        //Image 2
        //void OnTouchEffectAction2(object sender, TouchActionEventArgs args)
        //{
        //    switch (args.Type)
        //    {
        //        case TouchActionType.Pressed:
        //            if (!inProgressPaths2.ContainsKey(2))
        //            {
        //                SKPath path = new SKPath();

        //                path.AddCircle(ConvertToPixel(args.Location).X, ConvertToPixel(args.Location).Y, 5);
        //                inProgressPaths2.Add(2, path);
        //                canvasView2.InvalidateSurface();

        //            }
        //            break;

        //        case TouchActionType.Moved:
        //            if (inProgressPaths2.ContainsKey(2))
        //            {

        //            }
        //            break;

        //        case TouchActionType.Released:
        //            if (inProgressPaths2.ContainsKey(2))
        //            {
        //                completedPaths2.Add(inProgressPaths2[2]);
        //                inProgressPaths2.Remove(2);
        //                canvasView2.InvalidateSurface();
        //            }
        //            break;

        //        case TouchActionType.Cancelled:
        //            if (inProgressPaths2.ContainsKey(2))
        //            {
        //                inProgressPaths2.Remove(2);
        //                canvasView2.InvalidateSurface();
        //            }
        //            break;
        //    }
        //}

        //void OnCanvasViewPaintSurface2(object sender, SKPaintSurfaceEventArgs args)
        //{
        //    SKCanvas canvas = args.Surface.Canvas;
        //    canvas.Clear();

        //    foreach (SKPath path in completedPaths2)
        //    {
        //        canvas.DrawPath(path, paint2);
        //    }

        //    foreach (SKPath path in inProgressPaths2.Values)
        //    {
        //        canvas.DrawPath(path, paint2);
        //    }
        //}
        //SKPoint ConvertToPixel(Point pt)
        //{
        //    return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
        //                       (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        //}

        //get defect info
    }
}