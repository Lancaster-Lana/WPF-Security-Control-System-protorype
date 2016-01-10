using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using UIPrototype.Base;
using UIPrototype.Controls;
using UIPrototype.MODULE.HWConfiguration;
using UIPrototype.MODULE.HWConfiguration.Views;

namespace UIPrototype
{
    /// <summary>
    ///A ShellView or base presenter class for main HardwareConfiguration window: 
    ///interaction\navigation logic for all views of the HW Module
    /// </summary>
    public partial class HWConfigurationPresenter : RibbonWindow, IMainView
    {
        #region Properties

        /// <summary>
        /// Working area content control
        /// </summary>
        public UserControl CurrentView
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

        public HWConfigurationViewModel CurrentDataContext { get; set; }
       
        public HWModuleController ModuleController { get; set; }

        public UIPrototype.Commands.HWConfigCommands HWConfigCommands { get; set; }

        #endregion
 
        #region Constructor

        public HWConfigurationPresenter()
        {
            try
            {
                InitializeComponent();

                //1. Init DataContext
                this.CurrentDataContext = new HWConfigurationViewModel();

                //2. Load a single possible view
                this.CurrentView = new MainView(CurrentDataContext);

                //3. Init HWModule controller (register HW module views)
                this.ModuleController = new HWModuleController(CurrentView as MainView/*, CurrentDataContext*/);

                //4. Binding all commands, should be handled by the HWConfiguration MODULE          
                this.HWConfigCommands = new UIPrototype.Commands.HWConfigCommands(this.ModuleController);

                //All
                this.DataContext = this;
            }
            catch
            {
                //TODO:
            }
        }

        #endregion 

        #region Commands Handlers

        //Replaced with commands 
        //public void OnConfigClicked(object sender, RoutedEventArgs e)
        //{
        //    Button btnClicked = (Button)e.Source;

        //    switch (btnClicked.Name)
        //    {
        //        case "rbtnSetDefaults"://"rbtnSetSCPDefaults":
        //            HWModuleController.ShowSetDefaults();
        //            break;
        //    }           
        //}

        private void CanDownload(object sender, CanExecuteRoutedEventArgs e)
        {
            //if (this.ModuleController != null)
            //    this.ModuleController.EvaluateCanDefaults(sender, e);
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
                this.ModuleController.GenerateDoorsForController(e.Parameter as HWDoorsConfiguration);            
        }


        #endregion

        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
