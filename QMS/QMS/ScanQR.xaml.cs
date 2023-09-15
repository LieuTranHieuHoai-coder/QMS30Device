using BarcodeScanner.Mobile;
using QMS.Languages;
using QMS.QC_View;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace QMS
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanQR : ContentPage
    {

        public ScanQR()
        {
            InitializeComponent();
            Methods.AskForRequiredPermission();
            Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);
        }
        private void CameraView_OnDetectedAsync(object sender, OnDetectedEventArg e)
        {
            List<BarcodeResult> barcodeResults = e.BarcodeResults;
            string result = string.Empty;
            if (barcodeResults.Count > 0)
            {
                result = barcodeResults[0].DisplayValue;
            }
            // Finish Good
            if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("finish"))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                   // Methods.SetIsScanning(false); //disable scanning
                    await Navigation.PushModalAsync(new ResultQR(result));

                });
            }
            // Semi Good
            if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("semi")) 
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                   // Methods.SetIsScanning(false); //disable scanning
                    await Navigation.PushModalAsync(new ResultSemiGood(result));

                });
            }
            // QC Check Code
            if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("check qrcode (qc)"))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //Methods.SetIsScanning(false); //disable scanning
                    Commons.GlobalDefines.QcQrCode = result;
                    await Navigation.PushModalAsync(new CheckQRCode(result));

                });
            }
            // QC Recevice 
            if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("qc (qc receiving)"))
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                   // Methods.SetIsScanning(false); //disable scanning

                    await Navigation.PushModalAsync(new QCReceiving(result));

                });
            }
            // Packing
            if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("packing")) 
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                   // Methods.SetIsScanning(false); //disable scanning
                    await Navigation.PushModalAsync(new ResultPacking(result));

                });
            }
            

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Methods.SetIsScanning(true); //enable scanning
        }

        private async void btnBack_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void btnscan_Clicked(object sender, System.EventArgs e)
        {
            if (txtqrcode.Text==null)
            {
                // await DisplayAlert("Msg", " Nhập Mã QR", "OK");
                await DisplayAlert(this.Translate("Msg"), this.Translate("InputQR"), "OK");
                _ = txtqrcode.Focus();
            }
            if (txtqrcode.Text != null)
            {
                string result = txtqrcode.Text;
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("finish"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new ResultQR(result));

                    });
                }
                // Semi Good
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("semi"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new ResultSemiGood(result));

                    });
                }
                // QC Check Code
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("check qrcode (qc)"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Commons.GlobalDefines.QcQrCode = result;
                        await Navigation.PushModalAsync(new CheckQRCode(result));

                    });
                }
                // QC Recevice 
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Equals("qc (qc receiving)"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new QCReceiving(result));

                    });
                }
                // Packing
                if (Commons.GlobalDefines.SlectedPerm.ToString().ToLower().Contains("packing"))
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushModalAsync(new ResultQR(result));

                    });
                }
               
                txtqrcode.Text = "";
            }                   
        }
      
    }
}