using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.QcModels
{
    public class DefectProcessPerson: DefectProcessStatus
    {
        public string FullName { get; set; }
        public string ProcStatus { get; set; }
    }
}
