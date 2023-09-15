using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models
{
    class xDefectPart
    {
        //inside+ outside: mặt ngoài, trong của áo
        public int xID { get; set; }
        public string DefineCode { get; set; }
        public string PartName { get; set; }
        public string enUS { get; set; }
        public string zhTW { get; set; }
        public string viVN { get; set; }
        public string zhCN { get; set; }
        public string Site { get; set; }
    }
}
