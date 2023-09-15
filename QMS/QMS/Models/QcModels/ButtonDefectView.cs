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
    public class ButtonDefectView : INotifyPropertyChanged
    {
        ObservableCollection<xDefectCause> _defect;
        public ButtonDefectView()
        {
            //getData();
        }
        public ButtonDefectView(int id)
        {
            getDataById(id);
        }
        public ObservableCollection<xDefectCause> defect
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
        
        private async void getDataById(int id)
        {
            try
            {
                HttpClient client = new HttpClient();

                //Get workno
                string apiUrl = Commons.GlobalDefines.NewApiUrl + "/qc/defect/GetParentDefect?site=" + Commons.GlobalDefines.WorkingSite;
                HttpResponseMessage message = await client.GetAsync(apiUrl);

                if (message.IsSuccessStatusCode)
                {
                    string result = await message.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<ObservableCollection<xDefectCause>>(result);
                    _defect = new ObservableCollection<xDefectCause>(list.Where(x => x.ParentID == id));
                    defect = _defect;
                    //if (Commons.GlobalDefines.Language.Contains("vn"))
                    //{
                    //    //Commons.GlobalDefines.ParentName = defect[0].
                    //}
                    //else
                    //{
                    //    if (Commons.GlobalDefines.Language.Contains("tw"))
                    //    {
                            
                    //    }
                    //    else
                    //    {
                    //        if (Commons.GlobalDefines.Language.Contains("en"))
                    //        {
                                
                    //        }
                    //        else
                    //        {

                    //        }
                    //    }

                    //}
                    
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
