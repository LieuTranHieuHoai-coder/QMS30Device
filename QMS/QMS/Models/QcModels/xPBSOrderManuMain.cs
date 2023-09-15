using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class xPBSOrderManuMain
    {
        public string ID { get; set; }
        public string OrderNo { get; set; }
        public string WorkNo { get; set; }
        public string Customer { get; set; }
        public string Season { get; set; }
        public string BuyMonth { get; set; }
        public string Style { get; set; }
        public string ShippingSite { get; set; }
        public string ManuSite { get; set; }
        public string Factory { get; set; }
        public string OrderQty { get; set; }
        public string WorkOrderQty { get; set; }
        public string ESTDate { get; set; }
        public string Status { get; set; }
        public string POFlag { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyDate { get; set; }
    }
}
