using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// This user control drives searching capibility as text is typed into the 
    /// text portion of the control.
    /// </summary>
    public partial class WpfSearchControl : UserControl
    {
        #region Events

        public event TextChangedEventHandler SearchTextChanged;
        public event RoutedEventHandler SearchLostFocus;

        #endregion

        #region Constants

        internal const string DEFAULT_SEARCH_TEXT = "  Search";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the control text.
        /// </summary>
        public string ControlText
        {
            get
            {
                var controlText = String.Empty;
                if (_textBoxSearch.Text != DEFAULT_SEARCH_TEXT)
                    controlText = _textBoxSearch.Text;

                return controlText;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public WpfSearchControl()
        {
            InitializeComponent();

            InitializeDefaultSettings();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method sets up the controls with the default settings.
        /// </summary>
        private void InitializeDefaultSettings()
        {
            _textBoxSearch.Background = Brushes.White;
            _textBoxSearch.Foreground = Brushes.DarkGray;
            _textBoxSearch.FontStyle = FontStyles.Italic;
            _textBoxSearch.TextChanged -= new TextChangedEventHandler(_textBoxSearch_TextChanged);
            _textBoxSearch.Text = DEFAULT_SEARCH_TEXT;
            _textBoxSearch.TextChanged += new TextChangedEventHandler(_textBoxSearch_TextChanged);
        }

        /// <summary>
        /// Raise the lost focus event for any listeners.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void RaiseLostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchLostFocus != null)
                SearchLostFocus(sender, e);
        }

        /// <summary>
        /// Raise the text changed event for any listeners.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void RaiseTextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextChanged != null)
                SearchTextChanged(sender, e);
        }

        #endregion

        #region Events

        /// <summary>
        /// Event handles the control losing focus by changing the look of the control
        /// to that of the default look as well as letting any listeners know.
        /// 
        /// GTL - Would a routed event by better in WPF?
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _textBoxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_textBoxSearch.Text.Length == 0)
            {
                InitializeDefaultSettings();

                RaiseLostFocus(sender, e);
            }
        }

        /// <summary>
        /// Event handles the control receiving focus by changing the look of the control.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _textBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            _textBoxSearch.Background = Brushes.White;
            _textBoxSearch.Foreground = Brushes.Black;
            _textBoxSearch.FontStyle = FontStyles.Normal;
            _textBoxSearch.TextChanged -= new TextChangedEventHandler(_textBoxSearch_TextChanged);
            if (_textBoxSearch.Text == DEFAULT_SEARCH_TEXT)
                _textBoxSearch.Text = String.Empty;
            _textBoxSearch.TextChanged += new TextChangedEventHandler(_textBoxSearch_TextChanged);
        }

        /// <summary>
        /// Handles the text changed event by passing the event to any listeners.
        /// 
        /// GTL - could I use a routed event here?
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            RaiseTextChanged(sender, e);
        }

        #endregion
    }
}
