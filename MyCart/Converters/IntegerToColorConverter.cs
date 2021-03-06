﻿using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DigiFyy.Converters
{
    /// <summary>
    /// This class have methods to convert the Boolean values to color objects. 
    /// This is needed to validate in the Entry controls. If the validation is failed, it will return the color code of error, otherwise it will be transparent.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class IntegerToColorConverter : IValueConverter
    {
        /// <summary>
        /// This method is used to convert the bool to color.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Color.Default;

            switch (value.ToString())
            {
                case "0":
                    return Color.FromHex("#606060");    // Unregistered
                case "1":
                    return Color.FromHex("#0DD125");    // OK
                case "2":
                    return Color.FromHex("#FF3F6F");    // Stolen
                case "3":
                    return Color.FromHex("#3251FF");    // Found
                case "4":
                    Application.Current.Resources.TryGetValue("PrimaryColor", out var retVal);
                    return (Color)retVal;
                case "5":
                    Application.Current.Resources.TryGetValue("Gray-AB", out var outVal);
                    return (Color)outVal;
                case "-1":
                    return Color.FromHex("#FF7A14");    // Unknown = orange
                default:
                    return Color.Transparent;
            }
        }

        /// <summary>
        /// This method is used to convert the color to bool.
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the string.</returns>        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}