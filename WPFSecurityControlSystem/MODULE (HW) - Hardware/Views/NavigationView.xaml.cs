using System;
using System.Windows;
using System.Windows.Controls;
using Controls.WpfUI.GenericTreeView;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Services;
using WPFSecurityControlSystem.Common;
using IDenticard.Access.Common;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration.Views
{
    public sealed partial class NavigationView : BasePropertiesControl, INavigationView
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
                return treeHWConfiguration.SelectedNode; //OR treeHWConfiguration.GetTreeNode(SelectedTreeLink); 
            }
        }

        #endregion

        #region Constructor

        public NavigationView()
        {
            InitializeComponent();

            treeHWConfiguration.SelectedItemChanged += treeHWConfiguration_SelectedItemChanged;            
        }

        public NavigationView(object context)
            : this()
        {
            this.DataContext = _vm = context as HWConfigurationViewModel;  //Attach shell(common) context for all regions
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

            treeDevices.ItemsSource = context.PluginsItems;
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

        #region HWConfigurationTree handlers

        private void treeHWConfiguration_Loaded(object sender, RoutedEventArgs e)
        {
            treeHWConfiguration.ExpandAll();

            if (treeHWConfiguration.Items != null && treeHWConfiguration.Items.Count > 0)
                _vm.CurrentLink = treeHWConfiguration.Items[0]; //=treeHWConfiguration.RootNode                    

            _vm.HWRecursiveObjectsList =
            DataService.TreeHWConfigRecursiveNodesList = this.treeHWConfiguration.RecursiveObjectsList; //cashed data
        }

        private void treeHWConfiguration_SelectedItemChanged(object sender, RoutedTreeItemEventArgs<LinkNode> e)
        {
            //1. Force update related views if previous node and selected do not belong to one site    
            _vm.CurrentLink = e.NewItem as LinkNode;

            // refresh HWConfig tree context menu
            RefreshHWTreeContextMenu(_vm.CurrentLink); // in OnNavigationLinkChanged             
        }
      
        /// <summary>
        ///The handler for external navigation event: after navigation link changed (in the MainGrid View)
        ///fource navigation in HWConfig Tree
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnNavigationSiteChanged(object sender, EventArgs e)
        {           
            //Navigate and refresh HWConfig Tree current node  
            if (_vm != null && this.treeHWConfiguration.SelectedItem != _vm.CurrentLink)
            {
                //this.Refresh(this.treeHWConfiguration.SelectedItem, _vm.CurrentLink);                            
                this.treeHWConfiguration.Navigate(_vm.CurrentLink);

                //refresh HWTree context menu
                RefreshHWTreeContextMenu(_vm.CurrentLink); //  in OnNavigationLinkChanged  
            }
        }

        /// <summary>
        /// Refresh data in all regions (child views): HWConfiguration tree, Points grid, filterable controls
        /// </summary>
        public void Refresh(LinkNode fromNode, LinkNode navigationNode)//, bool fullRefresh)
        {
            //Region[Left].Refresh()
            //1. TODO: Update node data in HW Tree
            RefreshAndNavigateTreeNode(fromNode, navigationNode); //- Region[RightNavigation].Refresh() 

            //refresh HWTree context menu
            RefreshHWTreeContextMenu(_vm.CurrentLink); //  in OnNavigationLinkChanged                        
        }

        #endregion

    }
}
