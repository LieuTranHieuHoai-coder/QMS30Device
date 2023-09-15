using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QMS.Models.QcModels
{
    public class ButtonColorView : INotifyPropertyChanged
    {
        ObservableCollection<ButtonColorSize> _btns;
        public ButtonColorView()
        {
            getData();
        }
        public ObservableCollection<ButtonColorSize> returns
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
        private async void getData()
        {
            try
            {
                HttpClient client = new HttpClient();

                string apiColor = Commons.GlobalDefines.NewApiUrl + "/qc/buyplan/GetButtonColor?planno=" + Commons.GlobalDefines.Planno;
                HttpResponseMessage messageColor = await client.GetAsync(apiColor);
                if (messageColor.IsSuccessStatusCode)
                {
                    string resultColor = await messageColor.Content.ReadAsStringAsync();
                    ObservableCollection<ButtonColorSize> list = new ObservableCollection<ButtonColorSize>(JsonConvert.DeserializeObject<List<ButtonColorSize>>(resultColor));
                    returns = list;
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
