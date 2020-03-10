
using System;
using System.Collections.Generic;
using System.Globalization;
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
            if (value != null)
            {
                if (value.ToString() == "0")
                    return "Not registered";
                if (value.ToString() == "1")
                    return "Ok";
                if (value.ToString() == "2")
                    return "Stolen";
                if (value.ToString() == "3")
                    return "Found";
            }
            return "Unknown";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }      
}
