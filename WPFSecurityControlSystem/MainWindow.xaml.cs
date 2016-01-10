using System.Windows;

namespace WPFSecurityControlSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructor
        
        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion 

        #region Handlers

        public void OnHardwareConfigClicked(object sender, RoutedEventArgs e)
        {
            HWConfigurationShell hwConfigWindow = new HWConfigurationShell();
            hwConfigWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            hwConfigWindow.ShowDialog();
        }
 
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

    }   
}
