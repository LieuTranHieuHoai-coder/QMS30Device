using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class xProductionProcess
	{
		public int Pcode { get; set; }
		public string WorkNo { get; set; }
		public string OriginalFactory { get; set; }
		public string OperatorFactory { get; set; }
		public string Floor { get; set; }
		public string Customer { get; set; }
		public string BuyMonth { get; set; }
		public string Season { get; set; }
		public string OrderNo { get; set; }
		public string SewingLine { get; set; }
		public int StyleID { get; set; }
		public string Style { get; set; }
		public int POId { get; set; }
		public string PONo { get; set; }
		public int ColorID { get; set; }
		public string ColorName { get; set; }
		public int SizeID { get; set; }
		public string SizeName { get; set; }
		public string ShipDest { get; set; }
		public int SizeBreakDownQty { get; set; }
		public int PoQty { get; set; }
		public string OEMNo { get; set; }
		public string Qrcode { get; set; }
		public byte Defect { get; set; }
		public byte Pass { get; set; }
		public int Rework { get; set; }
		public byte Broken { get; set; }
		public string BreakDownType { get; set; }
		public string ProdType { get; set; }
		public string WorkType { get; set; }
		public string CreateUser { get; set; }
		public DateTime CreateDated { get; set; }
		public string BundleUser { get; set; }
		public DateTime BundleDated { get; set; }
		public string ModifyUser { get; set; }
		public DateTime ModifyDated { get; set; }
		public string FirstDefectUser { get; set; }
		public DateTime FirstDefectDated { get; set; }
		public DateTime QCReceivingDated { get; set; }
		public string QCReceivingUser { get; set; }
	}
}
