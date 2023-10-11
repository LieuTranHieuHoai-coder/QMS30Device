using QMS.Models.QcModels;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.Helpers;
using QMS.Languages;

namespace QMS.Commons
{
    public static class GlobalDefines
    {
        public static Models.Common.xUser LoggedUser { get; set; }
        public static string WorkingSite { get; set; }
        public static string WorkingFactory { get; set; }
        public static string WorkingLine { get; set; }
        public static int IndexSite { get; set; }
        public static int IndexFactory { get; set; }
        public static int IndexLine { get; set; }
        public static string PermTP { get; set; }
        public static string PermBTP { get; set; }
        public static string PermPacking { get; set; }
        public static string PermQC { get; set; }
        public static string SlectedPerm { get; set; }
       
        public static string ApiBaseUrl = "http://qvnqms.qve.com.vn";
        //public static string NewApiUrl = "http://192.168.1.46:9999";
        public static string NewApiUrl = "http://192.168.1.136:9999";
        
        public static List<Models.PermitModels.AdminGroupPerm> Permisions { get; set; }
        public static string Language = LocalizationResourceManager.Current.CurrentCulture.Name.ToLower();
        //QC object
        public static string QcQrCode { get; set; }
        public static string DotColor { get; set; }
        public static string DotFailLV_0 { get; set; }
        public static string DotFailLV_1 { get; set; }
        public static string DotFailLV_2 { get; set; }
        public static string Planno { get; set; }
        public static string DotFail_In_Out { get; set; }
        public static string ColorName { get; set; }

        public static string ImageID = "Front";
        public static string DefectID { get; set; }
        public static string ReasonID { get; set; }
        public static string PartID { get; set; }
        public static int DirectionID { get; set; }
        public static List<DefectPositionView> defectLists { get; set; }
        public static bool BtnPhotoStatus { get; set; }
        public static string Style { get; set; }
        public static string Customer { get; set; }
        public static string BuyMonth { get; set; }
        public static string WorkNo { get; set; }
        public static string SizeName { get; set; }
        public static string Season { get; set; }
        public static string ParentName { get; set; }
        public static string RpFromDate { get; set; }
        public static string RpToDate { get; set; }
        public static string RpSew { get; set; }
        public static string QcOrderNo { get; set; }
        public static int Pcode { get; set; }
        // end Qc object
        public static void ClearAll()
        {
            LoggedUser = null;
            WorkingSite = null;
            WorkingFactory = null;
            WorkingLine = null;
            IndexSite = -1;
            IndexFactory = -1;
            IndexLine = -1;
            defectLists = new List<DefectPositionView>();
            Permisions.Clear();
        }
    }
}
