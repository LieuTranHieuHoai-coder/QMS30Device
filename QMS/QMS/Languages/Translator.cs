using System;
using System.Globalization;
using System.Resources;

namespace QMS.Languages
{
    public static class Translator
    {
        private const string ResourceId = "QMS.Languages.AppResource";
        public static string Translate(this object page, string text)
        {

            ResourceManager rm = new ResourceManager(ResourceId, typeof(Translator).Assembly);
            CultureInfo culture = CultureInfo.CurrentCulture;
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            var result = rm.GetString(text, culture);
            if (result == null)
            {
#if DEBUG
                throw new ArgumentException(string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", text, ResourceId, culture.Name), "Text");
#else
                result = text;
#endif

            }
            return result;
        }
    }
}
