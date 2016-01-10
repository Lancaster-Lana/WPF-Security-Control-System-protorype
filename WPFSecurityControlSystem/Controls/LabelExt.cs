using System.Windows;
using System.Windows.Controls;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// Extended Label with asterisk - if IsReqiured set than * shown (WindowStyle name is 'StyleLabelExt')
    /// </summary>
    public class LabelExt : Label
    {
        public static DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(LabelExt), new UIPropertyMetadata(null));

        public bool IsRequired
        {
            get
            {
                return (bool)GetValue(IsRequiredProperty);
            }
            set
            {
                SetValue(IsRequiredProperty, value);
            }
        }
    }

    public class ControlsExtentions
    {
        public static DependencyProperty IsRequiredProperty = DependencyProperty.RegisterAttached("IsRequired", typeof(bool), typeof(Label), new UIPropertyMetadata(null));

        public static bool? GetIsIsRequired(DependencyObject target)
        {
            return (bool)target.GetValue(IsRequiredProperty);
        }

        //public static DependencyProperty IsRequiredProperty = DependencyProperty.Register("IsRequired", typeof(bool), typeof(Label), new UIPropertyMetadata(null));
    }
}
