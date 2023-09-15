using System;

namespace QMS.Models.QcModels
{
    public class UserWorkPlace
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public string WorkingSite { get; set; }
        public string WorkingFact { get; set; }
        public string WorkingLine { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
