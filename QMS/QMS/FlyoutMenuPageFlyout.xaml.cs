using QMS.QC_View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlyoutMenuPageFlyout : ContentPage
    {
        public ListView ListView;

        public FlyoutMenuPageFlyout()
        {
            InitializeComponent();

            BindingContext = new FlyoutMenuPageFlyoutViewModel();
            ListView = MenuItemsListView;
            btnloginsystem.Text = Commons.GlobalDefines.LoggedUser.Uid;
        }

        class FlyoutMenuPageFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<FlyoutMenuPageFlyoutMenuItem> MenuItems { get; set; }

            public FlyoutMenuPageFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<FlyoutMenuPageFlyoutMenuItem>(new[]
                {
                    new FlyoutMenuPageFlyoutMenuItem { Id = 0, Title = "FG-QC", TargetType = typeof(PopupFunction)} ,
                    new FlyoutMenuPageFlyoutMenuItem { Id = 1, Title = "PACKING" },
                    new FlyoutMenuPageFlyoutMenuItem { Id = 2, Title = "SEMI-QC" },
                    new FlyoutMenuPageFlyoutMenuItem { Id = 3, Title = "INLINEQC" },
                    new FlyoutMenuPageFlyoutMenuItem { Id = 4, Title = "LOGOUT", TargetType=typeof(MainPage)},


                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        private void logout_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private void Label_BindingContextChanged(object sender, EventArgs e)
        {
            
        }

        private async void profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new UserProfile());
        }
    }
}