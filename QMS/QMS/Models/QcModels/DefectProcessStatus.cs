using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
	public class DefectProcessStatus
	{
		public int Id { get; set; }
		public int DefectId { get; set; }
		public DateTime ProcessDate { get; set; }
		public string ProcessPerson { get; set; }
		public DateTime LastProcessDate { get; set; }
		public string LastProcessPerson { get; set; }
	}
}
