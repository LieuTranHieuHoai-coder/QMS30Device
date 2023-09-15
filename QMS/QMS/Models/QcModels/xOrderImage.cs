using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
	public class xOrderImage
	{
		public int xID { get; set; }
		public string Style { get; set; }
		public string WorkType { get; set; }
		public string Place { get; set; }
		public string ImgName { get; set; }
		public string ImgType { get; set; }
		public byte[] ImgBytes { get; set; }
	}
}
