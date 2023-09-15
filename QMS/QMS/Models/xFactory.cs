using System;

namespace QMS.Models
{
    public class xFactory
    {
        public int xID { get; set; }
        public string ApsKey { get; set; }
        public string FactoryName { get; set; }
        public string ProductionWorkShop { get; set; }
        public string SewingLine { get; set; }
        public int Worker { get; set; }
        public bool ApsState { get; set; }
        public string Type { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDated { get; set; }
        public string ModifyUser { get; set; }
        public DateTime ModifyDated { get; set; }
    }
}
