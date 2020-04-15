
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DigiFyy.Converters
{
    public class LongtoShortenedStringValueConverter
    {
        /// <summary>
        /// This class have methods to convert the Status number values to string.     
        /// </summary>
        [Preserve(AllMembers = true)]
        public class LongtoShortenedString : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int maxLength = 30;
                if (value == null)
                {
                    return string.Empty;
                }
                string longString = value as string;
                if (longString.Length < maxLength)
                    return longString;
                return string.Concat(longString.Substring(0, maxLength), "..");
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return true;
            }
        }
    }
}
