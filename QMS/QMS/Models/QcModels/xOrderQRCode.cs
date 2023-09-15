using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
	public class xOrderQRCode
	{
		
		public string CodeId { get; set; }
		public string RFIDCode { get; set; }
		public string Factory { get; set; }
		public string Customer { get; set; }
		public string Style { get; set; }
		public string Season { get; set; }
		public string BuyMonth { get; set; }
		public string ShipDest { get; set; }
		public string ColorName { get; set; }
		public string SizeName { get; set; }
		public string PoNo { get; set; }
		public bool IsPrint { get; set; }
		public int PrintQty { get; set; }
		public DateTime PrintDated { get; set; }
		public bool PrintStatus { get; set; }
		public string BundleStatus { get; set; }
		public string WorkNo { get; set; }
		public string BreakDownType { get; set; }
		public string WorkType { get; set; }
		public bool IsRecycle { get; set; }
		public string CreateUser { get; set; }
		public DateTime CreateDated { get; set; }
		public DateTime ChangeDated { get; set; }
		public string RelativeCode { get; set; }
	}
}
