using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Models.QcModels
{
    public class ProductionDefectInfoView : INotifyPropertyChanged
    {
        ObservableCollection<xOrderQRCode> _btns;
        public ProductionDefectInfoView(string qr)
        {
            getData(qr);
        }
       
        public ObservableCollection<xOrderQRCode> returns
        {
            get { return _btns; }
            set
            {
                if (value != null)
                {
                    _btns = value;
                    OnPropertyChanged();
                }
            }
        }
        private async void getData(string qr)
        {
            try
            {
                string apiUrl = "http://192.168.1.46:9999/qc/qr/GetQrInfo/" + qr;
                HttpClient client = new HttpClient();
                HttpResponseMessage message = await client.GetAsync(apiUrl);
                string result = await message.Content.ReadAsStringAsync();

                if (message.IsSuccessStatusCode)
                {
                    var data = JsonConvert.DeserializeObject<xOrderQRCode>(result);

                    ObservableCollection<xOrderQRCode> xOrders = new ObservableCollection<xOrderQRCode>();
                    //if (data.BundleStatus == "1")
                    //{
                    //    data.BundleStatus = "Sửa";
                    //}
                    //if (data.BundleStatus == "0")
                    //{
                    //    data.BundleStatus = "Đạt";
                    //}
                    xOrders.Add(data);
                    returns = xOrders;

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Thông Báo",ex.Message.ToString(),"Ok");
            }
            
        }

        private Task DisplayAlert(string v1, object reasonPhrase, string v2)
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
