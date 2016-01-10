using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using IDenticard.Common.Wpf;
using WPFSecurityControlSystem.Services;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration
{
    /// <summary>
    /// Shell ViewModel: plays the role of shared context (or shared service)
    /// for inter communication between all regions and navigation logic
    /// </summary>
    public class HWConfigurationViewModel : NotifyPropertyChangedBase//  ObservableCollection<HWBusinessObject>
    {

        #region Static Variables & Properties

        static ObservableCollection<IDenticard.Premisys.Site> _sites = null;
        static List<IDenticard.Premisys.AccessControlReader> _accessControlReaders = null;
        static List<IDenticard.Premisys.SCP> _controllers = null;
        static List<IDenticard.Premisys.SIO> _ioBoards = null;

        #endregion

        #region Static Methods

        /// <summary>
        /// Get  Points\Doors data for GridView
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<HWBusinessObject> GetSiteDoorsData(int siteID)
        {
            try
            {
                _sites = new ObservableCollection<IDenticard.Premisys.Site>(DataService.Sites); // IDenticard.Premisys.Site.Enumerate();
                _controllers = DataService.Controllers;
                _accessControlReaders = DataService.AccessControlReaders;
                _ioBoards = DataService.SIOBoards;
                //_doors = DataService.Doors;

                var arcFullData = (from site in _sites
                                   join scp in _controllers on site.SITE_ID equals scp.SITE_ID //into sc
                                   //from site_scp in sc.DefaultIfEmpty()
                                   join sio in _ioBoards on scp.SCP_ID equals sio.SCP_ID  //sio.SIO_ID equals arc.Reader_SIO_ID  
                                   join arc in _accessControlReaders on scp.SCP_ID equals arc.SCP_ID  //door, elevator                                                                                                         
                                   select new HWBusinessObject
                                   {
                                       SiteID = site.SITE_ID,
                                       SiteName = site.Node.Name,
                                       ControllerID = scp.SCP_ID,
                                       ControllerName = scp.Node.Name,
                                       CommType = GetCommTypeFriendlyNameForSontroller(scp),
                                       ControllerType = GetScpTypeFriendlyName(scp.SCPType_ID),
                                       DefaultMode = GetARCModeFriendlyName(arc.Default_Mode_ID),//Offline_Mode_ID,
                                       OfflineMode = GetARCModeFriendlyName(arc.Offline_Mode_ID),

                                       IOBoardID = sio.SIO_ID,//arc.Door_SIO_ID, 
                                       IOBoardName = (sio != null && sio.Node != null) ? sio.Node.Name : "",

                                       TimeZone = GetTimeZoneFriendlyName(arc.AssociatedTimeZoneID),

                                       DoorID = arc.Id,//.Door_Input_ID,//TODO:
                                       DoorName = (arc != null && arc.Node != null) ? arc.Node.Name : "",
                                       //arc.ACR_ID == sio.                                                                              
                                       //IDenticard.Premisys.AccessControlReader.FindDoorTimeZoneID(arc.ACR_ID, scp.SCP_ID);
                                       //TODO: scp.TimeZoneMask,                                                                                            
                                   }).Distinct();

                if (siteID > -1)
                    arcFullData = arcFullData.Where(s => s.SiteID == siteID);
                return new ObservableCollection<HWBusinessObject>(arcFullData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("DB Connection error !" + ex.Message);
            }

            return null;
        }

        private static string GetTimeZoneFriendlyName(short? timeZoneId)//(AccessReaderModes)modeId;  
        {
            var timeZones = DataService.GetTimeZones();
            string name = (from timeZone in timeZones
                           where timeZone.Id == timeZoneId
                           select timeZone.Node.Name).FirstOrDefault();
            return name;
        }

        private static string GetARCModeFriendlyName(short? modeId)//(AccessReaderModes)modeId;  
        {
            string name = (from mode in DataService.AcrModes
                           where mode.AcrMode_ID == modeId
                           select mode.Name).FirstOrDefault();
            return name;
        }

        private static string GetScpTypeFriendlyName(short? scpTypeId)//(AccessReaderModes)modeId;  
        {
            //ScpType type = scpTypeId != null ? (ScpType)scpTypeId : 0;

            string name = (from mode in DataService.ScpTypes
                           where mode.SCPType_ID == scpTypeId
                           select mode.Name).FirstOrDefault();
            return name;
        }

        private static string GetCommTypeFriendlyNameForSontroller(IDenticard.Premisys.SCP scp)//(AccessReaderModes)modeId;  
        {
            //CommType type = commTypeId != null ? (CommType)commTypeId : 0;
            Channel parentChannel = new Channel(scp.Channel_ID, (int)scp.SITE_ID, false);
            int commTypeId = parentChannel.CommType_ID;

            string name = (from mode in DataService.CommTypes
                           where mode.CommType_ID == commTypeId
                           select mode.Name).FirstOrDefault();
            return name;
        }

        #endregion

        #region HWConfiguration class members

        #region Events

        /// <summary>
        /// Handling navigation event from external source for each child view
        /// </summary>
        public event EventHandler NavigationLinkChanged;

        /// <summary>
        /// Handling change current site event from external source for each child view
        /// </summary>
        public event EventHandler NavigationSiteChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Current site to be navigated in all areas after it is changed in one view, for example, in the main area
        /// </summary>
        int _siteId = -1;
        public int CurrentSiteID
        {
            get
            {
                return _siteId;
            }
            set
            {
                if (_siteId == value) return;

                _siteId = value;
                OnPropertyChanged("CurrentSiteID");

                //Update current link               
                if (DataService.TreeHWConfigRecursiveNodesList != null)
                {
                    var siteNodeLink = DataService.TreeHWConfigRecursiveNodesList.Where(n => n.Id == _siteId && n.NamespaceId == 1).FirstOrDefault();
                    if (CurrentLink != siteNodeLink)
                        CurrentLink = siteNodeLink;
                }

                //TODO: Navigate to this site link in any region view wherever: central, right sites combo boxes
                if (NavigationSiteChanged != null)
                    NavigationSiteChanged(_siteId, null);
            }
        }

        /// <summary>
        /// Current linkNode of active view (causes navigation, refreshing in another areas views)
        /// </summary>
        LinkNode _currentLink = null;
        public LinkNode CurrentLink
        {
            get
            {
                return _currentLink;
            }
            set
            {
                if (_currentLink == value) return;
                _currentLink = value;
                OnPropertyChanged("CurrentLink");

                //After add, remove, change
                //RefreshAllViews(CurrentLink, false);

                //In child views changing current link event can be handled in its own way
                NavigateCommand.Execute(_currentLink);
            }
        }

        public AccessBO CurrentObject
        {
            get
            {
                if (CurrentLink != null)
                    return DataService.GetObject(CurrentLink);
                return null;
            }
        }

        public ObservableCollection<IDenticard.Premisys.Site> Sites
        {
            get
            {
                //if (_sites == null)
                //    _sites = DataService.GetSites(); 
                return _sites;
            }
            set
            {
                _sites = value;
                OnPropertyChanged("Sites");
            }
        }

        #region Properties for the left region (TREEs: Hardware, Globals, Access Settings,  Plug-in)

        ObservableCollection<LinkNode> _hwTreeItems;
        public ObservableCollection<LinkNode> HWTreeItems
        {
            get
            {
                return _hwTreeItems;
            }
            set
            {
                _hwTreeItems = value;
                OnPropertyChanged("HWTreeItems");
            }
        }

        /// <summary>
        /// Retain HWConfigTree all links to business objects (after binding completed)
        /// </summary>
        public List<LinkNode> HWRecursiveObjectsList { get; set; }

        public List<LinkNode> AccessSettingItems { get; set; }
        public List<LinkNode> GLOBALItems { get; set; }

        public List<LinkNode> PluginsItems { get; set; }

        public void RefreshTreeData()
        {
            RefreshHWTreeData();

            GLOBALItems = DataService.GetTreeGlobalData();
            AccessSettingItems = DataService.GetTreeAccessSettingData();

            PluginsItems = DataService.GetTreePluginsData();
        }


        public void RefreshHWTreeData()
        {
            HWTreeItems = DataService.GetHWTreeData();
        }

        #endregion

        #region Data for the main (central) region - filterable GridView

        /// <summary>
        /// Get Points\Doors data for GridView
        /// </summary>
        /// <returns></returns>       
        ObservableCollection<HWBusinessObject> _siteDoorsData;
        public ObservableCollection<HWBusinessObject> SiteDoorsData
        {
            get
            {
                return _siteDoorsData;
            }
            set
            {
                _siteDoorsData = value;
                OnPropertyChanged("SiteDoorsData");
            }
        }

        #endregion

        #endregion

        #region UI Commands

        RelayCommand _navigateCommand;

        public ICommand NavigateCommand
        {
            get
            {
                if (_navigateCommand == null)
                    _navigateCommand = new RelayCommand(delegate
                    {
                        //1. Update current site link (after the active link is changed in one of views of any region)
                        var currentSite = DataService.FindParentSiteFromChildLink(_currentLink);
                        if (currentSite != null && CurrentSiteID != currentSite.SITE_ID)
                            CurrentSiteID = currentSite.SITE_ID;

                        //2. Fire event for external navigation if active link of one views is changed
                        if (NavigationLinkChanged != null)
                            NavigationLinkChanged(this, EventArgs.Empty);
                    });
                return _navigateCommand;
            }
        }

        #endregion

        #region Constructor

        public HWConfigurationViewModel()
        {
            // Commands = new HWConfigurationCommands(this);
            InitData();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Init data for Tree and Grid Views
        /// </summary>
        public void InitData()
        {
            RefreshSitesData();
            RefreshTreeData();
            RefreshGridData();
        }

        /// <summary>
        /// Refresh data after add, remove, change linknode in one of the regions
        /// if node changed, then only navigate the nodein other views (fullRefresh = true)
        /// </summary>
        /// <param name="navigatationNode"></param>
        public void RefreshAllData(LinkNode parentNode, LinkNode navigatationNode, bool fullRefresh)
        {
            //1.Refresh data in all views            
            if (fullRefresh)
            {
                RefreshSitesData();//Sites = DataService.GetSites(); //Get all sites after delete, add only
                RefreshHWTreeData();
                RefreshGridData();//CurrentSiteID);    
                RefreshRightToolbarData();
            }

            //2. Force UI Update and navigation to the current link in another views
            CurrentLink = navigatationNode;
        }

        private void RefreshSitesData()
        {
            Sites = new ObservableCollection<IDenticard.Premisys.Site>(DataService.GetSites());
        }

        /// <summary>
        /// Fill grid with all data by all sites
        /// </summary>
        private void RefreshGridData()
        {
            try
            {
                SiteDoorsData = GetSiteDoorsData(-1);
            }
            catch
            {
                //TODO:
            }
        }

        private void RefreshRightToolbarData()
        {
            //TODO:
            //DataService.GetSiteControllers(CurrentSiteID);
        }

        public void ExecuteCommand<T>(ICommand command, params object[] commandParams /*int? parentNodeId, List<LinkNode> allExistingLinks,*/ )
            where T : AccessBO
        {
            dynamic entityParameter = null;
            LinkNode navigationNode = null;

            if (command == ApplicationCommands.New || command == WPFSecurityControlSystem.Commands.HWConfigCommands.GenerateDoors)
            {
                //For a new command parameter '0' = LinkNode to parent collection
                if (commandParams == null || commandParams.Count() == 0) return;//NO PARAMETER PASSED

                var parentCollectionLink = commandParams[0] as LinkNode;
                AccessBOCollection parentCollection = DataService.GetObjectsCollection(parentCollectionLink); //DataService.GetObjectsCollection<T>(allExistingLinks, Convert.ToInt32(parentNodeId));

                if (parentCollection != null)
                {
                    var subType = commandParams.Count() > 1 ? commandParams[1] : -1;//the second paramer is a type of Business Object

                    if (command == ApplicationCommands.New)
                        entityParameter = parentCollection.AddChild(); //only one child will be added                   

                    else if (command == WPFSecurityControlSystem.Commands.HWConfigCommands.GenerateDoors)
                    {
                        var doorCollection = parentCollection;
                        var sioType = (SIOType)subType;
                        var doorsCount = Convert.ToInt32(commandParams[2]);

                        entityParameter = new HWDoorsConfiguration(doorCollection, sioType, doorsCount);
                    }

                    navigationNode = parentCollection.Link;
                }
            }
            else if (command == ApplicationCommands.Open) //Edit
            {
                //For an edit command parameter '0' = LinkNode to the edited object
                if (commandParams != null && commandParams.Count() > 0 && commandParams[0] is AccessBO)
                {
                    entityParameter = commandParams[0] as AccessBO;

                    //TODO: navidate current node in TreeView to know where editable node located
                    navigationNode = entityParameter.Link;
                }
            }
            else //TODO
            {

            }
            //1. Execute command (with DataServices, bo generation)
            command.Execute(entityParameter);

            //TODO: navigate current node in TreeView to know where a new TreeNode will be created
            CurrentLink = navigationNode;

            //2. Refresh Data
            RefreshAllData(null, CurrentLink, true);
        }

        /// <summary>
        /// Returns the link to the collection of T type (search result in HWConfiguration tree)
        /// </summary>
        /// <typeparam name="T">child collection objects type</typeparam>
        /// <param name="parentNodeId">Node having collection with objects of T type </param>
        /// <returns></returns>
        internal LinkNode GetParentBOCollection<T>(int parentNodeId) where T : AccessBO
        {
            var allTreeLinkNodes = this.HWRecursiveObjectsList;//treeHWConfiguration.HWRecursiveObjectsList; //all HW Tree nodes links to business objects
            List<LinkNode> subCollections = allTreeLinkNodes.Where(subLink => subLink.IsCollection && subLink.Id == parentNodeId).ToList();

            //Find suitable subCollection link having children of T type
            LinkNode parentCollectionLink = subCollections.Where(subCollection => subCollection.UiId.StartsWith(typeof(T).FullName)).FirstOrDefault();

            return parentCollectionLink;
        }

        #endregion

        #endregion
    }
}
