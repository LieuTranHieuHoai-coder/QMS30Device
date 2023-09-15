using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DateTimePicker : Rg.Plugins.Popup.Pages.PopupPage
    {
        public DateTimePicker()
        {
            InitializeComponent();
        }
    }
}