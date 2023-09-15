using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Models.QcModels
{
    public class TotalDefectView : INotifyPropertyChanged
    {
        ObservableCollection<ViewModels.QcScanQty> _defect;
        public TotalDefectView()
        {
            getData();
        }
        public TotalDefectView(string fd, string td, string sew)
        {
            getDataParam(fd,td,sew);
        }
        public ObservableCollection<ViewModels.QcScanQty> defect
        {
            get { return _defect; }
            set
            {
                if (value != null)
                {
                    _defect = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void getData()
        {
            try
            {
                HttpClient client = new HttpClient();
                //Get workno
                string apiUrl = Commons.GlobalDefines.NewApiUrl + $"/qc/report/ListDefectDaily?fromdate={Commons.GlobalDefines.RpFromDate:yyyy-MM-dd}&todate={Commons.GlobalDefines.RpToDate:yyyy-MM-dd}&sew=" + Commons.GlobalDefines.RpSew;
                HttpResponseMessage message = await client.GetAsync(apiUrl.Replace(" ","%20"));

                if (message.IsSuccessStatusCode)
                {
                    string result = await message.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<ObservableCollection<ViewModels.QcScanQty>>(result);
                    defect = list;
                }
                else
                {
                    await DisplayAlert("Status", message.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }
        private async void getDataParam(string fd, string td, string sew)
        {
            try
            {
                HttpClient client = new HttpClient();
                //Get workno
                string apiUrl = Commons.GlobalDefines.NewApiUrl + $"/qc/report/ListDefectDaily?fromdate={fd}&todate={td}&sew=" + sew;
                HttpResponseMessage message = await client.GetAsync(apiUrl.Replace(" ", "%20"));

                if (message.IsSuccessStatusCode)
                {
                    string result = await message.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<ObservableCollection<ViewModels.QcScanQty>>(result);
                    defect = list;
                }
                else
                {
                    await DisplayAlert("Status", message.StatusCode.ToString(), "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Source, ex.Message, "OK");
            }
        }
        private Task DisplayAlert(string source, string message, string v)
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
