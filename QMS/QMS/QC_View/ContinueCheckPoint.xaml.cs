﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContinueCheckPoint : Rg.Plugins.Popup.Pages.PopupPage
    {
        public ContinueCheckPoint()
        {
            InitializeComponent();
        }
    }
}