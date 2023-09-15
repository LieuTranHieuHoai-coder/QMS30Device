using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS.QC_View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowImage : ContentPage
    {
        //public ImageSource MyImage { get; set; }
        public ShowImage(byte[] imgByte)
        {

            //showImageAPI(imgByte);
            
            //this.BindingContext = MyImage;

            InitializeComponent();

            MyImage.Source = showImageAPI(imgByte);

            //showImage.WidthRequest = DeviceDisplay.MainDisplayInfo.Width;
            //showImage.HeightRequest = DeviceDisplay.MainDisplayInfo.Height / 1.7;


        }

        public ImageSource showImageAPI(byte[] src)
        {
            return ImageSource.FromStream(() => new MemoryStream(src));
        }     

        private async void backbtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}