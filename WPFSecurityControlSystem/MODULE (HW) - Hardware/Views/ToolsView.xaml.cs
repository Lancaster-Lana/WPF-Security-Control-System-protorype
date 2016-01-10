using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Services;
using WPFSecurityControlSystem.Commands;
using IDenticard.AccessUI;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration.Views
{
    /// <summary>
    /// Interaction logic for Right Toolbox
    /// </summary>
    public partial class ToolsView : UserControl
    {
        #region Properties
        
        //dynamic _currentAccessObject;
        HWConfigurationViewModel _vm;

        //HWDoorsConfiguration _currentConfiguration;
        //HWDoorsConfiguration CurrentConfiguration
        //{
        //    get
        //    { 
        //        if(_currentConfiguration == null)
        //            _currentConfiguration = new HWDoorsConfiguration();
                
        //        return 
        //    }
        //}

        #endregion
        
        #region Constructor

        public ToolsView()
        {
            InitializeComponent();
        }

        public ToolsView(object context)//DI
            : this()
        {
            this.DataContext = context;
            _vm = context as HWConfigurationViewModel; //Attach shell(common) context for all regions            
            _vm.NavigationLinkChanged += new EventHandler(OnNavigationLinkChanged);

            //Init the RIGHT REGION: accordion toolbox controls    
            //IRegion rightContentRegion = this.regionManager.Regions[RegionNames.RightToolsContentRegion];
            //cmbSite.ItemsSource = context.Sites;
            cmbSite.DisplayMemberPath = "Node.Name";//__Site.SPParamSiteID;
            cmbSite.SelectedValuePath = __Site.ColumnSiteID;

            cmbScpType.ItemsSource = DataService.GetScpTypes();
            cmbScpType.DisplayMemberPath = __SCPType.ColumnName;
            cmbScpType.SelectedValuePath = __SCPType.ColumnSCPTypeID;

            var ioBoardTypes = DataService.GetIOBoardTypes();
            ioBoardTypes.RowFilter = __SIOType.ColumnSIOTypeID + " not in ("
                                    + (int)IDenticard.Access.Common.SIOType.MR16IN + ","
                                    + (int)IDenticard.Access.Common.SIOType.MR16OUT + ")";
            cmbIOBoardType.ItemsSource = ioBoardTypes;

            cmbIOBoardType.DisplayMemberPath = __SIOType.ColumnName;
            cmbIOBoardType.SelectedValuePath = __SIOType.ColumnSIOTypeID;

            //cmbController.ItemsSource = ControllersList;// DataService.GetSiteControllers(selectedSiteID);//TODO
            ControllersList = (ListCollectionView)CollectionViewSource.GetDefaultView(DataService.Controllers);
        }

        #endregion
        
        #region The accordion toolbox handlers (the right region)
      
        ICollectionView ControllersList
        {
            get
            {
                return CollectionViewSource.GetDefaultView(cmbController.ItemsSource);
            }
            set
            {
                //if (_controllersView == null)
                //    _controllersView = (ListCollectionView)CollectionViewSource.GetDefaultView(DataService.Controllers);               
                cmbController.ItemsSource = value;
            }
        }

        private void cmbSite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedSiteID = cmbSite.SelectedItem is IDenticard.Premisys.Site ? ((IDenticard.Premisys.Site)cmbSite.SelectedItem).SITE_ID : -1;
            if (ControllersList != null)
                ControllersList.Filter = (selectedSiteID == -1) ? null :
                                        new Predicate<object>(delegate(object item)
                                        {
                                            if (item == null) return false;
                                            var controller = item as IDenticard.Premisys.SCP;
                                            return controller != null && controller.SITE_ID != null && controller.SITE_ID == selectedSiteID;
                                        });

            //REPLACED direct DAL methods with late binding
            //ObjectDataProvider orderProvider = this.FindResource("dsControllers") as ObjectDataProvider;
            //orderProvider.MethodParameters[0] = selectedSiteID;                         
        }

        private void btnAddSite_Click(object sender, RoutedEventArgs e)
        {           
            var parentCollectionLink = _vm.GetParentBOCollection<Site>(0);
            _vm.ExecuteCommand<Site>(ApplicationCommands.New, parentCollectionLink);

            //TODO UI part: Should be updated form ViewModel on CurrentLinkNode changed event
            cmbSite.ItemsSource = _vm.Sites; //Should be refresed automatially
            cmbSite.SelectedValue = _vm.CurrentSiteID;
        }

        private void btnAddControllerOfTheType_Click(object sender, RoutedEventArgs e)
        {
            var parentSite = cmbSite.SelectedItem as IDenticard.Premisys.Site;

            if (parentSite == null)
            {
                MessageBox.Show("Please, select a site before a new controller will be created.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            //var node = parentSite.Node;           
            //TODO: Find command parameter - parent SCPCollection to add a new controller
            int site_id = parentSite.SITE_ID;
            var parentCollectionLink = _vm.GetParentBOCollection<SCP>(site_id);

            var scpType = cmbScpType.SelectedValue;

            //Call a command to add a new controller
            _vm.ExecuteCommand<SCP>(ApplicationCommands.New, parentCollectionLink, scpType);

            //TODO UI part: Should be updated form ViewModel on CurrentLinkNode changed event
            cmbController.ItemsSource = DataService.GetSiteControllers(_vm.CurrentSiteID);
            cmbController.SelectedValue = _vm.CurrentLink.Id;//= created controller id
        }

        private void btnGenerateDoorsGroup_Click(object sender, RoutedEventArgs e)
        {
            var parentController = cmbController.SelectedItem as IDenticard.Premisys.SCP;
            if (parentController == null)
            {
                MessageBox.Show("Please, select a controller before a new doors group generation.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            int scp_id = parentController.SCP_ID;

            var sioType = cmbIOBoardType.SelectedValue; //IDenticard.Access.Common.SIOType
            if (sioType == null)
            {
                MessageBox.Show("Please, pick up an IOBoard type before a new doors group generation.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
           
            int doorsCount = (int)upDoorsCount.Value;


            var parentCollectionLink = _vm.GetParentBOCollection<Door>(scp_id);
            _vm.ExecuteCommand<Door>(HWConfigCommands.GenerateDoors, parentCollectionLink, sioType, doorsCount);
        }

        #endregion

        #region External Handlers
       
        /// <summary>
        /// The external navigation event handler: force update fileds in the right region toolbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigationLinkChanged(object sender, EventArgs e)
        {            
            if (_vm.CurrentLink == null)
            {
                cmbSite.SelectedValue = -1;
                cmbIOBoardType.SelectedValue = -1;
                cmbScpType.SelectedValue = -1;
                cmbController.SelectedValue = -1;
            }
            else
            {
                if (cmbSite.SelectedValue == null || Convert.ToInt32(cmbSite.SelectedValue) != _vm.CurrentSiteID)
                    cmbSite.SelectedValue = _vm.CurrentSiteID;

                if (_vm.CurrentObject is SCP)
                {
                    cmbController.SelectedValue = _vm.CurrentLink.Id;
                    cmbScpType.SelectedValue = ((SCP)_vm.CurrentObject).SCPType;
                }
            }
        
        }

        #endregion

    }
}
