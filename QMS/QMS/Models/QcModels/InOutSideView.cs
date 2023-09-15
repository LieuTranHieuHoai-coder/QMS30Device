using QMS.Models.QcModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace QMS.QC_View
{
    public class InOutSideView : INotifyPropertyChanged
    {
        ObservableCollection<ReturnBtn> _btns;
        public InOutSideView()
        {
            getData();
        }
        public InOutSideView(string status)
        {
            ChangeColorAsync();
        }
        public ObservableCollection<ReturnBtn> returns
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
            ReturnBtn temp = new ReturnBtn();
            string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";
            CookieContainer cookies = new CookieContainer();
            if (Commons.GlobalDefines.Language.Contains("vn"))
            {
                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "vn"));
            }
            else
            {
                if (Commons.GlobalDefines.Language.Contains("cn"))
                {
                    cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                    cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "cn"));
                }
                else
                {
                    if (Commons.GlobalDefines.Language.Contains("tw"))
                    {
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "tw"));
                    }
                    else
                    {
                        if (Commons.GlobalDefines.Language.Contains("en"))
                        {
                            cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", "QVN"));
                            cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "en"));
                        }
                    }

                }

            }

            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            HttpClient client = new HttpClient(handler);

            FormUrlEncodedContent form = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("func","SelectQCMaintainPart"),
            });
            HttpResponseMessage message = await client.PostAsync(apiUrl, form);

            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<List<ReturnBtn>>(result);
                returns = new ObservableCollection<ReturnBtn>();
                string splitResult = result.Result.ToString();
                string[] arrBtn = splitResult.Split('"', '\n', '[', ']', '{', '}', ':');
                for (int i = 10; i < arrBtn.Length; i = i + 20)
                {
                    if (!arrBtn[i].Equals(""))
                    {
                        temp.DefectBtn = arrBtn[i];
                        temp.Color = arrBtn[i + 5];
                        temp.xID = arrBtn[i + 10];
                        //string str = System.Text.Encoding.Unicode.GetString(array);
                        returns.Add(new ReturnBtn()
                        {
                            DefectBtn = System.Uri.UnescapeDataString(System.Text.RegularExpressions.Regex.Unescape(temp.DefectBtn)),
                            Color = temp.Color,
                            xID = temp.xID
                        });
                    }
                }
                
            }

        }

        private async void ChangeColorAsync()
        {
            ReturnBtn temp = new ReturnBtn();
            string apiUrl = "http://qvnqms.qve.com.vn/savedatabase.php";
            CookieContainer cookies = new CookieContainer();
            if (Commons.GlobalDefines.Language.Contains("vn"))
            {
                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", Commons.GlobalDefines.WorkingSite));
                cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "vn"));
            }
            else
            {
                if (Commons.GlobalDefines.Language.Contains("cn"))
                {
                    cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", Commons.GlobalDefines.WorkingSite));
                    cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "cn"));
                }
                else
                {
                    if (Commons.GlobalDefines.Language.Contains("tw"))
                    {
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", Commons.GlobalDefines.WorkingSite));
                        cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "tw"));
                    }
                    else
                    {
                        if (Commons.GlobalDefines.Language.Contains("en"))
                        {
                            cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("SITE", Commons.GlobalDefines.WorkingSite));
                            cookies.Add(new Uri("http://qvnqms.qve.com.vn"), new Cookie("LANGUAGE", "en"));
                        }
                    }
                }
            }
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;

            HttpClient client = new HttpClient(handler);

            FormUrlEncodedContent form = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string,string>("func","SelectQCMaintainPart"),
            });
            HttpResponseMessage message = await client.PostAsync(apiUrl, form);

            if (message.IsSuccessStatusCode)
            {
                var result = message.Content.ReadAsStringAsync();
                //var data = JsonConvert.DeserializeObject<List<ReturnBtn>>(result);
                returns = new ObservableCollection<ReturnBtn>();
                string splitResult = result.Result.ToString();
                string[] arrBtn = splitResult.Split('"', '\n', '[', ']', '{', '}', ':');
                for (int i = 10; i < arrBtn.Length; i = i + 20)
                {
                    if (!arrBtn[i].Equals(""))
                    {
                        temp.DefectBtn = arrBtn[i];
                        temp.Color = "#343a40";
                        temp.xID = arrBtn[i + 10];
                        //string str = System.Text.Encoding.Unicode.GetString(array);
                        returns.Add(new ReturnBtn()
                        {
                            DefectBtn = System.Uri.UnescapeDataString(System.Text.RegularExpressions.Regex.Unescape(temp.DefectBtn)),
                            Color = temp.Color,
                            xID = temp.xID
                        });
                    }
                }

            }
           
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
