using Newtonsoft.Json;
using QMS.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPassOrDefect : ContentPage
    {
        public ListPassOrDefect()
        {
            InitializeComponent();
        }
        public ListPassOrDefect(DateTime fdate, DateTime tdate, string line)
        {
            InitializeComponent();
            getdata(fdate, tdate, line);
        }
        public async void getdata(DateTime fdate, DateTime tdate, string SLine)
        {
            try
            {
                DataViewer.ItemsSource = null;
               
                if (fdate <= tdate)
                {
                    using (var client = new HttpClient { BaseAddress = new Uri(Commons.GlobalDefines.NewApiUrl) })
                    {
                        var apiQty = $"/qc/report/ListPassOrDefect?fromdate={fdate:yyyy-MM-dd}&todate={tdate:yyyy-MM-dd}&sew={SLine}&floor={SLine}";
                        var response = await client.GetAsync(apiQty);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrWhiteSpace(content))
                            {
                                var data = JsonConvert.DeserializeObject<List<ViewModels.ListPassOrDefect>>(content);
                                
                                DataViewer.ItemsSource = data;
                               
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
            
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}