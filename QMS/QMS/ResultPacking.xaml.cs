using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultPacking : ContentPage
    {
        public ResultPacking()
        {
            InitializeComponent();
        }
        public ResultPacking(string QRcode)
        {
            InitializeComponent();
            
        }

       
    }
}