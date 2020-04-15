
using DigiFyy.Helpers;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DigiFyy.Converters
{
    /// <summary>
    /// This class have methods to convert the Status number values to string.     
    /// </summary>
    [Preserve(AllMembers = true)]
    public class StatusIntegerToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resmgr = new ResourceManager("DigiFyy.Resources.AppResources", typeof(TranslateExtension).GetTypeInfo().Assembly);
            var ci = CrossMultilingual.Current.CurrentCultureInfo;
        
            if (value != null)
            {
                if (value.ToString() == "0")
                    return resmgr.GetString("StatusNotRegistered", ci);
                if (value.ToString() == "1")
                    return resmgr.GetString("StatusRegistered", ci);
                if (value.ToString() == "2")
                    return resmgr.GetString("StatusStolen", ci);
                if (value.ToString() == "3")                    
                    return resmgr.GetString("StatusFound", ci);
            }
            return resmgr.GetString("StatusUnknown", ci);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }      
}
