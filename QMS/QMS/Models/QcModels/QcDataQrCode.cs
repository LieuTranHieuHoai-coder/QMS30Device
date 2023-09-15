using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using QMS.Models.QcModels;

namespace QMS.Models.QcModels
{
    public class QcDataQrCode
    {
        public ObservableCollection<QrCodeModel> data { get; set; }
        public string method { get; set; }
        public string msg { get; set; }
        public string rescode { get; set; }
    }
}
