using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class DetailQcQrCode
    {
        public string Id { get; set; }
        public string Pcode { get; set; }
        public string UId { get; set; }
        public string Status { get; set; }
        public string QrCode { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }

    }
}
