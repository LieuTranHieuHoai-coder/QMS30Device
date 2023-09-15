using System;

namespace QMS.Models.Common
{
    public class xUser
	{
		public int Id { get; set; }
		public string Source { get; set; }
		public string Uid { get; set; }
		public string Pw { get; set; }
		public string Fullname { get; set; }
		public string Email { get; set; }
		public string Active { get; set; }
		public string Company { get; set; }
		public string Department { get; set; }
		public string Title { get; set; }
		public string Comment { get; set; }
		public DateTime Lastdt { get; set; }
		public string Lastuid { get; set; }

	}
}
