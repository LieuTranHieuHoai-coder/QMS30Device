using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.PackingModels
{
    public class PackingModel
    {
        public string Pcode { get; set; }
        public string WorkNo { get; set; }
        public string OriginalFactory { get; set; }
        public string OperatorFactory { get; set; }
        public string Floor { get; set; }
        public string Customer { get; set; }
        public string BuyMonth { get; set; }
        public string Season { get; set; }
        public string OrderNo { get; set; }
        public string SewingLine { get; set; }
        public string StyleID { get; set; }
        public string Style { get; set; }
        public string POId { get; set; }
        public string PONo { get; set; }
        public string ColorID { get; set; }
        public string ColorName { get; set; }
        public string SizeID { get; set; }
        public string SizeName { get; set; }
        public string ShipDest { get; set; }
        public string SizeBreakDownQty { get; set; }
        public string PoQty { get; set; }
        public string OEMNo { get; set; }
        public string Qrcode { get; set; }
        public string Defect { get; set; }
        public string Pass { get; set; }
        public string Rework { get; set; }
        public string Broken { get; set; }
        public string BreakDownType { get; set; }
        public string ProdType { get; set; }
        public string WorkType { get; set; }
        public string CreateUser { get; set; }
        public string CreateDated { get; set; }
        public string BundleUser { get; set; }
        public string BundleDated { get; set; }
        public string ModifyUser { get; set; }
        public string ModifyDated { get; set; }
        public string FirstDefectUser { get; set; }
        public string FirstDefectDated { get; set; }
        public string QCReceivingDated { get; set; }
        public string QCReceivingUser { get; set; }
        public string bg { get; set; }
        public string tc { get; set; }
        public string rescode { get; set; }

    }
}
