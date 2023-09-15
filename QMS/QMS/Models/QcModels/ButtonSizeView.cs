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
    public class ButtonSizeView : INotifyPropertyChanged
    {
        ObservableCollection<ButtonColorSize> _sizes;
        public ButtonSizeView()
        {
            getData();
        }
        public ObservableCollection<ButtonColorSize> sizes
        {
            get { return _sizes; }
            set
            {
                if (value != null)
                {
                    _sizes = value;
                    OnPropertyChanged();
                }
            }
        }
        private async void getData()
        {
            try
            {
                HttpClient client = new HttpClient();

                string apiColor = Commons.GlobalDefines.NewApiUrl + "/qc/buyplan/GetButtonSize?planno=" + Commons.GlobalDefines.Planno + "&color=" + Commons.GlobalDefines.ColorName;
                HttpResponseMessage messageColor = await client.GetAsync(apiColor);
                if (messageColor.IsSuccessStatusCode)
                {
                    string resultColor = await messageColor.Content.ReadAsStringAsync();
                    ObservableCollection<ButtonColorSize> list = new ObservableCollection<ButtonColorSize>(JsonConvert.DeserializeObject<List<ButtonColorSize>>(resultColor));
                    sizes = list;
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
