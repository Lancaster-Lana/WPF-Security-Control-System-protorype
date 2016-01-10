using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using UIPrototype.Base;
using UIPrototype.Services;
using UIPrototype.Controls;
using UIPrototype.Common;

using IDenticard.Common.DBConstant;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using System.ComponentModel;

namespace UIPrototype.MODULE.HWConfiguration.Views
{
    /// <summary>
    /// A base class for HW Configuration view with its common layout 
    /// </summary>
    public sealed partial class MainView : BasePropertiesControl
    {
        #region Variables & Properties
        
        HWConfigurationViewModel _vm;
        dynamic _currentAccessObject; //it is object currently selected mainly

        public LinkNode SelectedTreeItem
        {
            get
            {                
                return this.treeHWConfiguration.SelectedItem;
            }          
        }

        public TreeViewItem SelectedTreeNode
        {
            get
            {                                                                                                                          
                return treeHWConfiguration.SelectedNode;//OR treeHWConfiguration.GetTreeNode(SelectedTreeLink); 
            }
        }

        //[Import(AllowRecomposition = false)]
        //public IModuleManager ModuleManager;

        //[Import(AllowRecomposition = false)]
        //public IRegionManager RegionManager;

        #endregion

        #region Constructor

        public MainView()
        {
            InitializeComponent();

            btnHideLeftTools.Content = "<";
            btnHidRightTools.Content = ">";

            treeHWConfiguration.SelectedItemChanged +=new IDenticard.WpfUI.GenericTreeView.RoutedTreeItemEventHandler<IDenticard.Access.Common.LinkNode>(treeHWConfiguration_SelectedItemChanged);            
        }      

        public MainView(HWConfigurationViewModel context)
            : this()
        {
            this.DataContext = _vm = context;  //Attach ViewModel 
            this._vm.NavigationLinkChanged += new EventHandler(OnNavigationLinkChanged);
            this._vm.NavigationSiteChanged += new EventHandler(OnNavigationSiteChanged);

            LoadProperties(context);     //Load data into views entity-site\controller\reader                 
        }

        #endregion

        #region Methods

        public override void LoadProperties(dynamic entity)
        {
            var context = entity as HWConfigurationViewModel;

            base.LoadProperties(context);            

            //1. Load data into the LEFT REGION: fill hierarchical\tree controls 
            //IRegion leftContentRegion = this.regionManager.Regions[RegionNames.LeftHWContentRegion];
            //1.1 Load the left TreeViews
            treeHWConfiguration.Items = context.HWTreeItems;
            //TODO: retain information about all HWConfig nodes for search over the whole tree

            treeGlobals.ItemsSource = context.GLOBALItems;           
            treeAccessSettings.ItemsSource = context.AccessSettingItems;

            treePlugins.ItemsSource = context.PluginsItems; 

            //2 Load data into the main area\CENTRAL REGION: filterable Grid
            //IRegion mainContentRegion = this.regionManager.Regions[RegionNames.MainContentRegion];
            ctrlSiteContent.LoadData(context);
            // ctrlSiteContent.cmbViewing.ItemsSource = cmbSite.ItemsSource = DataSourceHelper.Sites.Select(s => s.Node.Name);
            // ctrlSiteContent.gridPoints.ItemsSource = context.SiteDoorsData;

            //3. Init the RIGHT REGION: accordion toolbox controls    
            //IRegion rightContentRegion = this.regionManager.Regions[RegionNames.RightToolsContentRegion];
            cmbSite.ItemsSource = context.Sites;
            cmbSite.DisplayMemberPath = "Node.Name";//__Site.SPParamSiteID;
            cmbSite.SelectedValuePath = __Site.ColumnSiteID;

            cmbScpType.ItemsSource = DataService.GetScpTypes();
            cmbScpType.DisplayMemberPath = __SCPType.ColumnName;
            cmbScpType.SelectedValuePath = __SCPType.ColumnSCPTypeID;

            cmbIOBoardType.ItemsSource = DataService.GetIOBoardTypes();
            cmbIOBoardType.DisplayMemberPath = __SIOType.ColumnName;
            cmbIOBoardType.SelectedValuePath = __SIOType.ColumnSIOTypeID;

            //cmbController.ItemsSource = ControllersList;// DataService.GetSiteControllers(selectedSiteID);//TODO
            ControllersList = (ListCollectionView)CollectionViewSource.GetDefaultView(DataService.Controllers);
        }

        //protected override void LoadFilterableControls()
        //{
        //    base.LoadFilterableControls();
        //}        

        /// <summary>
        /// Refresh data in all regions (child views): HWConfiguration tree, Points grid, filterable controls
        /// </summary>
        public void Refresh(LinkNode fromNode, LinkNode navigationNode, bool fullRefresh)
        {            
            //Region[Left].Refresh()
            //1. TODO: Update node data in HW Tree
            this.RefreshAndNavigateTreeNode(fromNode, navigationNode); //- Region[RightNavigation].Refresh() 
                                                
            //2.Refresh data in all related views (via changing of DataContext)           
            _vm.RefreshAllViews(navigationNode, fullRefresh); //- Region[Central].Refresh()                            
        }

        private void RefreshAndNavigateTreeNode(LinkNode fromNode, LinkNode navigationNode)
        {
            this.treeHWConfiguration.RefreshNode(fromNode, navigationNode);//Refresh HWConfiguration subTree from this node
        }

        private void RefreshHWTreeContextMenu(object context)
        {
            var linkNode = context as LinkNode;
            string nodeName = linkNode != null ? linkNode.Name : string.Empty;
            bool isFolder = linkNode != null ? linkNode.IsCollection/*IsFolder*/ : false;
            object nodeObject = linkNode != null ? linkNode.AccessObjectLink : null;
            string nodeType = linkNode != null ? linkNode.UiId : null;

            if (nodeType == null) return;

            menuAddSite.IsEnabled = isFolder && nodeType.Equals("IDenticard.AccessUI.Site");
            menuAddSite.Visibility = nodeName.Equals(Constants.SitesFolder) ? Visibility.Visible : Visibility.Collapsed;

            menuEditSite.IsEnabled = menuDeleteSite.IsEnabled = menuDownloadSite.IsEnabled 
                                  = !isFolder && nodeType.Equals("IDenticard.AccessUI.Site");

            menuAddController.IsEnabled = isFolder && nodeName.ToLower().Equals(Constants.ControllersFolder.ToLower()); //nodeTag is Site;
            menuEditController.IsEnabled = menuDeleteController.IsEnabled = !isFolder && nodeType.Equals("IDenticard.AccessUI.SCP");

            menuAddIOBoard.IsEnabled = isFolder && nodeName.ToLower().Equals(Constants.IOBoardsFolder.ToLower()); //nodeTag is SCP;
            menuEditIOBoard.IsEnabled = menuDeleteIOBoard.IsEnabled = !isFolder && nodeType.Equals("IDenticard.AccessUI.SIO");
        }

        #endregion

        #region Event Handlers
   
        #region Common Shell Handlers

        private void btnHideLeftTools_Click(object sender, RoutedEventArgs e)
        {
            if (btnHideLeftTools.IsChecked == false)
            {
                pnlLeftTabs.Visibility = System.Windows.Visibility.Visible;
                //pnlLeftTools.Width = 300;
                btnHideLeftTools.Content = "<";                
            }
            else if (btnHideLeftTools.IsChecked == true)
            {
                pnlLeftTabs.Visibility = System.Windows.Visibility.Collapsed;
                //pnlLeftTools.Width = 30;
                btnHideLeftTools.Content = ">";                
            }            
        }

        private void btnHidRightTools_Click(object sender, RoutedEventArgs e)
        {
            if (btnHidRightTools.IsChecked == false)
            {
                pnlRightAccordionTools.Visibility = System.Windows.Visibility.Visible;
                //pnlRightTools.Width = 270;
                btnHidRightTools.Content = ">";
            }
            else if (btnHidRightTools.IsChecked == true)
            {
                pnlRightAccordionTools.Visibility = System.Windows.Visibility.Collapsed;
                //pnlRightTools.Width = 30;
                btnHidRightTools.Content = "<";
            }
        }

        /// <summary>
        /// TODO: Should be handled in HWConfigurationPresenter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnSetDefaults_Click(object sender, RoutedEventArgs e)
        //{
        //    HWModuleController.ShowSetDefaults();
        //}

        #endregion

        #region Left Region handlers

        #region HWConfigurationTree handlers

        private void treeHWConfiguration_Loaded(object sender, RoutedEventArgs e)
        {           
             treeHWConfiguration.ExpandAll();

             if (treeHWConfiguration.Items != null && treeHWConfiguration.Items.Count > 0)
                 _vm.CurrentLink = treeHWConfiguration.Items[0]; //=treeHWConfiguration.RootNode                    

            _vm.HWRecursiveObjectsList = 
            DataService.TreeHWConfigRecursiveNodesList = this.treeHWConfiguration.RecursiveObjectsList; //cashed data
        }

        private void treeHWConfiguration_SelectedItemChanged(object sender, IDenticard.WpfUI.GenericTreeView.RoutedTreeItemEventArgs<IDenticard.Access.Common.LinkNode> e)
        { 
            //1. Update related views if previous node and selected do not belong to one site    
            _vm.CurrentLink = e.NewItem as IDenticard.Access.Common.LinkNode;           

            //2. Refresh HW tree context menu
            //_vm.RefreshAllViews(selectedNode, false);   
            //RefreshHWTreeContextMenu(selectedNode); - in OnNavigationLinkChanged      
        }

        /// <summary>
        /// Handle event after navigation link changed (mainly in tree)- fource navigation in toolbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigationSiteChanged(object sender, EventArgs e)
        {     
            /*
            if (_vm != null)// && _vm.CurrentSiteID != null)
            {
                //1.HWTree context menu
                //RefreshHWTreeContextMenu(_vm.CurrentLink); -  in OnNavigationLinkChanged

                //2.Central Filterable grid - has its own handlers

                //3.Right region toolbox
                cmbSite.SelectedValue = _vm.CurrentSiteID;
            }
            */
        }

        private void OnNavigationLinkChanged(object sender, EventArgs e)
        {
            //Split the next code on 3 diffrent views
            if (_vm != null)// && _vm.CurrentSiteID != null)
            {
                //1.HWTree context menu
                RefreshHWTreeContextMenu(_vm.CurrentLink);

                //3.Right region toolbox
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
        }

        #endregion

        #endregion

        #region Right Region - accordion toolbox handlers
     
        /// <summary>
        /// The list of all controllers of the current site
        /// </summary>
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
                ControllersList.Filter =  (selectedSiteID == -1) ? null :
                                        new Predicate<object>(delegate(object item)
                                        {
                                            if (item == null) return false;
                                            var controller = item as IDenticard.Premisys.SCP;
                                            return controller != null && controller.SITE_ID != null ? controller.SITE_ID == selectedSiteID : false;
                                        });

            //REPLACED direct DAL methods with late binding
            //ObjectDataProvider orderProvider = this.FindResource("dsControllers") as ObjectDataProvider;
            //orderProvider.MethodParameters[0] = selectedSiteID;                         
        }

        private void btnAddSite_Click(object sender, RoutedEventArgs e)
        {
            _currentAccessObject = null;
            //treeHWConfiguration.Items[0].AccessObjectLink            
            //var allTreeLinks = treeHWConfiguration.RecursiveObjectsList;

            var parentCollectionLink = _vm.GetParentBOCollection<Site>(0);            
            _vm.ExecuteCommand<Site>(ApplicationCommands.New, parentCollectionLink);

            //TODO UI part: Should be updated form ViewModel on CurrentLinkNode changed event
            cmbSite.ItemsSource = _vm.Sites; //Should be refresed automatially
            cmbSite.SelectedValue = _vm.CurrentSiteID;
        } 
        
        private void btnAddControllerOfTheType_Click(object sender, RoutedEventArgs e)
        {
            var parentSite = cmbSite.SelectedItem as IDenticard.Premisys.Site;
            _currentAccessObject = parentSite;

            if (_currentAccessObject == null)
            {
                MessageBox.Show("Please, select a site before before a new controller will be created.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            //var node = parentSite.Node;           
            //TODO: Find command parameter - parent SCPCollection to add a new controller
            int site_id = parentSite.SITE_ID;
            var parentCollectionLink = _vm.GetParentBOCollection<SCP>(site_id);

            //Call a command to add a new controller
            _vm.ExecuteCommand<SCP>(ApplicationCommands.New, parentCollectionLink /*, site_id, allTreeLinks,*/);

            //TODO UI part: Should be updated form ViewModel on CurrentLinkNode changed event
            cmbController.ItemsSource = DataService.GetSiteControllers(_vm.CurrentSiteID);
            cmbController.SelectedValue = _vm.CurrentLink.Id;//= created controller id
        }

        private void btnAddIOBoard_Click(object sender, RoutedEventArgs e)
        {
            var parentController = cmbController.SelectedItem as IDenticard.Premisys.SCP;
            _currentAccessObject = parentController;
            if (_currentAccessObject == null)
            {
                MessageBox.Show("Please, select a controller before a new IOBoard will be created.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }          
            
            //TODO: Find command parameter - parent SIOCollection to add a new ioboard
            int scp_id = parentController.SCP_ID;
            var ioboardsCollectionLink = _vm.GetParentBOCollection<SIO>(scp_id);

            //Call a command to add a new ioboard
            _vm.ExecuteCommand<SIO>(ApplicationCommands.New, ioboardsCollectionLink);               
        }       

        private void btnBuildCnfg_Click(object sender, RoutedEventArgs e)
        {
            var parentController = cmbController.SelectedItem as IDenticard.Premisys.SCP;

            if (parentController == null)
            {
                MessageBox.Show("Please, select a controller before a new IOBoard will be created.", "Warning", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }
          
            //TODO: Find command parameters 
            int scp_id = parentController.SCP_ID;
            var ioboardsCollectionLink = _vm.GetParentBOCollection<Door>(scp_id);//parent SIOCollection to add a new ioboard           
            var sioType = (SIOType)cmbIOBoardType.SelectedValue;  // Type of new io boards          
            int siosCount = (int)upDoorsCount.Value; // count of ioboards

            //HWDoorsConfiguration generation = new HWDoorsConfiguration(ioboardsCollectionLink, sioType, siosCount);
            _vm.ExecuteCommand<Door>(UIPrototype.Commands.HWConfigCommands.GenerateDoors, ioboardsCollectionLink, sioType, siosCount);               
        }

        #endregion

        #endregion
    }
}
