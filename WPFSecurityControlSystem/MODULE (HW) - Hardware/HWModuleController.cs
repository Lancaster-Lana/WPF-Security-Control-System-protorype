using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Input;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Controls;
using WPFSecurityControlSystem.Services;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using WPFSecurityControlSystem.Commands;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration
{
    /// <summary>
    /// The application controller class for HWConfigrationModule (interaction logic of views from diffrent regions) (like as IDenticard.AccessUI.ConfigHW class in  WinForms version of PremiSys).
    /// Purpose:
    /// - handling events\executing HWConfiguration commands from toolbar(s), the main work area;
    /// - navigation logic between views in the main region, navigation region and toolbox
    /// - interaction HWConfigrationModule window with external modules\services
    /// </summary>
    public class HWModuleController //: ConfigHW
    {
        #region Static Content

        static ViewFactory _viewsFactory;

        /// <summary>
        /// Init HW Module views in static constructor
        /// </summary>
        static HWModuleController()
        {
            var viewsTypes = new List<Type>()
                            {
                                typeof(SetDefaultPropertiesControl), // NOTICE - mark exported controls with Export["ViewName"]
                                typeof(SitePropertiesControl),
                                typeof(SCPPropertiesControl),
                                typeof(SIOPropertiesControl),
                                typeof(DoorPropertiesControl)
                            };

            var container = new CompositionContainer(new TypeCatalog(viewsTypes));

            _viewsFactory = new ViewFactory(container);             
        }

        public static UserControl GetView(string viewName)
        {
            try
            {
                return _viewsFactory.GetView(viewName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public static UserControl GetView(Type viewType)
        {
            try
            {
                return _viewsFactory.GetView(viewType.Name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }     

        #region Common Methods

        public static bool? AddHWElementDialog(AccessBOCollection parentCollection, ref object entity)
        {
            UserControl contentControl = (entity != null)
                                        ? GetView(entity.GetType().FullName)
                                        : GetView(parentCollection.ChildName);

            //Add object of the collection temporary            
            //entity = parentEntity.AddChild() as AccessBO
            BasePropertiesDialog dlgAddEntity = new BasePropertiesDialog(parentCollection, contentControl);
            dlgAddEntity.Title = "Add a new " + parentCollection.ChildName;
            dlgAddEntity.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            bool? dialResult = dlgAddEntity.ShowDialog();
            if (dialResult == true)
            {
                //Set result to the referenced entity
                entity = dlgAddEntity.Data;

                var bentity = entity as AccessBO;
                if (bentity != null)
                    MessageBox.Show("The '" + bentity.Name + "' created !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (entity is AccessBO)
                parentCollection.RemoveChild((AccessBO)entity);

            return dialResult;
        }

        public static bool? ShowHWElementDialog(AccessBO entity)
        {
            if (entity == null)
                return false; //TODO:

            UserControl contentControl = GetView(entity.GetType().FullName);//FullName;
            BasePropertiesDialog dlgEditProperties = new BasePropertiesDialog(entity, contentControl);//ViewType.Controller
            dlgEditProperties.Title = entity.Name;
            dlgEditProperties.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? dialResult = dlgEditProperties.ShowDialog();
            if (dialResult == true)
            {
                if (entity != null && entity is AccessBO)
                    MessageBox.Show("The " + ((AccessBO)entity).Name + " saved !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            return dialResult;
        }

        public bool? ShowSetDefaults()
        {
            var contentControl = GetView("SetDefaults") as BasePropertiesControl;//ViewType.SetDefaults.ToString()); 
            BasePropertiesDialog dlgAddSite = new BasePropertiesDialog(contentControl);
            dlgAddSite.Title = "Set Defaults";
            //dlgAddSite.View.Content = new SitePropertiesControl();
            dlgAddSite.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            return dlgAddSite.ShowDialog();
        }

        #endregion

        #endregion

        #region Class properties

        HWConfigurationViewModel DataContext { get; set; }
        HWConfigurationShell CurrentView { get; set; }

        private HWConfigCommands _commands = null;
        internal HWConfigCommands Commands
        {
            get
            {
                if (_commands == null)
                    _commands = new HWConfigCommands(this);
                return _commands;
            }
        }

        #endregion     

        #region Class Constructor

        public HWModuleController(HWConfigurationShell shellView /*HWConfigurationViewModel context,*/):base()
        {
            this.CurrentView = shellView;

            this.DataContext = shellView.DataContext as HWConfigurationViewModel;                         
        }

        #endregion

        #region Commands Methods

        public void Download(object controllerObject)
        {
            var controller = controllerObject as SCP;
            if (controller != null)
            {
                //Commands.DownloadCommand.Execute(controller);
                //controller.DownloadRequired = true;
                DownloadLog log = new DownloadLog(DownloadLogLevel.detail);
                SCP.DownloadPanel(controller.SITE_ID, controller.Channel.Channel_ID, controller.SCP_ID, true, true, true, log);
            }
        }

        public void EvaluateCanCreate(object sender, CanExecuteRoutedEventArgs e)
        {
            //Get the processed item
            var item = this.DataContext.CurrentLink;//this.MainView.SelectedTreeLink;

            e.CanExecute = item != null && item.IsCollection; //Only to the folders can be added items
            e.Handled = true;
        }          

        /// <summary>
        /// Creates a sub category for the clicked item
        /// and refreshes the tree.
        /// </summary>
        public void AddHardwareElement(object sender, ExecutedRoutedEventArgs e)
        {
            //string commandParamenter = Convert.ToString(e.Parameter);
            var parentNode = this.DataContext.CurrentLink; //TODO: this.MainView.SelectedTreeLink;

            var parentCollection = DataService.GetObjectsCollection(parentNode);
            dynamic newHWEntity = null;

            //var viewType = HWConfigurationUIService.GetItemTypeByParentFolder(parentCollection.ChildName);//(ViewType)Enum.Parse(typeof(ViewType), commandParamenter);
           
            //Create a new HW element
            if (AddHWElementDialog(parentCollection, ref newHWEntity) == true) //ConfigHW.PerformAddClick(newHWEntity); //add to TreeView as LinkNode
            {
                //1. Create entity in DB                
                // DataService.Create(bentity); - but bentity already created     
                var bentity = newHWEntity as AccessBO;
                DataService.Update(bentity);
                                
                //2. Navigate and expand new treenode
                var newNode = bentity.Link;                            
                Refresh(parentNode, newNode, true);
               
                //The event as handled
                e.Handled = true;
            }
        }

        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        public void EvaluateCanEdit(object sender, CanExecuteRoutedEventArgs e)
        {
            //get the processed item
            var item = this.DataContext.CurrentLink;

            e.CanExecute = item != null && !item.IsCollection;//IsFolder;
            e.Handled = true;
        }

        public void EditHardwareElement(object sender, ExecutedRoutedEventArgs e)
        {
            var bentity = DataService.GetObject(this.DataContext.CurrentLink);//this.MainView.SelectedTreeLink); 

            if (ShowHWElementDialog(bentity) == true)
            {
                if (bentity == null)
                {
                    MessageBox.Show("A new tree node is not created - empty data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var currentLink = this.DataContext.CurrentLink;

                //1. DAL refresh : link node information(name and tag) from saved business object
                DataService.Update(currentLink, bentity);
               
                //2. rest of changes: refreshing dat in all views
                this.Refresh(currentLink.Parent, currentLink, false);                                              
            }
        }

        /// <summary>
        /// Checks whether it is allowed to delete a category, which is only
        /// allowed for nested categories, but not the root items.
        /// </summary>
        public void EvaluateCanDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            //get the processed item
            var item = this.DataContext.CurrentLink;

            e.CanExecute = item != null && !item.IsCollection && item.Parent != null;        
            e.Handled = true;
        }

        /// <summary>
        /// Deletes the currently processed item. This can be a right-clicked
        /// item (context menu) or the currently selected item, if the user
        /// pressed delete.
        /// </summary>        
        public void DeleteHardwareElement(object sender, ExecutedRoutedEventArgs e)
        {
            //get item
            var item = this.DataContext.CurrentLink;//this.MainView.SelectedTreeLink;

            //remove from parent
            if (item != null)

                if (MessageBox.Show("Are you sure to delete " + item.Name + " ?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    //1.DAL : destroy object
                    LinkNode parentNode = item.Parent;
                    LinkNode prevNavigated;
                    DataService.DeleteLinkNode(item, out prevNavigated);

                    //2. Refresh data to be displayed in UI (related views) with navigation to previous node
                    Refresh(parentNode, prevNavigated, true);

                    //mark event as handled
                    e.Handled = true;
                }
        }

        public List<LinkNode> GenerateDoorsForController(HWDoorsConfiguration configurationInformation)//AccessBOCollection siosCollection, int doorsCount, int doorType)
        {
            //int count = configurationInformation.Count;
            //var ioType = configurationInformation.SIOBoardType;
            //var parentCollection = configurationInformation.IOBoardsCollection;

            //Create doors
            List<LinkNode> generatedLinks = DataService.GenerateDoors(configurationInformation);
            return generatedLinks;
        }

        #endregion
        
        #region  UI methods
        
        /// <summary>
        /// Refresh data for all views after add, change, remove HWConfiguration element
        /// Navigate and expand the suitable node
        /// </summary>
        public void Refresh(LinkNode parentNode, LinkNode navigationNode, bool fullRefresh)
        {
            //Refresh data
            this.DataContext.RefreshAllData(parentNode, navigationNode, fullRefresh);
            
            //Refresh UI - all views
            this.CurrentView.RefreshUI(parentNode, navigationNode, fullRefresh);
        }

        #endregion
    }
}