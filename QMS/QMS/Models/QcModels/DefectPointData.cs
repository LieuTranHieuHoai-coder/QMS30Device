using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class DefectPointData
	{
		public int xID { get; set; }
		public string OperatorFactory { get; set; }
		public string Floor { get; set; }
		public string SewingLine { get; set; }
		public string Qrcode { get; set; }
		public string DefectID { get; set; }
		public string ReasonID { get; set; }
		public string PartID { get; set; }
		public string PartStatus { get; set; }
		public string ImageID { get; set; }
		public double ScaleX { get; set; }
		public double ScaleY { get; set; }
		public string ImgType { get; set; }
		public string ImgBytes { get; set; }
		public string Status { get; set; }
		public string CreateUser { get; set; }
		public DateTime CreateDated { get; set; }
		public string ModifyUser { get; set; }
		public DateTime ModifyDated { get; set; }
		public int DirectID { get; set; }
	}
}
