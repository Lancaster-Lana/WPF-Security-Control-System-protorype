using System.Windows;
using System.Windows.Controls;

namespace WPFSecurityControlSystem.Controls
{
    public class TemplateBorder : Border
    {
        static TemplateBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TemplateBorder), new FrameworkPropertyMetadata(typeof(TemplateBorder)));
        }
    }

    public class SearchTextBox : TextBox
    {
        static SearchTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTextBox), new FrameworkPropertyMetadata(typeof(SearchTextBox)));

            TextProperty.OverrideMetadata(
                typeof(SearchTextBox),
                new FrameworkPropertyMetadata(new PropertyChangedCallback(TextPropertyChanged)));
        }

        public static readonly DependencyProperty TextBoxInfoProperty = DependencyProperty.Register(
            "TextBoxInfo",
            typeof(string),
            typeof(SearchTextBox),
            new PropertyMetadata(string.Empty));

        public string TextBoxInfo
        {
            get { return (string)GetValue(TextBoxInfoProperty); }
            set { SetValue(TextBoxInfoProperty, value); }
        }

        private static readonly DependencyPropertyKey HasTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasText",
            typeof(bool),
            typeof(SearchTextBox),
            new FrameworkPropertyMetadata(false));

        public static readonly DependencyProperty HasTextProperty = HasTextPropertyKey.DependencyProperty;

        public bool HasText
        {
            get
            {
                return (bool)GetValue(HasTextProperty);
            }
        }

        static void TextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            SearchTextBox itb = (SearchTextBox)sender;

            bool actuallyHasText = itb.Text.Length > 0;
            if (actuallyHasText != itb.HasText)
            {
                itb.SetValue(HasTextPropertyKey, actuallyHasText);
            }
        }
    }
}




