using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using IDenticard.AccessUI;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.DTO;
using WPFSecurityControlSystem.Services;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration.Views
{
    /// <summary>
    /// A base class for the main region (area) of HW configuration window: filterable points grid view
    /// </summary>
    public partial class MainContentsView : UserControl
    {
        #region Stub Properties

        /// <summary>
        /// Wrapper for visible columns of the GridView
        /// </summary>
        public List<InfoColumn> GridViewColumns
        {
            get
            {
                //var allColumns = SCP.Fields, 
                var visibleGridColumns = gridPoints.Columns
                                        .Where(c => c.Visibility == System.Windows.Visibility.Visible)
                                        .OrderBy(c => c.DisplayIndex);
                var assignedColumns = (from DataGridColumn column in visibleGridColumns
                                       select new InfoColumn
                                       {
                                           //ID = column.id,
                                           ID = column.SortMemberPath,
                                           Name = column.Header.ToString(),
                                           //Description = column.Header.ToString(),
                                           IsAssigned = true
                                       }).ToList();
                return assignedColumns;
            }
        }

        public List<InfoColumn> GridViewAllColumns
        {
            get
            {                                
                //var allColumns = SCP.Fields, 
                var allColumns = new List<InfoColumn>();
                allColumns.Add(new InfoColumn() { ID = "CommType", Name = "Comm Type"});//, IsAssigned =  GridViewColumns.Count(c => c.ID.Equals("CommType")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "ControllerType", Name = "Controller Type"});// , IsAssigned =  GridViewColumns.Count(c => c.ID.Equals("ControllerType")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "DefaultMode", Name = "Default Mode"});// , IsAssigned = GridViewColumns.Count(c => c.ID.Equals("DefaultMode")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "OfflineMode", Name = "Offline Mode" });// , IsAssigned = GridViewColumns.Count(c => c.ID.Equals("OfflineMode")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "Properties", Name = "Properties"});// , IsAssigned = GridViewColumns.Count(c => c.ID.Equals("Properties")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "TimeZone", Name = "Time Zone"});// , IsAssigned = GridViewColumns.Count(c => c.ID.Equals("TimeZone")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "SiteName", Name = "Site"});//, IsAssigned = GridViewColumns.Count(c => c.ID.Equals("SiteName")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "DoorName", Name = "Door/Point"});//, IsAssigned =  GridViewColumns.Count(c => c.ID.Equals("DoorName")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "IOBoardName", Name = "I/O Board"});// , IsAssigned = GridViewColumns.Count(c => c.ID.Equals("IOBoardName")) > 0 });
                allColumns.Add(new InfoColumn() { ID = "ControllerName", Name = "Controller"});//, IsAssigned =  GridViewColumns.Count(c => c.ID.Equals("ControllerName")) > 0 });

                var assignedIDs = GridViewColumns.Select(i => i.Name).ToList();
                var nonAssigned = allColumns.Where(c => !assignedIDs.Contains(c.Name));  //= allColumns.Intersect<InfoColumn>(GridViewColumns);
                foreach (InfoColumn nonAssignedColumn in nonAssigned)
                    nonAssignedColumn.IsAssigned = false;

                var allArranged = GridViewColumns.Union(nonAssigned).ToList();
                return allArranged;
            }
        }

        #endregion

        #region Variables & Properties Properties

        HWConfigurationViewModel _vm = null;

        /// <summary>
        /// Currently navigated site in the comnbo box
        /// </summary>        
        int _currentSiteID = -1;
        public int CurrentSiteID
        {
            get
            {                
                return _currentSiteID;                
            }
            set
            {
                _currentSiteID = value;
                cmbViewing.SelectedValue = _currentSiteID;

                //Refresh common ViewModel current site info
                if (_vm != null && _vm.CurrentSiteID != value)                
                    _vm.CurrentSiteID = value;
            }
        }
      
        protected System.ComponentModel.ICollectionView DataView
        {
            get
            {
                return System.Windows.Data.CollectionViewSource.GetDefaultView(gridPoints.ItemsSource);//(_vm.SiteDoorsData)
            }
            set 
            {             
               gridPoints.ItemsSource = value;
            }
        }

        #endregion

        #region Constructor

        public MainContentsView()
        {
            InitializeComponent();

            cmbViewing.SelectedValuePath = __Site.ColumnSiteID;
            cmbViewing.DisplayMemberPath = "Node.Name"; //TODO:
            //gridPoints.ItemsSource = - later binding
        }

        public MainContentsView(object context): this()
        {                        
            this.DataContext = _vm = context as HWConfigurationViewModel;
            _vm.NavigationSiteChanged += new EventHandler(NavigationSiteChanged);

            Refresh();            
        }

        #endregion

        #region Handlers

        private void btnAddColumn_Click(object sender, RoutedEventArgs e)
        {
            BasePropertiesDialog dialog = DialogsFactory.CreateColumnsPickerDialog(GridViewAllColumns, "View Columns");
            if (dialog.ShowDialog() == true)
            {
                //gridPoints.Columns.Clear();
                List<InfoColumn> allColumns = dialog.Data as List<InfoColumn>;
                List<InfoColumn>  assignedColumns = allColumns.Where(c => c.IsAssigned).ToList();
                if (assignedColumns != null)
                {
                    var exisingGridColumnsBindings = gridPoints.Columns.Select(col => ((DataGridHyperlinkColumn)col).Binding.ToString()).ToList();
                       
                    //Find cell style for search
                    Style searchStyle = (Style)Resources["DataGridCellStyleForSearch"];                  

                    foreach (InfoColumn column in allColumns)
                    {                        
                        int newIndex = assignedColumns.IndexOf(column);
                        var existingColumn = gridPoints.Columns.Where(c => c.Header.ToString() == column.Name).FirstOrDefault();
                        //1. If column is not added yet
                        if (existingColumn == null)
                        { 
                            if (column.IsAssigned)
                            {                           
                                DataGridHyperlinkColumn gridColumn = new DataGridHyperlinkColumn();
                                gridColumn.Binding = new Binding(column.Name);
                                gridColumn.ContentBinding = new Binding(column.ID);                                
                                gridColumn.Header = column.Name;
                                gridColumn.Width = 120; //Default width                                                    
                                gridPoints.Columns.Add(gridColumn); //Add columns to the grid                          
                                gridColumn.DisplayIndex = newIndex;

                                gridColumn.CellStyle = searchStyle;
                            }
                        }
                        else//2.Arrange already added columns
                        {
                            existingColumn.Visibility = column.IsAssigned ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
                            if (column.IsAssigned)
                            {
                                existingColumn.DisplayIndex = newIndex;
                                //existingColumn.CellStyle = searchStyle;
                            }
                        }
                       
                    }
                }
            }
        }

        private void cmbViewing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {                           
                //1. To support navigation in the HW tree and filtering in grid
                int siteSiteID = -1;
                if(cmbViewing.SelectedItem is IDenticard.Premisys.Site)
                    siteSiteID = ((IDenticard.Premisys.Site)cmbViewing.SelectedItem).SITE_ID;            
                                
                //Filter 
                if(DataView != null)
                    DataView.Filter = (siteSiteID == -1) ? null :
                                            new Predicate<object>(delegate(object item)
                                            {
                                                var site = item as HWBusinessObject;
                                                return site != null ? site.SiteID == siteSiteID : false;
                                            });            

                //2. To force navigation on the common ViewModel - refresh current its site info               
                if (_vm != null)                            
                    _vm.CurrentSiteID = Convert.ToInt32(cmbViewing.SelectedValue);
                    //_vm.CurrentLink = e.Source as IDenticard.Access.Common.LinkNode;                                
            
                //2. The following approach is replaced with (1) for the sake of late binding
                //ObjectDataProvider orderProvider = this.FindResource("dsDoorPointsData") as ObjectDataProvider;
                //string filterSite = Convert.ToString(cmbViewing.SelectedValue);
                //orderProvider.MethodParameters[0] = filterSite;

                e.Handled = true;
        }

        private void gridPoints_DoorLinkClick(object sender, RoutedEventArgs e)
        {
                var hyperLink = e.OriginalSource as Hyperlink;
                var currentItem = hyperLink.DataContext as HWBusinessObject;                
                int parentId = currentItem.ControllerID;
                int objectId = (int)currentItem.DoorID;

                OpenLink<Door>(hyperLink, parentId, objectId);
        }

        private void gridPoints_IOBoardLinkClick(object sender, RoutedEventArgs e)
        {
                var hyperLink = e.OriginalSource as Hyperlink;           
                var currentItem = hyperLink.DataContext as HWBusinessObject;
                int parentId = currentItem.ControllerID;
                int objectId = (int)currentItem.IOBoardID;

                OpenLink<SIO>(hyperLink, parentId, objectId);
        }

        private void gridPoints_ControllerLinkClick(object sender, RoutedEventArgs e)
        {
                var hyperLink = e.OriginalSource as Hyperlink;
                //hyperLink.CommandBindings[0].Command
                var currentItem = hyperLink.DataContext as HWBusinessObject;
                int parentId = currentItem.SiteID;
                int objectId = (int)currentItem.ControllerID;

                OpenLink<SCP>(hyperLink, parentId, objectId);
        }

        private void gridPoints_SiteLinkClick(object sender, RoutedEventArgs e)
        {
                var hyperLink = e.OriginalSource as Hyperlink;            
                var currentItem = hyperLink.DataContext as HWBusinessObject;
                int parentId = -1;
                int objectId = (int)currentItem.SiteID;

                OpenLink<Site>(hyperLink, parentId, objectId);
        }

        private void OpenLink<T>(Hyperlink hyperLink, int parentId, int objectId) where T : IDenticard.Access.Common.AccessBO
        {           
                var currentItem = hyperLink.DataContext as HWBusinessObject;
                var currentObj = DataService.GetNodeObject<T>(parentId, objectId);

                _vm.ExecuteCommand<T>(ApplicationCommands.Open, currentObj);
        }

        #endregion

        #region External events

        /// <summary>
        /// Changes should be made after site is changed in sigle view
        /// Navigate to the site in combo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigationSiteChanged(object sender, EventArgs e)
        {
            var siteID = Convert.ToInt32(sender);

            //Refresh grid data
            Refresh();

            //Navigate correspondant site in sites combobox
            CurrentSiteID = siteID;            
        }
  
        #endregion

        #region Methods

        /// <summary>
        /// Refresh data in all regions (child views): HWConfiguration tree, Points grid, filterable controls
        ///or Region[Central].Refresh()  
        /// </summary>
        public void Refresh()//IDenticard.Access.Common.LinkNode fromNode, IDenticard.Access.Common.LinkNode navigationNode, bool fullRefresh)
        {            
            // Refresh data in all related views (via changing of DataContext)   
            this.DataView = System.Windows.Data.CollectionViewSource.GetDefaultView(_vm.SiteDoorsData);                                                      
        }

        #endregion       
    }
}

/*
      private void ucSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ucSearch.ControlText.Length == 0)
            {
                cmbViewing_SelectionChanged(null, null);
                //DataView.Filter = null;
            }
            else
            {
                var filterString = Convert.ToString(ucSearch.ControlText).ToUpper();
                 bool contains = false;

                //1 Search way
                int countOfVisible = GridViewColumns.Count;
                for (int i = 0; i < countOfVisible; i++)
                 {
                     for (int j = 0; j < gridPoints.Items.Count; j++)
                     {

                         try
                         {

                             var cell = gridPoints.GetCell(i, j);
                             //if (((System.Windows.Controls.TextBlock)(cell.Content)).Text.ToLower().Contains(filterString))                                
                             cell.Background = new SolidColorBrush(Colors.Red);
                            // else
                            //     cell.Background = new SolidColorBrush(Colors.Transparent);
                             //cell.PersistId;

                             var row = (DataGridRow)gridPoints.ItemContainerGenerator.ContainerFromItem(gridPoints.Items[j]);
                             row.Background = Brushes.Red;

                             //cell.UpdateLayout();
                         }
                         catch 
                         {
                         }
                     }
                 }

                //gridPoints.Items.Refresh();
                               
                ////only visible columns search
                //foreach (var column in GridViewColumns)                                    

                //        DataView.Filter = delegate(object item)
                //        {
                                         
                //                        var listItem = (HWBusinessObject)item;
                //                        var type = typeof(HWBusinessObject);
                //                        object cellValue = type.GetField(column.Name).GetValue(item); 
                                       
                //                        //listItem[GridViewColumns]
                //                        var row = System.Linq.Expressions.Expression.Parameter(typeof(HWBusinessObject), "row");
                                        
                //                        var parameter = System.Linq.Expressions.Expression.PropertyOrField(row, column.ID);
                //                        //listItem[parameter.Member]

                //                        if (Convert.ToString(listItem.ControllerName).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.ControllerType).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.CommType).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.SiteName).ToUpper().Contains(filterString) ||
                //                            //Convert.ToString(listItem.TimeZone).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.DoorName).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.OfflineMode).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.DefaultMode).ToUpper().Contains(filterString) ||
                //                            Convert.ToString(listItem.IOBoardName).ToUpper().Contains(filterString))
                //                        {
                //                            contains = true;
                //                        }

                //                        return contains;
                //       };      
             
                };          

           DataView.Refresh();
        }
*/