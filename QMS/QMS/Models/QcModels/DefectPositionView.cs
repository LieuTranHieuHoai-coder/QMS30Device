using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class DefectPositionView
    {
        public int xID { get; set; } //(int, not null)
        public string OperatorFactory { get; set; } //(text, null)
        public string Floor { get; set; } //(text, null)
        public string SewingLine { get; set; } //(text, null)
        public string Qrcode { get; set; } //(text, null)
        public string DefectID { get; set; } //(text, null)
        public string ReasonID { get; set; } //(text, null)
        public string PartID { get; set; } //(text, null)
        public string PartStatus { get; set; } //(text, null)
        public string ImageID { get; set; } //(text, null)
        public double ScaleX { get; set; } //(float, null)
        public double ScaleY { get; set; } //(float, null)
        public string Status { get; set; } //(text, null)
        public string CreateUser { get; set; } //(text, null)
        public DateTime CreateDated { get; set; } //(date, null)
        public string ModifyUser { get; set; } //(text, null)
        public DateTime ModifyDated { get; set; } //(date, null)
        public string DefectCauseEN { get; set; } //(text, null)
        public string DefectCauseVN { get; set; } //(text, null)
        public string DefectCauseTW { get; set; } //(text, null)
        public string DefectCauseCN { get; set; } //(text, null)
        public string DefectCauseColor { get; set; } //(text, null)
        public string DefineCode { get; set; } //(text, null)
        public string ReasonEN { get; set; } //(text, null)
        public string ReasonVN { get; set; } //(text, null)
        public string ReasonCN { get; set; } //(text, null)
        public string ReasonTW { get; set; } //(text, null)
        public string PartNameEN { get; set; } //(text, null)
        public string PartNameVN { get; set; } //(text, null)
        public string PartNameCN { get; set; } //(text, null)
        public string PartNameTW { get; set; } //(text, null)        
        public byte[] ImgBytes { get; set; }
        public int HasProcessed { get; set; }
        public string ProcStatus { get; set; }
        public int Id { get; set; }
        public int DirectID { get; set; }
        public string viVN { get; set; }
        public string enUS { get; set; }
        public string zhCN { get; set; }
        public string zhTW { get; set; }

    }
}
