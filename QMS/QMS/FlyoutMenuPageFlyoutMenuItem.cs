using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS
{
    public class FlyoutMenuPageFlyoutMenuItem
    {
        public FlyoutMenuPageFlyoutMenuItem()
        {
            TargetType = typeof(FlyoutMenuPageFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}