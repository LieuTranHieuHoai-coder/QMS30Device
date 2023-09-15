using QMS.Commons;
using QMS.Models;
using QMS.Models.PermitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using QMS.Languages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingQR : ContentPage
    {
        public ScanData scan = new ScanData();
        public List<Site> siteList { get; set; }
        public List<Site> factoryList { get; set; }
        public List<SewingLine> sewingsList { get; set; }
        public List<PermitModels> permitsList { get; set; }
        public string site = "";
        public string sitefactory = "";
        public string sewingline = "";
        string apiUrl = "http://qvnqms.qve.com.vn/chooseplace.php";
        public SettingQR()
        {
            InitializeComponent();
                ShowLocations();
                ShowFactories();
                ShowSewingline();
                ShowPermits();
        }
        // API 
        public async Task<List<Factory>> GetLocationsAsync()
        {
            Factory temp = new Factory();
            List<Factory> f = new List<Factory>();

            var client = new HttpClient(new HttpClientHandler());
            var content = new FormUrlEncodedContent(new[]{
                new KeyValuePair<string,string>("key","FactoryName"),
            });
            var response = await client.PostAsync(apiUrl, content);
            var res = response.Content.ReadAsStringAsync();
            string splitResult = res.Result.ToString();
            string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
            for (int i = 0; i < arrFactory.Length; i++)
            {
                if (!arrFactory[i].Equals(""))
                {
                    temp.FactoryName = arrFactory[i];
                    temp.ProductionWorkShop = "";
                    f.Add(new Factory()
                    {
                        FactoryName = temp.FactoryName,
                        ProductionWorkShop = temp.ProductionWorkShop
                    });
                }
            }
            f.RemoveAt(0);
            return f;
        }
        public async Task<List<Factory>> GetFactoriesAsync(string site)
        {

            Factory temp = new Factory();
            List<Factory> f = new List<Factory>();

            var client = new HttpClient(new HttpClientHandler());
            if (site != null && !site.Equals(""))
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","ProductionWorkShop"),
                    new KeyValuePair<string,string>("site",site)
                });
                var response = await client.PostAsync(apiUrl, content);
                var res = response.Content.ReadAsStringAsync();
                string splitResult = res.Result.ToString();
                string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                for (int i = 0; i < arrFactory.Length; i++)
                {
                    if (!arrFactory[i].Equals(""))
                    {
                        temp.FactoryName = arrFactory[i];
                        temp.ProductionWorkShop = "";
                        f.Add(new Factory()
                        {
                            FactoryName = temp.FactoryName,
                            ProductionWorkShop = temp.ProductionWorkShop
                        });
                    }
                }
                f.RemoveAt(0);
            }
            else
            {
                PkFactory.IsEnabled = false;
            }

            return f;
        }
        public async Task<List<SewingLine>> GetSewingLineAsync(string site, string factory)
        {

            SewingLine temp = new SewingLine();
            List<SewingLine> f = new List<SewingLine>();

            var client = new HttpClient(new HttpClientHandler());
            if (site != null && !site.Equals("") && factory != null && !factory.Equals(""))
            {
                var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","SewingLine"),
                    new KeyValuePair<string,string>("site",site),
                    new KeyValuePair<string,string>("factory",factory)
                });
                var response = await client.PostAsync(apiUrl, content);
                var res = response.Content.ReadAsStringAsync();
                string splitResult = res.Result.ToString();
                string[] arrSewing = splitResult.Split(',', '"', '\n', '[', ']');
                for (int i = 0; i < arrSewing.Length; i++)
                {
                    if (!arrSewing[i].Equals(""))
                    {
                        temp.Line = arrSewing[i];

                        f.Add(new SewingLine()
                        {
                            Line = temp.Line
                        });
                    }
                }
                f.RemoveAt(0);
            }
            else
            {
                PkSewingline.IsEnabled = false;
            }

            return f;
        }


        //Show content
        public async void ShowLocations()
        {
            PkSite.ItemsSource = await GetLocationsAsync();
        }
        public async void ShowFactories()
        {
            PkFactory.ItemsSource = await GetFactoriesAsync(sitefactory);
        }
        public async void ShowSewingline()
        {
            PkSewingline.ItemsSource = await GetSewingLineAsync(sitefactory, sewingline);
        }
        public void ShowPermits()
        {

            // Admin
            if (GlobalDefines.PermTP.Equals("101000000000") && GlobalDefines.PermBTP.Equals("101000000000")
                && GlobalDefines.PermPacking.Equals("101000000000") && GlobalDefines.PermQC.Equals("101000000000"))
            {
                List<PermitModels> temp = new List<PermitModels>();
                temp.Add(new PermitModels() { perm = "Thành Phẩm (FinishGood)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "QC (QC Receiving)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Check QRCode (QC)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Bán Thành Phẩm (SemiGood)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Bao Bì (Packing)", level = "", name = "", path = "" });
                //temp.Add(new PermitModels() { perm = "DS Công Việc QC (QC Personal WorkList)", level = "", name = "", path = "" });
                //  temp.Add(new PermitModels() { perm = "Lỗi Mặc Định (Defect Distribution)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Báo Cáo Hằng Ngày(Daily Production Reports)", level = "", name = "DailyReport", path = "" });

                //pkfunction.ItemsSource = temp;
            }
            // Finish Good, QC
            if (GlobalDefines.PermTP.Equals("101000000000") && !GlobalDefines.PermBTP.Equals("101000000000")
                && !GlobalDefines.PermPacking.Equals("101000000000") && GlobalDefines.PermQC.Equals("101000000000"))
            {
                List<PermitModels> temp = new List<PermitModels>();
                temp.Add(new PermitModels() { perm = "Thành Phẩm (FinishGood)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "QC (QC Receiving)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Check QRCode (QC)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Công Việc Cá Nhân(QC Personal WorkList)", level = "", name = "", path = "" });
                //temp.Add(new PermitModels() { perm = "DS Công Việc QC(QC WorkList)", level = "", name = "", path = "" });
                // temp.Add(new PermitModels() { perm = "Lỗi Mặc Định (Defect Distribution)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Báo Cáo Hằng Ngày(Daily Production Reports)", level = "", name = "DailyReport", path = "" });
                //pkfunction.ItemsSource = temp;
            }
            // Semi Good
            if (GlobalDefines.PermTP.Equals("101000000000") && GlobalDefines.PermBTP.Equals("101000000000")
                && !GlobalDefines.PermPacking.Equals("101000000000") && GlobalDefines.PermQC.Equals("101000000000"))
            {
                List<PermitModels> temp = new List<PermitModels>();
                temp.Add(new PermitModels() { perm = "Bán Thành Phẩm (SemiGood)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Thành Phẩm (FinishGood)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "QC (QC Receiving)", level = "", name = "", path = "" });
                // temp.Add(new PermitModels() { perm = "DS Công Việc QC(QC WorkList)", level = "", name = "", path = "" });
                // temp.Add(new PermitModels() { perm = "Lỗi Mặc Định (Defect Distribution)", level = "", name = "", path = "" });
                temp.Add(new PermitModels() { perm = "Báo Cáo Hằng Ngày(Daily Production Reports)", level = "", name = "DailyReport", path = "" });
                //pkfunction.ItemsSource = temp;
            }
            // Packing
            if (!GlobalDefines.PermTP.Equals("101000000000") && GlobalDefines.PermBTP.Equals("101000000000")
                && !GlobalDefines.PermPacking.Equals("101000000000") && !GlobalDefines.PermQC.Equals("101000000000"))
            {
                List<PermitModels> temp = new List<PermitModels>();
                temp.Add(new PermitModels() { perm = "Bán Thành Phẩm (SemiGood)", level = "", name = "", path = "" });

                //pkfunction.ItemsSource = temp;
            }
        }
        // Event
        //private async void Btnlogout_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PopModalAsync();
        //}

        private async void btnconfirm_Clicked(object sender, EventArgs e)
        {
            try
            {
                //if (pkfunction.SelectedItem == null)
                //{
                //    await DisplayAlert("Thông Báo", "Vui Lòng Chọn Chức Năng", "OK");
                //}
                //else
                //{
                if (PkSite.SelectedIndex > -1 && PkFactory.SelectedIndex > -1 && PkSewingline.SelectedIndex > -1)
                {
                    Commons.GlobalDefines.WorkingSite = PkSite.Items[PkSite.SelectedIndex].ToString();
                    Commons.GlobalDefines.WorkingFactory = PkFactory.Items[PkFactory.SelectedIndex].ToString();
                    Commons.GlobalDefines.WorkingLine = PkSewingline.Items[PkSewingline.SelectedIndex].ToString();
                    //Commons.GlobalDefines.SlectedPerm = pkfunction.Items[pkfunction.SelectedIndex].ToString();
                    //Luan --->
                    //var selectFunc = pkfunction.SelectedItem as PermitModels;
                    //if (!string.IsNullOrEmpty(selectFunc.name))
                    //{
                    //    if (selectFunc.name.Equals("DailyReport"))
                    //    {
                    //        await Navigation.PushModalAsync(new Reports.DailyReportPage());
                    //        return;
                    //    }
                    //}
                    //<---Luan
                    //if (pkfunction.SelectedIndex == 5)
                    //{
                    //    await Navigation.PushModalAsync(new QC_View.DateTimePicker());
                    //}
                    //if (pkfunction.SelectedIndex == 6)
                    //{
                    //    await Navigation.PushModalAsync(new QC_View.DateTimePicker());
                    //}

                    //else
                    //{
                    // QC Check Code - QC Recevice 
                    //if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("check qrcode (qc)") ||
                    //    Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("qc (qc receiving)"))
                    //{

                    //    await Navigation.PushModalAsync(new ZXingScanQR() { });
                    //}
                    //else
                    //{
                    //    await Navigation.PushModalAsync(new ScanQR());
                    //}
                    //}

                    await Navigation.PushModalAsync(new QC_View.MainMenu());
                }
                else
                {
                    //await DisplayAlert("Thông Báo", "Vui lòng nhập thông tin", "Ok");
                    await DisplayAlert(this.Translate("Msg"), this.Translate("INPUTINFORMATION"), "OK");
                }
                //    }

            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "Close");
            }
        }

        [Obsolete]
        private async void imgclose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
        private void pkSite_SelectedIndexChanged(object sender, EventArgs e)
        {

            string sitefff = PkSite.Items[PkSite.SelectedIndex].ToString();

            Task.Delay(200)
            .ContinueWith(async t =>
            {
                Factory temp = new Factory();
                List<Factory> f = new List<Factory>();

                var client = new HttpClient(new HttpClientHandler());
                if (sitefff != null)
                {
                    var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","ProductionWorkShop"),
                    new KeyValuePair<string,string>("site",sitefff)
                });
                    var response = await client.PostAsync(apiUrl, content);
                    var res = response.Content.ReadAsStringAsync();
                    string splitResult = res.Result.ToString();
                    string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                    for (int i = 0; i < arrFactory.Length; i++)
                    {
                        if (!arrFactory[i].Equals(""))
                        {
                            temp.FactoryName = arrFactory[i];
                            temp.ProductionWorkShop = "";
                            f.Add(new Factory()
                            {
                                FactoryName = temp.FactoryName,
                                ProductionWorkShop = temp.ProductionWorkShop
                            });
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    f.RemoveAt(0);

                    PkFactory.ItemsSource = f;

                    PkFactory.IsEnabled = true;
                });
            });
            site = sitefff;
        }
        private void pkFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sitefff = PkSite.Items[PkSite.SelectedIndex].ToString();
            if (PkFactory.SelectedIndex == -1)
            {
                PkSewingline.SelectedItem = null;
                PkSewingline.IsEnabled = false;
                return;
            }
            string factory = PkFactory.Items[PkFactory.SelectedIndex].ToString();

            Task.Delay(200)
            .ContinueWith(async t =>
            {
                SewingLine temp = new SewingLine();
                List<SewingLine> f = new List<SewingLine>();

                var client = new HttpClient(new HttpClientHandler());
                if (sitefff != null && !sitefff.Equals("") && factory != null && !factory.Equals(""))
                {
                    var content = new FormUrlEncodedContent(new[]{
                    new KeyValuePair<string,string>("key","SewingLine"),
                    new KeyValuePair<string,string>("site",sitefff),
                    new KeyValuePair<string,string>("factory",factory)
                });
                    var response = await client.PostAsync(apiUrl, content);
                    var res = response.Content.ReadAsStringAsync();
                    string splitResult = res.Result.ToString();
                    string[] arrFactory = splitResult.Split(',', '"', '\n', '[', ']');
                    for (int i = 0; i < arrFactory.Length; i++)
                    {
                        if (!arrFactory[i].Equals(""))
                        {
                            temp.Line = arrFactory[i];

                            f.Add(new SewingLine()
                            {
                                Line = temp.Line
                            });
                        }
                    }
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    f.RemoveAt(0);

                    PkSewingline.ItemsSource = f;

                    PkSewingline.IsEnabled = true;
                });
            });
        }
        //private void pkfunction_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
    }
}