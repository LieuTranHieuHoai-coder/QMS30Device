using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class QcDefectImgStatus
    {
        public List<QcImageModel> data { get; set; }
        public string method { get; set; }
        public string msg { get; set; }
        public string rescode { get; set; }
    }
}
