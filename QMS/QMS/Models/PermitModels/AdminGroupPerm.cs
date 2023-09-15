using System;

namespace QMS.Models.PermitModels
{
    public class AdminGroupPerm
    {
        public int Id { get; set; }
        public int Group_id { get; set; }
        public int Menu_id { get; set; }
        public string Switch { get; set; }
        public int Permission { get; set; }
        public string PermBinary => ConvertToBinary();
        public string Comment { get; set; }
        public DateTime Lastdt { get; set; }
        public string Lastuid { get; set; }

        private string ConvertToBinary()
        {
            var result = "";
            var num = Permission;
            while (num > 1)
            {
                int remainder = Permission % 2;
                result = Convert.ToString(remainder) + result;
                num /= 2;
            }
            result = Convert.ToString(num) + result;
            return result;
        }
    }
}
