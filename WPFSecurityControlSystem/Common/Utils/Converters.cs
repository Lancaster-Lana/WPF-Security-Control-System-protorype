using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Net;
using WPFSecurityControlSystem.Common;

namespace WPFSecurityControlSystem.Utils
{
    public static class ConverterHelper
    {         
        public static bool TryParseIP(string inputstring, out IPAddress resultIP)
        {
            object ip = CultureInfo.CurrentUICulture.GetFormat(typeof(IPAddress));
            resultIP = null;
            try
            {
                var octets = inputstring.Split('.', ',');
                var bytes = new byte[4];
                for (int i = 0; i < 4; i++) //only 4 there is needed            
                    bytes[i] = Convert.ToByte(octets[i].Trim());

                //Trim() removes left\right whitespaces,
                //ToByte() convert to a value in the range 0-255
                resultIP = new IPAddress(bytes); //Try to create a valid IP address with valid octets                 
            }
            catch //parse error
            {
                return false;
            }

            return true;
        }

        public static int CompareIP(IPAddress startIp, IPAddress endIp)
        {
            byte[] bytesStart = startIp.GetAddressBytes();
            byte[] bytesEnd = endIp.GetAddressBytes();
            for(int i = 0; i < 4; i++)
            {
                if(bytesStart[i] > bytesEnd[i])
                    return 1;
                else if (bytesStart[i] < bytesEnd[i])
                    return -1;            
            }
            
            return 0;
        }


        public static bool IpRangeContains(IDenticard.AccessUI.IpAddressRange range, IPAddress ip)
        {
            return CompareIP(range.StartAddress, ip) == -1  && CompareIP(range.EndAddress, ip) == 1
                  || CompareIP(range.StartAddress, ip) == 0 || CompareIP(range.EndAddress, ip) == 0;
        }        

        public static bool GetIpAddressAndPort(string commString, out IPAddress ip, out int port)
        {
            ip = null;
            port = 0;
            try
            {
                var octets = commString.Split(Constants.DOT_SEPARATOR);
                var bytes = new byte[4];
                for (int i = 0; i < 4; i++)          
                    bytes[i] = Convert.ToByte(octets[i].Trim());
                ip = new IPAddress(bytes); //Try to create a valid IP address with valid octets      
                port = (octets.Count() > 4) ? Convert.ToInt32(octets[4]) : 0;
            }
            catch //parse error
            {
                return false;
            }

            return true;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class VisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool sourceValue = (bool)value;
            if (sourceValue)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility sourceValue = (Visibility)value;
            if (sourceValue == Visibility.Visible)
                return true;
            else
                return false;
        }

        #endregion
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class ImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //var imageIndex = (int)value;
            //    return ResourcesHelper.GetImagePath(imageIndex);      

           return ResourcesHelper.GetImagePathForHWItem(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {            
             return false;
        }

        #endregion
    }
    

    [ValueConversion(typeof(bool), typeof(char))]
    public class AsteriskConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool sourceValue = (bool)value;
            return (sourceValue) ? '*' : ' ';
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            char sourceValue = (char)value;
            return sourceValue.Equals('*');
        }
    }

    /// <summary>
    /// Converts between a DateTime and a String.
    /// </summary>
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime date = (DateTime)value;
            return date.ToString("d", culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, culture, DateTimeStyles.None, out resultDateTime))
            {
                return resultDateTime;
            }

            return value;
        }
    }

    public class ErrorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ReadOnlyObservableCollection<ValidationError> errors = value as ReadOnlyObservableCollection<ValidationError>;

            if (errors == null || errors.Count == 0)
                return string.Empty;

            return errors[0].ErrorContent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class SearchTermConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = values[0] == null ? string.Empty : values[0].ToString();
            var searchTerm = values[1] as string;

            return !string.IsNullOrEmpty(searchTerm) &&
                   !string.IsNullOrEmpty(stringValue) &&
                   stringValue.ToLower().Contains(searchTerm.ToLower());
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

}
