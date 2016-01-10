using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Windows.Controls.Ribbon;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.MODULE.HWConfiguration;
using WPFSecurityControlSystem.MODULE.HWConfiguration.Views;

namespace WPFSecurityControlSystem
{
    /// <summary>
    ///The shell view for the HW Configuration window combined with UI presentation logic(or UIService class for this module): 
    ///for interaction\navigation for all views of the HWConfiguration Module
    /// </summary>
    public partial class HWConfigurationShell : RibbonWindow, IShellView
    {
        #region Properties

        /// <summary>
        /// Work area content control
        /// </summary>
        public UserControl ContentsView
        {
            get
            {
                return this.ClientArea.Content as UserControl;
            }
            set
            {
                this.ClientArea.Content = value;
            }
        }

        /// <summary>
        /// The left-side tree view
        /// </summary>
        public UserControl NavigationView
        {
            get
            {
                return this.NavigationRegion.Content as UserControl;
            }
            set
            {
                this.NavigationRegion.Content = value;
            }
        }

        /// <summary>
        /// The right-side accordion toolbox
        /// </summary>
        public UserControl ToolsView
        {
            get
            {
                return this.ToolsRegion.Content as UserControl;
            }
            set
            {
                this.ToolsRegion.Content = value;
            }
        }

        //public HWConfigurationViewModel CurrentDataContext { get; set; }
       
        public HWModuleController ModuleController { get; set; }

        public WPFSecurityControlSystem.Commands.HWConfigCommands HWConfigCommands { get; set; }

        #endregion
 
        #region Constructor

        public HWConfigurationShell()
        {
            try
            {
                InitializeComponent();
                btnHideLeftTools.Content = "<";
                btnHidRightTools.Content = ">";

                //Init DataContext - common for all regions                
                this.DataContext = new HWConfigurationViewModel();

                //1. Init HWModule controller (register HW module views)
                this.ModuleController = new HWModuleController(this);

                // Binding all commands, should be handled by the HWConfiguration MODULE          
                this.HWConfigCommands = new WPFSecurityControlSystem.Commands.HWConfigCommands(this.ModuleController);

                //2. Load views to the main region, to the navigation and tools regions
                this.ContentsView = new MainContentsView(this.DataContext); // common context for all view of the Shell window
                //Load views for navigation and tools regions
                this.NavigationView = new NavigationView(this.DataContext);
                this.ToolsView = new ToolsView(this.DataContext);              
            }
            catch
            {
                //TODO:
            }
        }

        #endregion 

        #region Commands Handlers

        private void CanDownload(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (this.ModuleController != null)
            //    this.ModuleController.EvaluateCanDownload(sender, e);
            e.CanExecute = true; //Check Admin rights
        }

        private void Download(object sender, ExecutedRoutedEventArgs e)
        {
           if (this.ModuleController != null)
               this.ModuleController.Download(sender);
        }

        private void CanSetDefaults(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (this.ModuleController != null)
            //    this.ModuleController.EvaluateCanDefaults(sender, e);
            e.CanExecute = true; //Admin rights
        }

        private void SetDefaults(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.ShowSetDefaults();            
        }

        private void CanCreate(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.EvaluateCanCreate(sender, e);
        }
        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        public void AddHardware(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.AddHardwareElement(sender, e); 
        }

        private void CanEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.EvaluateCanEdit(sender, e);
        }
        public void EditHardware(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.EditHardwareElement(sender, e);
        }

        private void CanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.EvaluateCanDelete(sender, e);
        }
        public void DeleteHardware(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ModuleController != null)
                this.ModuleController.DeleteHardwareElement(sender, e);            
        }

        private void GenerateDoorsForController(object sender, ExecutedRoutedEventArgs e)
        {
            if (this.ModuleController != null)
            {
                //HWDoorsConfiguration = this.ToolsView.CurrentConfiguration;
                var doors = this.ModuleController.GenerateDoorsForController(e.Parameter as HWDoorsConfiguration);
                MessageBox.Show("You have generated " + doors.Count + " doors", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Methods

        public void RefreshUI(IDenticard.Access.Common.LinkNode parentNode, IDenticard.Access.Common.LinkNode navigationNode, bool fullRefresh)
        {
            //1. Refresh navigation region
            ((INavigationView)this.NavigationView).Refresh(parentNode, navigationNode); //with full refresh

            //2.  Refresh central and right toolbox region
            //TODO:
        }

        #endregion

        #region the ShellView Handlers (common for regions manipulation)

        private void btnHideLeftTools_Click(object sender, RoutedEventArgs e)
        {
            if (btnHideLeftTools.IsChecked == false)
            {
                pnlLeftTabs.Visibility = System.Windows.Visibility.Visible;
                btnHideLeftTools.Content = "<";
            }
            else if (btnHideLeftTools.IsChecked == true)
            {
                pnlLeftTabs.Visibility = System.Windows.Visibility.Collapsed;
                btnHideLeftTools.Content = ">";
            }
        }

        private void btnHidRightTools_Click(object sender, RoutedEventArgs e)
        {
            if (btnHidRightTools.IsChecked == false)
            {
                pnlRightAccordionTools.Visibility = System.Windows.Visibility.Visible;
                btnHidRightTools.Content = ">";
            }
            else if (btnHidRightTools.IsChecked == true)
            {
                pnlRightAccordionTools.Visibility = System.Windows.Visibility.Collapsed;
                btnHidRightTools.Content = "<";
            }
        }

        #endregion

    }
}
