using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class xPBSWorkSewingLineMain
    {
        public string ID { get; set; }
        public string WorkNo { get; set; }
        public string PlanNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Worker { get; set; }
        public string EFF { get; set; }
        public string SMV { get; set; }
        public string Qty { get; set; }
        public string SewingLine { get; set; }
        public string ProdType { get; set; }
        public string SemiType { get; set; }
        public string ESTDate { get; set; }
        public string Status { get; set; }
        public string Layer { get; set; }
        public string Remark { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyDate { get; set; }
    }
}
