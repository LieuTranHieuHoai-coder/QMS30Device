using System;
using System.Collections.Generic;
using System.Text;

namespace QMS.Models.PermitModels
{
    public class PermitModels
    {
        public string name { set; get; }
        public string level { set; get; }
        public string path { set; get; }
        public string perm { set; get; }
        public PermissionDetailField PermDetails => BundlePermissionDefine(perm);
        public PermissionDetailField BundlePermissionDefine(string perm)
        {
            PermissionDetailField f = new PermissionDetailField();
            if (perm.Length == 12)
            {
                if (perm.Substring(0, 1).Equals("1")) f.Read = true;
                if (perm.Substring(1, 1).Equals("1")) f.Update = true;
                if (perm.Substring(2, 1).Equals("1")) f.Create = true;
                if (perm.Substring(3, 1).Equals("1")) f.Delete = true;
                if (perm.Substring(4, 1).Equals("1")) f.Refresh = true;
                if (perm.Substring(5, 1).Equals("1")) f.Sync = true;
                if (perm.Substring(6, 1).Equals("1")) f.Define1 = true;
                if (perm.Substring(7, 1).Equals("1")) f.Define2 = true;
                if (perm.Substring(8, 1).Equals("1")) f.Define3 = true;
                if (perm.Substring(9, 1).Equals("1")) f.Define4 = true;
                if (perm.Substring(10, 1).Equals("1")) f.Define5 = true;
                if (perm.Substring(11, 1).Equals("1")) f.Define6 = true;
            }
            return f;
        }
    }
}
