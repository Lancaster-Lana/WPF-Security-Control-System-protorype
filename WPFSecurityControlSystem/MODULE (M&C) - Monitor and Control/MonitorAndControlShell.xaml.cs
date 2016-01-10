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
    public partial class MonitorAndControlShell : RibbonWindow, IShellView
    {
        #region Properties

        /// <summary>
        /// Working area content control
        /// </summary>
        public UserControl ActiveView
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

        public MonitorAndControlShell()
        {
            try
            {
                InitializeComponent();

                //1. Init DataContext
                this.CurrentDataContext = new HWConfigurationViewModel();

                //2. Load a single possible view
                //this.ActiveView = new ActiveView(CurrentDataContext);

                //3. Init HWModule controller (register HW module views)
                this.ModuleController = new HWModuleController(ActiveView as MainView);

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

    }
}
