﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace WPFSecurityControlSystem.Controls.Search
{
    public static class SearchOperations
    {
        public static string GetSearchTerm(DependencyObject obj)
        {
            return (string)obj.GetValue(SearchTermProperty);
        }

        public static void SetSearchTerm(DependencyObject obj, string value)
        {
            obj.SetValue(SearchTermProperty, value);
        }

        public static readonly DependencyProperty SearchTermProperty =
            DependencyProperty.RegisterAttached(
                "SearchTerm",
                typeof(string),
                typeof(SearchOperations),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetIsMatch(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsMatchProperty);
        }

        public static void SetIsMatch(DependencyObject obj, bool value)
        {
            obj.SetValue(IsMatchProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsMatch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsMatchProperty =
            DependencyProperty.RegisterAttached("IsMatch", typeof(bool), typeof(SearchOperations), new UIPropertyMetadata(false));
    }

    public class SearchTermConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var searchTerm = values[0] as string;
            var data = values[2];
            var dataType = data.GetType();

            var columnName = values[1].ToString();            
            var columnProperty = dataType.GetProperty(columnName); //.NET Reflection                       
            var value = columnProperty.GetValue(data, null);
            
            string stringValue = value == null ? string.Empty : value.ToString();
            
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
