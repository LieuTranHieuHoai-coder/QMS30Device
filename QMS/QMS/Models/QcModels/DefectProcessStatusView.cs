using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class DefectProcessStatusView
    {
        public int Id { get; set; }
        public int DefectId { get; set; }
        public DateTime ProcessDate { get; set; }
        public string ProcessPerson { get; set; }
        public string FullName { get; set; }
        public string ProcStatus { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string DefectCauseEN { get; set; } //(text, null)
        public string DefectCauseVN { get; set; } //(text, null)
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
    }
}
