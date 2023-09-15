using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
	public class xDefectImage
	{
		public int xID { get; set; }
		public string QRCode { get; set; }
		public int DPId { get; set; }
		public string ImgType { get; set; }
		public byte[] ImgBytes { get; set; }
		public string CreateUser { get; set; }
		public DateTime CreateDated { get; set; }
	}
}
