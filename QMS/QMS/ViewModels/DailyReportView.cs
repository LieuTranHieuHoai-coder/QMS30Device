using System;

namespace QMS.ViewModels
{
    public class DailyReportView
    {
        public DateTime CreateDated { get; set; } //(date, null)
        public string SewingLine { get; set; } //(text, null)
        public string OperatorFactory { get; set; } //(text, null)
        public string Floor { get; set; } //(text, null)
        public string Customer { get; set; } //(text, null)
        public string BuyMonth { get; set; } //(text, null)
        public string Season { get; set; } //(text, null)
        public string Style { get; set; } //(text, null)
        public string OrderNo { get; set; } //(text, null)
        public string PONo { get; set; } //(text, null)
        public string ColorName { get; set; } //(text, null)
        public string SizeName { get; set; } //(text, null)
        public int Qty { get; set; } //(decimal(0,0), null)
        public int PassQty { get; set; } //(decimal(0,0), null)
        public string BgColor { get; set; }
        public int DefectQty { get; set; } //(decimal(0,0), null)
        public int BrokenQty { get; set; } //(decimal(0,0), null)
    }

}
