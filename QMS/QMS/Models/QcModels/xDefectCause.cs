using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models
{
    public class xDefectCause
    {
        //lỗi áo
        public int xID { get; set; }
        public int ParentID { get; set; }
        public string DefineCode  { get; set; }
        public string enUS { get; set; }
        public string zhTW { get; set; }
        public string viVN { get; set; }
        public string zhCN { get; set; }
        public string DefineColor { get; set; }
        public string Status { get; set; }
        public string Site { get; set; }
        public DateTime? ChangeDated { get; set; }
        public DateTime? CreateDated { get; set; }
        public string Operator { get; set; }
    }
}
