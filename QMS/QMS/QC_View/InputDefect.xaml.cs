using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using QMS.Languages;
using QMS.Models;
using QMS.Models.QcModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InputDefect : ContentPage
    {
        public List<ViewModels.xPBSOrderManuMainView> info = new List<ViewModels.xPBSOrderManuMainView>();
        public InputDefect()
        {
            InitializeComponent();
                
        }
        protected override void OnAppearing()
        {
            getSewing();
            //getdata();
            base.OnAppearing();
        }
        private void getDefect_Clicked(object sender, EventArgs e)
        {
            getDefect.IsEnabled = false;
            passDefect.IsEnabled = false;
            Navigation.PushModalAsync(new ZXingScanQR());
        }

        private async void getdata()
        {
            try
            {
                HttpClient client = new HttpClient();

                //Get workno
                string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/buyplan/GetStyleBySewingLine?line=" + Commons.GlobalDefines.WorkingLine;
                HttpResponseMessage message = await client.GetAsync(apiUrl);
                if (message.IsSuccessStatusCode)
                {
                    string result = await message.Content.ReadAsStringAsync();
                   
                    info = new List<ViewModels.xPBSOrderManuMainView>(JsonConvert.DeserializeObject<List<ViewModels.xPBSOrderManuMainView>>(result));
                    for (int i = 0; i < info.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(info[i].Style))
                        {
                            var temp = info[i].Style;
                            if (info[i].Style.Contains("-"))
                            {
                                temp = info[i].Style.Split(new string[] { "-" }, StringSplitOptions.None)[0];
                            }
                            var sl = temp.Length;
                            if (sl > 8)
                            {
                                temp = temp.Substring(sl - 8, 8);
                            }
                            info[i].Style = temp;
                        }
                    }
                    PkStyle.ItemsSource = info;
                    //PkStyle.BindingContext = info;
                    PkStyle.SelectedIndex = 0;
                }
            }
            catch (Exception)
            {

                throw;
            }
            //HttpClient client = new HttpClient();

            ////Get workno
            //string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/buyplan/GetStyleBySewingLine?line=" + PkSewingline.Items[PkSewingline.SelectedIndex].ToString();
            //HttpResponseMessage message = await client.GetAsync(apiUrl);
            //if (message.IsSuccessStatusCode)
            //{
            //    string result = await message.Content.ReadAsStringAsync();
            //    info = new List<ViewModels.xPBSOrderManuMainView>(JsonConvert.DeserializeObject<List<ViewModels.xPBSOrderManuMainView>>(result));
            //    PkStyle.ItemsSource = info;
            //    PkStyle.SelectedIndex = 0;
            //}
            
        }

        private void generateSize_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                StyleName.Text = "";
                
                Commons.GlobalDefines.ColorName = button.CommandParameter.ToString();
                listSize.IsVisible = true;
                listSize.BindingContext = new ButtonSizeView();
                StyleName.Text = button.CommandParameter.ToString();
                button.TextColor = Color.White;
                button.BackgroundColor = Color.Green;
                
            }
        }

       private async void getSewing()
        {
            try
            {
                //Get Sewingline

                HttpClient client = new HttpClient();
                string apiSew = Commons.GlobalDefines.NewApiUrl + "/qc/master/GetSewLines?site=" + Commons.GlobalDefines.WorkingSite + "&fty=" + Commons.GlobalDefines.WorkingFactory;
                HttpResponseMessage messageSew = await client.GetAsync(apiSew);
                string resultSew = await messageSew.Content.ReadAsStringAsync();
                List<xFactory> sewline = new List<xFactory>(JsonConvert.DeserializeObject<List<xFactory>>(resultSew));
                PkSewingline.ItemsSource = sewline;
                for (int i = 0; i < sewline.Count; i++)
                {
                    if (sewline[i].SewingLine == Commons.GlobalDefines.WorkingLine)
                    {
                        PkSewingline.SelectedIndex = i;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void PkSewingline_SelectedIndexChanged(object sender, EventArgs e)
        {
            Commons.GlobalDefines.WorkingLine = PkSewingline.Items[PkSewingline.SelectedIndex].ToString();
            getdata();
            
        }

        private void getSize_Clicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                Commons.GlobalDefines.SizeName = button.CommandParameter.ToString();
                StyleName.Text = Commons.GlobalDefines.ColorName + " + " + button.CommandParameter.ToString();
                getDefect.IsEnabled = true;
                passDefect.IsEnabled = true;
                button.TextColor = Color.White;
                button.BackgroundColor = Color.Green;
                scannedQrcode.IsEnabled = true;
            }
            
        }

        private async void scannedQrcode_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ZXingScanQR());
        }

        private async void passDefect_Clicked(object sender, EventArgs e)
        {
            try
            {
                passDefect.IsEnabled = false;
                getDefect.IsEnabled = false;
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetProductionProcess/" + DateTime.Now.ToString("yyMMddHHmmssff")); // Get info Qrcode ProductionProcessQvn
                var response_exist = await client.GetAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetDefectPositions/" + DateTime.Now.ToString("yyMMddHHmmssff")); // Check defect in Process
                var resultAPI = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<xProductionProcess>(resultAPI);
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
                        OrderNo = Commons.GlobalDefines.QcOrderNo,
                        SewingLine = PkSewingline.Items[PkSewingline.SelectedIndex].ToString(),
                        StyleID = 0,
                        Pass = 1,
                        Style = Commons.GlobalDefines.Style,
                        ColorName = Commons.GlobalDefines.ColorName,
                        SizeName = Commons.GlobalDefines.SizeName,
                        Qrcode = DateTime.Now.ToString("yyMMddHHmmssff").ToString(),
                        CreateUser = Commons.GlobalDefines.LoggedUser.Uid,
                        BundleUser = Commons.GlobalDefines.LoggedUser.Uid
                    };
                    var json = JsonConvert.SerializeObject(process);
                    var apiData = new StringContent(json, Encoding.UTF8, "application/json");
                    var newRow = await client.PostAsync(Commons.GlobalDefines.NewApiUrl + "/qc/defect/InsertxProductionProcess", apiData);
                    if (newRow.IsSuccessStatusCode)
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("AddNewSuccess"), "OK");
                        Commons.GlobalDefines.WorkingLine = PkSewingline.Items[PkSewingline.SelectedIndex].ToString();
                        listSize.IsVisible = false;
                    }
                    else
                    {
                        await DisplayAlert(this.Translate("Msg"), this.Translate("QRNOTAVAILABLE"), "OK");
                    }
                    
                }
                else
                {
                    await DisplayAlert(this.Translate("Msg"), this.Translate("QRNOTAVAILABLE"), "OK");
                }    
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void back_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PopupFunction());
        }

        private void PkStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PkStyle.SelectedIndex >= 0)
            {
                Commons.GlobalDefines.Planno = info[PkStyle.SelectedIndex].PlanNo;
                Commons.GlobalDefines.Style = info[PkStyle.SelectedIndex].Style;
                Commons.GlobalDefines.QcOrderNo = info[PkStyle.SelectedIndex].OrderNo;
                customer.Text = info[PkStyle.SelectedIndex].Customer;
                buyMonth.Text = info[PkStyle.SelectedIndex].BuyMonth;
                Commons.GlobalDefines.Customer = customer.Text;
                Commons.GlobalDefines.BuyMonth = buyMonth.Text;
                Commons.GlobalDefines.Season = info[PkStyle.SelectedIndex].Season;
                listColor.BindingContext = new ButtonColorView();
                title.Text = Commons.GlobalDefines.LoggedUser.Fullname + " " + Commons.GlobalDefines.WorkingLine;
                SeasonName.Text = Commons.GlobalDefines.Season;
            }
        }

       
    }
}