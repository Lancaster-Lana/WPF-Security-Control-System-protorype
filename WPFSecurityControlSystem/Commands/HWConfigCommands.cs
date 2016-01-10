using System;
using System.Windows.Input;

namespace WPFSecurityControlSystem.Commands
{
    public class HWConfigCommands
    {
        MODULE.HWConfiguration.HWModuleController Controller { get; set; }

        #region Constructor

        static HWConfigCommands()
        {               
            InputGestureCollection inputs = new InputGestureCollection();
            inputs.Add(new KeyGesture(Key.D, ModifierKeys.Control, "Ctrl+G"));
            GenerateDoors = new RoutedUICommand("Generate", "Generate", typeof(HWConfigCommands), inputs);
        }

        public HWConfigCommands(MODULE.HWConfiguration.HWModuleController controller) //UIPrototype.MODULE.HWConfiguration.HWConfigurationViewModel context, UIPrototype.MODULE.HWConfiguration.Views.ShellView view)
        {
            Controller = controller;  //new MODULE.HWConfiguration.HWModuleController(view);
            //Context = view.DataContext as UIPrototype.MODULE.HWConfiguration.HWConfigurationViewModel;
        }

        #endregion

        #region SetDefaults Command

        private IDenticard.Common.Wpf.RelayCommand<object> _setDefaultsCommand = null;
        /// <summary>
        /// Returns the Download command.
        /// </summary>
        public ICommand SetDefaultsCommand
        {
            get
            {
                if (_setDefaultsCommand == null)
                    _setDefaultsCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object template)
                    {
                        Controller.ShowSetDefaults();
                    });

                return _setDefaultsCommand;
            }
        }

        //public static RoutedUICommand SetDefaultsCommand = new RoutedUICommand("SetDefaults", "SetDefaults", typeof(HWConfigCommands));

        #endregion

        #region Download Command

        //public static RoutedUICommand Download { get; set; }

        private IDenticard.Common.Wpf.RelayCommand<object> _downloadCommand = null;
        /// <summary>
        /// Returns the Download command.
        /// </summary>
        public ICommand DownloadCommand
        {
            get
            {
                if (_downloadCommand == null)
                    _downloadCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object node)
                    {
                        Controller.Download(node);//this, null); or _parent.PerformDownloadClick(node);
                    });

                return _downloadCommand;
            }
        }

        #endregion      

        #region Group Commands

        private IDenticard.Common.Wpf.RelayCommand<object> _addGroupCommand = null;
        public ICommand AddGroupCommand
        {
            get
            {
                if (_addGroupCommand == null)
                    _addGroupCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate
                    {
                        //TODO:
                    });

                return _addGroupCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _editGroupCommand = null;
        public ICommand EditGroupCommand
        {
            get
            {
                if (_editGroupCommand == null)
                    _editGroupCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate
                    {
                        //TODO:
                    });

                return _editGroupCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _deleteGroupCommand = null;
        public ICommand DeleteGroupCommand
        {
            get
            {
                if (_deleteGroupCommand == null)
                    _deleteGroupCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _deleteGroupCommand;
            }
        }


        private IDenticard.Common.Wpf.RelayCommand<object> _removeDoorsCommand = null;
        public ICommand RemoveDoorsCommand
        {
            get
            {
                if (_removeDoorsCommand == null)
                    _removeDoorsCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _removeDoorsCommand;
            }
        }        

        #endregion

        #region Access Commands

        private IDenticard.Common.Wpf.RelayCommand<object> _addAccessCommand = null;
        public ICommand AddAccessCommand
        {
            get
            {
                if (_addAccessCommand == null)
                    _addAccessCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _addAccessCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _editAccessCommand = null;
        public ICommand EditAccessCommand
        {
            get
            {
                if (_editAccessCommand == null)
                    _editAccessCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _editAccessCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _deleteAccessCommand = null;
        public ICommand DeleteAccessCommand
        {
            get
            {
                if (_deleteAccessCommand == null)
                    _deleteAccessCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _deleteAccessCommand;
            }
        }

        #endregion.net

        #region Plugins Commands

        private IDenticard.Common.Wpf.RelayCommand<object> _addPluginCommand = null;
        public ICommand AddPluginCommand
        {
            get
            {
                if (_addPluginCommand == null)
                    _addPluginCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate
                    {
                        //TODO:
                    });

                return _addPluginCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _editPluginCommand = null;
        public ICommand EditPluginCommand
        {
            get
            {
                if (_editPluginCommand == null)
                    _editPluginCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate
                    {
                        //TODO:
                    });

                return _editPluginCommand;
            }
        }

        private IDenticard.Common.Wpf.RelayCommand<object> _deletePluginCommand = null;
        public ICommand DeletePluginCommand
        {
            get
            {
                if (_deletePluginCommand == null)
                    _deletePluginCommand = new IDenticard.Common.Wpf.RelayCommand<object>(delegate(object group)
                    {
                        //TODO:
                    });

                return _deletePluginCommand;
            }
        }

        #endregion

        #region ConfigureCommand

        /// <summary>
        /// Configuring\ generation of subTree for Site\Controller\IOBoards (count)
        /// </summary>
        public static RoutedUICommand GenerateDoors { get; set; }

        public event EventHandler Configure;

        IDenticard.Common.Wpf.RelayCommand _configureCommand;

        /// <summary>
        /// Returns the ConfigureCommand 
        /// </summary>
        public ICommand ConfigureCommand
        {
            get
            {
                if (_configureCommand == null)
                    _configureCommand = new IDenticard.Common.Wpf.RelayCommand(delegate()
                    {
                        if (Configure != null)
                            Configure(this, EventArgs.Empty);
                    });
                return _configureCommand;
            }
        }
       

        #endregion                           
    }
}
