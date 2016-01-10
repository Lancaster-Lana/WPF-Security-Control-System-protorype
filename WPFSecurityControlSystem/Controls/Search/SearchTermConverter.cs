using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Microsoft.Windows.Controls;

namespace UIPrototype.Controls.Search
{
    public class SearchTermConverter : IMultiValueConverter
    {        
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var searchTerm = values[0] as string;

            var stringValue = values[1] == null ? string.Empty : values[1].ToString();                        

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
