using QMS.Commons;
using QMS.Models;
using QMS.Models.PermitModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using QMS.Languages;
using QMS.Models.QcModels;
using Newtonsoft.Json;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentPage
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
        public Setting()
        {
            InitializeComponent();
            //GetUserWorkPlace();


            //if (Commons.GlobalDefines.WorkingFactory != null)
            //{
            //    PkFactory.SelectedItem = Commons.GlobalDefines.;
            //}
            //if (Commons.GlobalDefines.WorkingLine != null)
            //{
            //    PkSewingline.SelectedItem = 0;
            //}
            //ShowPermits();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await ShowLocations();
                //await ShowFactories();
                //await ShowSewingline();
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        public async void GetUserWorkPlace()
        {
            HttpClient client = new HttpClient();
            var result = await client.GetAsync(GlobalDefines.NewApiUrl + "/qc/master/GetUserWorkPlace/" + GlobalDefines.LoggedUser.Uid);

            if (!result.ReasonPhrase.Contains("Not Found"))
            {
                var response = result.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserWorkPlace>(response.Result);
                Commons.GlobalDefines.WorkingFactory = data.WorkingFact;
                Commons.GlobalDefines.WorkingLine = data.WorkingLine;
                Commons.GlobalDefines.WorkingSite = data.WorkingSite;
            }
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

        public async Task<List<xFactory>> GetSites()
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(GlobalDefines.NewApiUrl) })
                {
                    var api = "/qc/master/GetSites";
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            return JsonConvert.DeserializeObject<List<xFactory>>(content);
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<xFactory>> GetFactories(string site)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(GlobalDefines.NewApiUrl) })
                {
                    var api = "/qc/master/GetFactories/" + site;
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            return JsonConvert.DeserializeObject<List<xFactory>>(content);
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<xFactory>> GetSewLines(string site, string fact)
        {
            try
            {
                using (var client = new HttpClient { BaseAddress = new Uri(GlobalDefines.NewApiUrl) })
                {
                    var api = $"/qc/master/GetSewLines?site={site}&fty={fact}";
                    var response = await client.GetAsync(api);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        if (!string.IsNullOrEmpty(content))
                        {
                            return JsonConvert.DeserializeObject<List<xFactory>>(content);
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
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
        public async Task ShowLocations()
        {
            try
            {
                var sites = await GetSites();
                if (sites != null && sites.Count > 0)
                {
                    PkSite.ItemsSource = sites;
                    if (GlobalDefines.WorkingSite != null)
                    {
                        for (int i = 0; i < sites.Count; i++)
                        {
                            if (sites[i].FactoryName.Equals(GlobalDefines.WorkingSite))
                            {
                                PkSite.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task ShowFactories(xFactory selectedSite)
        {
            try
            {
                //var selectedSite = PkSite.SelectedItem as Site;
                var facts = await GetFactories(selectedSite.FactoryName);
                if (facts != null && facts.Count > 0)
                {
                    PkFactory.ItemsSource = facts;
                    for (int i = 0; i < facts.Count; i++)
                    {
                        if (GlobalDefines.WorkingFactory != null)
                        {
                            if (facts[i].ProductionWorkShop.Equals(GlobalDefines.WorkingFactory))
                            {
                                PkFactory.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task ShowSewingline(xFactory selectedSite, xFactory selectedFact)
        {
            try
            {
                //var selectedSite = PkSite.SelectedItem as Site;
                //var selectedFact = PkFactory.SelectedItem as Factory;
                var sewLines = await GetSewLines(selectedSite.FactoryName, selectedFact.ProductionWorkShop);
                if (sewLines != null && sewLines.Count > 0)
                {
                    PkSewingline.ItemsSource = sewLines;
                    if (GlobalDefines.WorkingLine != null)
                    {
                        for (int i = 0; i < sewLines.Count; i++)
                        {
                            if (sewLines[i].SewingLine.Equals(GlobalDefines.WorkingLine))
                            {
                                PkSewingline.SelectedIndex = i;
                                break;
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public void ShowPermits()
        {
            try
            {
                List<PermitModels> temp = new List<PermitModels>();
                foreach (var per in GlobalDefines.Permisions)
                {
                    if (per.Menu_id == 359)
                    {
                        temp.Add(new PermitModels() { perm = "Thành Phẩm (FinishGood)", level = "", name = "", path = "" });
                    }
                    if (per.Menu_id == 801)
                    {
                        temp.Add(new PermitModels() { perm = "QC (QC Receiving)", level = "", name = "", path = "" });
                    }
                    if (per.Menu_id == 361)
                    {
                        temp.Add(new PermitModels() { perm = "Check QRCode (QC)", level = "", name = "", path = "" });
                    }
                    if (per.Menu_id == 455)
                    {
                        temp.Add(new PermitModels() { perm = "Bán Thành Phẩm (SemiGood)", level = "", name = "", path = "" });
                    }
                    if (per.Menu_id == 368)
                    {
                        temp.Add(new PermitModels() { perm = "Báo Cáo Hằng Ngày(Daily Production Reports)", level = "", name = "DailyReport", path = "" });
                    }
                }
            }
            catch (Exception)
            {

                throw;
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
                if (PkSite.SelectedIndex > -1 && PkFactory.SelectedIndex > -1 && PkSewingline.SelectedIndex > -1)
                {
                    var selectedSite = PkSite.SelectedItem as xFactory;
                    var selectedFactory = PkFactory.SelectedItem as xFactory;
                    var selectedLine = PkSewingline.SelectedItem as xFactory;

                    GlobalDefines.WorkingSite = selectedSite.FactoryName;
                    GlobalDefines.WorkingFactory = selectedFactory.ProductionWorkShop;
                    GlobalDefines.WorkingLine = selectedLine.SewingLine;                   

                    // send api save workplace

                    UserWorkPlace userWorkPlace = new UserWorkPlace
                    {
                        WorkingSite = GlobalDefines.WorkingSite,
                        WorkingFact = GlobalDefines.WorkingFactory,
                        WorkingLine = GlobalDefines.WorkingLine,
                        Uid = GlobalDefines.LoggedUser.Uid
                    };
                    using (var client = new HttpClient { BaseAddress = new Uri(GlobalDefines.NewApiUrl) })
                    {
                        var json = JsonConvert.SerializeObject(userWorkPlace);
                        var data = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(GlobalDefines.NewApiUrl + "/qc/master/SaveWorkPlace", data);
                        if (!response.IsSuccessStatusCode)
                        {
                            await DisplayAlert("SaveWorkPlace", response.ReasonPhrase, "OK");
                        }
                    }

                    // end send api save workplace
                    GlobalDefines.WorkingSite = selectedSite.FactoryName;
                    GlobalDefines.WorkingFactory = selectedFactory.ProductionWorkShop;
                    GlobalDefines.WorkingLine = selectedLine.SewingLine;
                    Application.Current.MainPage = new QC_View.PopupFunction();
                }
                else
                {
                    //await DisplayAlert("Thông Báo", "Vui lòng nhập thông tin", "Ok");
                    await DisplayAlert(this.Translate("Msg"), this.Translate("INPUTINFORMATION"), "OK");
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "Close");
            }
        }

        [Obsolete]
        private void imgclose_Clicked(object sender, EventArgs e)
        {
            GlobalDefines.IndexSite = 0;
            GlobalDefines.IndexFactory = 0;
            GlobalDefines.IndexLine = 0;
            Application.Current.MainPage = new MainPage();
        }
        private async void pkSite_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                var selectedSite = PkSite.SelectedItem as xFactory;
                await ShowFactories(selectedSite);                
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
            }
        }
        private async void pkFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedSite = PkSite.SelectedItem as xFactory;
                var selectedFact = PkFactory.SelectedItem as xFactory;
                await ShowSewingline(selectedSite, selectedFact);
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message + ex.StackTrace, "OK");
            }
        }
        //private void pkfunction_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}
    }
}