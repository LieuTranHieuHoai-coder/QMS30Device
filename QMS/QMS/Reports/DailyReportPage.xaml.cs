using Newtonsoft.Json;
using QMS.Languages;
using QMS.Models.QcModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyReportPage : ContentPage
    {
        public DailyReportPage()
        {
            InitializeComponent();
            SFactory = Commons.GlobalDefines.WorkingFactory;
            SLine = null;          
        }        
        public string SFactory { get; set; }
        public string SLine { get; set; }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await LoadFactory();
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async Task LoadFactory()
        {
            try
            {
                var fdate = DPFrom.Date;
                var tdate = DPTo.Date;
                using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                {
                    var api = $"/qc/master/GetFactories/{Commons.GlobalDefines.WorkingSite}";
                    
                    var response = await client.GetAsync(api);
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrWhiteSpace(content))
                        {
                            var fts = JsonConvert.DeserializeObject<List<Models.Common.xFactory>>(content);
                            if (fts != null && fts.Count > 0)
                            {
                                PickerFactory.ItemsSource = fts;
                                for (int i = 0; i < fts.Count; i++)
                                {
                                    if (fts[i].ProductionWorkShop.Equals(SFactory))
                                    {
                                        PickerFactory.SelectedIndex = i;
                                    }
                                }
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
        private async void ButtonClose_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void PickerFactory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (PickerFactory.SelectedIndex > -1)
                {
                    var selected = PickerFactory.SelectedItem as Models.Common.xFactory;
                    if (selected != null)
                    {
                        SFactory = selected.ProductionWorkShop;
                        using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                        {
                            var api = $"/qc/master/GetSewLines?site={Commons.GlobalDefines.WorkingSite}&fty={selected.ProductionWorkShop}";
                            var response = await client.GetAsync(api);
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                if (!string.IsNullOrWhiteSpace(content))
                                {
                                    var lines = JsonConvert.DeserializeObject<List<Models.Common.xFactory>>(content);
                                    if (lines != null && lines.Count > 0)
                                    {
                                        PickerLine.ItemsSource = lines;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void PickerLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (PickerLine.SelectedIndex > -1)
                {
                    var selected = PickerLine.SelectedItem as Models.Common.xFactory;
                    if (selected != null)
                    {
                        SLine = selected.SewingLine;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }

        private async void ButtonQuery_Clicked(object sender, EventArgs e)
        {
            try
            {
                DataViewer.ItemsSource = null;
                ButtonQuery.Text = $"{AppResource.Loading}...";
                ButtonQuery.IsEnabled = false;
                var fdate = DPFrom.Date;
                var tdate = DPTo.Date;
                if (fdate <= tdate)
                {
                    using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                    {
                        var api = $"/qc/report/DailyReport?fd={fdate:yyyy-MM-dd}&td={tdate:yyyy-MM-dd}&site={Commons.GlobalDefines.WorkingSite}&fty={SFactory}&line={SLine}";
                        var response = await client.GetAsync(api.Replace(" ", "%20"));
                       
                        if (response.IsSuccessStatusCode)
                        {
                            var apiQty = $"/qc/report/GetQcScanQty?fromdate={fdate:yyyy-MM-dd}&todate={tdate:yyyy-MM-dd}&sew={SLine}&floor={SFactory}";
                            var responseQty = await client.GetAsync(apiQty);
                            var contentQty = await responseQty.Content.ReadAsStringAsync();
                            var dataQty = JsonConvert.DeserializeObject<List<ViewModels.QcScanQty>>(contentQty);
                            //QtyScan.Text = dataQty[0].QcScan;

                            var content = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                var data = JsonConvert.DeserializeObject<List<ViewModels.DailyReportView>>(content);
                                for (int i = 0; i < data.Count; i++)
                                {
                                    if (i % 2 > 0)
                                    {
                                        data[i].BgColor = "#F6E1C3";
                                    }
                                    else
                                    {
                                        data[i].BgColor = "White";
                                    }
                                }
                                DataViewer.ItemsSource = data;
                                var totalCheck = data.Sum(x => x.Qty);
                                var totalPass = data.Sum(x => x.PassQty);
                                var totalDefect = data.Sum(x => x.DefectQty);
                                LabelTotalQty.Text = totalCheck.ToString();
                                LabelTotalPass.Text = totalPass.ToString();
                                LabelTotalDefect.Text = totalDefect.ToString();
                                LabelPercent.Text = ((double.Parse(totalPass.ToString()) / double.Parse(totalCheck.ToString())) * 100).ToString("0.00") + "%";
                                LabelDefectPercent.Text = ((double.Parse(totalDefect.ToString()) / double.Parse(totalCheck.ToString())) * 100).ToString("0.00") + "%";
                            }
                        }
                    }
                }
                else
                {
                    await DisplayAlert(AppResource.Warning, AppResource.DateRangeInvalid, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
            finally
            {
                ButtonQuery.Text = AppResource.Query;
                ButtonQuery.IsEnabled = true;
            }
        }

        [Obsolete]
        private void ButtonDefectCode_Clicked(object sender, EventArgs e)
        {
           
            if (SLine!=null)
            {
                PopupNavigation.PushAsync(new QC_View.PopupTotalDefect(DPFrom.Date.ToString("yyyy-MM-dd"), DPTo.Date.ToString("yyyy-MM-dd"), SLine));
            }
            else
            {
                DisplayAlert("Thông báo", "Chưa chọn chuyền may", "Ok");
            }
        }

        private void ButtonDefectDetail_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new QC_View.ListPassOrDefect(DPFrom.Date, DPTo.Date, SLine));
        }
    }
}