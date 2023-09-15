using QMS.Languages;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using System.ComponentModel;
using QMS.QC_View;

[assembly: ExportFont("MaterialIconsOutlinedRegular.otf", Alias = "MaterialIconsOutlinedRegular")]
namespace QMS
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            InitSystemLaguage();

            MainPage = new MainPage();
            // MainPage = new FlyoutMenuPage();
            //MainPage = new QC_View.PopupFunction();
            // MainPage = new QC_View.UserProfile();
            // MainPage = new QC_View.MainMenu();
            //MainPage = new QC_View.SearchDefect();
            //MainPage = new Page1();
            Commons.GlobalDefines.Permisions = new System.Collections.Generic.List<Models.PermitModels.AdminGroupPerm>();
        }

        public void InitSystemLaguage()
        {
            LocalizationResourceManager.Current.PropertyChanged += Current_PropertyChanged;
            LocalizationResourceManager.Current.Init(AppResource.ResourceManager);
        }

        private void Current_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AppResource.Culture = LocalizationResourceManager.Current.CurrentCulture;
        }        

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
