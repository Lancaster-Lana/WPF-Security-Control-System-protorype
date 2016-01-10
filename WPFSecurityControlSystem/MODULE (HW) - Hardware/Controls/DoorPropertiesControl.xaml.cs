using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Services;
using WPFSecurityControlSystem.Utils;
using WPFSecurityControlSystem.Base;

namespace WPFSecurityControlSystem.Controls
{
    //[Export(typeof(Door))]
    [Export("Door")]
    public partial class DoorPropertiesControl 
    {

        #region Contructor

        public DoorPropertiesControl()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();

            ErrorProvider.RegisterValidator(txtName);

            ErrorProvider.RegisterValidator(cmbReader);
            ErrorProvider.RegisterValidator(cmbReaderConfiguration);
            ErrorProvider.RegisterValidator(cmbPairedReader);

            ErrorProvider.RegisterValidator(cmbReaderMode);
        }

        /// <summary>
        /// Load combo controls and default values
        /// </summary>
        protected override void LoadFilterableControls()
        {
            base.LoadFilterableControls();

            try
            {
                //Fill Reader Tab data                       
                cmbReader.ItemsSource = DataService.GetDoorReaders(Entity);
                cmbReader.DisplayMemberPath = __Node.ColumnName;
                cmbReader.SelectedValuePath = __Reader.ColumnReaderID;
                cmbReader.SelectedValue = Entity.ReaderId;

                cmbPairedReader.ItemsSource = DataService.GetPairedReaders(Entity);
                cmbPairedReader.DisplayMemberPath = __Node.ColumnName;
                cmbPairedReader.SelectedValuePath = __AccessControlReader.ColumnACRID;
                cmbPairedReader.SelectedValue = Entity.PairedAcrId;

                cmbReaderConfiguration.ItemsSource = DataService.GetReaderConfigurations();
                cmbReaderConfiguration.DisplayMemberPath = __ReaderConfiguration.ColumnName;
                cmbReaderConfiguration.SelectedValuePath = __ReaderConfiguration.ColumnReaderCfgID;
                cmbReaderConfiguration.SelectedValue = Entity.ReaderConfigurationId;
                //UpdateDialogForConfiguration(selectedConfiguration);

                //Modes section
                cmbReaderMode.ItemsSource = DataService.AcrModes;
                cmbReaderMode.DisplayMemberPath = ___Global.ColumnName;//__AcrMode.ColumnName;
                cmbReaderMode.SelectedValuePath = "AcrMode_ID"; //__AcrMode.ColumnAcrModeID;
                cmbReaderMode.SelectedValue = Entity.DefaultModeId;

                cmbLEDMode.ItemsSource = IDenticard.LookupData.LedModes;
                cmbLEDMode.DisplayMemberPath = "Name";
                cmbLEDMode.SelectedValuePath = "LEDMode_ID";
                cmbLEDMode.SelectedValue = Entity.LedModeId;

                // OfflineMode - filter on valid values.
                cmbOfflineMode.ItemsSource = DataService.AcrOfflineModes;
                cmbOfflineMode.DisplayMemberPath = "Name";
                cmbOfflineMode.SelectedValuePath = "AcrMode_ID";
                cmbOfflineMode.SelectedValue = Entity.OfflineModeId;

                //Alternate Reader section
                cmbAltReader.ItemsSource = DataService.GetDoorReaders(Entity);                   
                cmbAltReader.DisplayMemberPath = __Node.ColumnName;
                cmbAltReader.SelectedValuePath = __Reader.ColumnReaderID;
                cmbAltReader.SelectedValue = Entity.AlternateReaderId;

                cmbAltReaderConfiguration.ItemsSource = DataService.GetReaderAltConfigurations();  //TODO:                         
                cmbAltReaderConfiguration.DisplayMemberPath = __AltReaderConfiguration.ColumnName;
                cmbAltReaderConfiguration.SelectedValuePath = __AltReaderConfiguration.ColumnAltReaderCfgID;
                cmbAltReaderConfiguration.SelectedValue = Entity.AlternateReaderConfigurationId;

                //Card Formats section                            
                // SCPCardFormatsView - only assigned and default Card Formats
                gridCardFormats.ItemsSource = DataService.GetSCPCardFormatsFilterableView(true); //- binding exists AS ItemsSource="{Binding SCPCardFormatsView}" 

                //DefaultReader.AcrControlFlags
                gridControlFlags.ItemsSource = DataService.AcrControlFlags;

                //2. Door configuration tab
                //Relay Configuration
                //--------------                   
                cmbTimeZoneForUnlock.ItemsSource = DataService.GetTimeZones();
                cmbTimeZoneForUnlock.DisplayMemberPath = "Node.Name";//__TimeZone.ColumnName;
                cmbTimeZoneForUnlock.SelectedValuePath = __TimeZone.ColumnTimeZoneID;
                cmbTimeZoneForUnlock.SelectedValue = Entity.AssociatedTimeZoneID; //TODO:               

                //Strike
                //var  tempds = Lookup.GetAvailablePoints(Entity.ScpId, 1, Entity.StrikeOutputId, true);
                cmbOuput.DisplayMemberPath = __Node.ColumnName;
                cmbOuput.SelectedValuePath = __OutputPoint.ColumnOutputID;
                cmbOuput.ItemsSource = DataService.GetAvailablePoints(Entity.ScpId, 1, Entity.StrikeOutputId, true); 
                cmbOuput.SelectedValue = Entity.StrikeOutputId;      
                //Strike Mode
                cmbStrikeMode.DisplayMemberPath = __StrikeMode.ColumnName;
                cmbStrikeMode.SelectedValuePath = __StrikeMode.ColumnStrikeModeID;
                cmbStrikeMode.ItemsSource = DataService.GetStrikeModeDataView(); 
                cmbStrikeMode.SelectedValue = Entity.StrikeModeId;

                //Contact configuration                              
                cmbInput.DisplayMemberPath = __Node.ColumnName;
                cmbInput.SelectedValuePath = __InputPoint.ColumnInputID;
                cmbInput.ItemsSource = DataService.GetAvailablePoints(Entity.ScpId, 0, Entity.DoorInputId, true);                   
                cmbInput.SelectedValue = Entity.DoorInputId;

                //REX configuration                               
                cmbPrimaryREX.DisplayMemberPath = __Node.ColumnName;
                cmbPrimaryREX.SelectedValuePath = __InputPoint.ColumnInputID;
                cmbPrimaryREX.ItemsSource = DataService.GetAvailablePoints(Entity.ScpId, 0, Entity.Rex0InputId, true);
                cmbPrimaryREX.SelectedValue = Entity.Rex0InputId;
               
                cmbAlternativeREX.DisplayMemberPath = __Node.ColumnName;
                cmbAlternativeREX.SelectedValuePath = __InputPoint.ColumnInputID;
                cmbAlternativeREX.ItemsSource = DataService.GetAvailablePoints(Entity.ScpId, 0, Entity.Rex1InputId, false);
                cmbAlternativeREX.SelectedValue = Entity.Rex1InputId;

                //Antipassback TAB
                //------------------
                var tempds = Lookup.GetLookUp(__AntipassbackMode.SPReadAntipassbackMode);
                cmbAntipassbackMode.DisplayMemberPath = __AntipassbackMode.ColumnName;
                cmbAntipassbackMode.SelectedValuePath = __AntipassbackMode.ColumnAntiPassbackModeID;
                cmbAntipassbackMode.ItemsSource = tempds.Tables[0].DefaultView;
                cmbAntipassbackMode.SelectedValue = Entity.AntiPassbackModeId;

                // Create filter if necessary
                // Populate the areas to choose from for moving from.
                tempds = Lookup.GetAccessAreas();

                var areaInView = tempds.Tables[0].DefaultView;
                var additionalFilter = String.Empty;
                var controllerLinkNode = Entity.Link.Parent.Parent; //TODO:
                var controller = (SCP)controllerLinkNode.AccessObjectLink;
                var controllerType = (ScpType)controller.SCPType;
                var isEpController = !(controllerType == ScpType.SCP_2 || controllerType == ScpType.SCP_C || controllerType == ScpType.SCP_E);

                if (!isEpController)
                {
                    additionalFilter = String.Format("{0} <> 1 AND {0} <> 3", __AccessArea.ColumnFlags);
                    areaInView = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(tempds.Tables[0],
                        __AccessArea.ColumnAccessAreaID, typeof(AccessArea).ToString(), additionalFilter);
                }
                else
                {
                    areaInView = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(tempds.Tables[0],
                        __AccessArea.ColumnAccessAreaID, typeof(AccessArea).ToString());
                }
                cmbAreaIn.DisplayMemberPath = __Node.ColumnName;
                cmbAreaIn.SelectedValuePath = __AccessArea.ColumnAccessAreaID;
                cmbAreaIn.ItemsSource = areaInView;
                cmbAreaIn.SelectedValue = Entity.AntiPassbackAreaInId;

                // Create filter if necessary
                var areaToView = tempds.Tables[0].DefaultView;
                if (!isEpController)
                {
                    areaToView = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(tempds.Tables[0],
                                    __AccessArea.ColumnAccessAreaID, typeof(AccessArea).ToString(), additionalFilter);
                }
                else
                {
                    areaToView = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(tempds.Tables[0],
                                    __AccessArea.ColumnAccessAreaID, typeof(AccessArea).ToString());
                }
                cmbAreaTo.DisplayMemberPath = __Node.ColumnName;
                cmbAreaTo.SelectedValuePath = __AccessArea.ColumnAccessAreaID;
                cmbAreaTo.ItemsSource = areaToView;
                cmbAreaTo.SelectedValue = Entity.AntiPassbackAreaToId;

            }
            catch (Exception ex)
            { 
                //TODO:
            }
        }

        public override void LoadProperties(Door entity)
        {
            base.LoadProperties(entity);            

            if (Entity == null) return; //TODO:

            //1. Load Security
            entity.ReadSecurity();
            ctrlPermissions.LoadSecurity(entity);

            //2. Fill common  properties
            txtName.Text = entity.Name;
            txtDescription.Text = entity.Description;            
            
            //3. Fill other controls

          }

        public override void SaveProperties()
        {
            //Save permissions

            ctrlPermissions.SaveSecurity();
            //1.Read controls properties
            if (Entity != null)
            {                
                //Common 
                Entity.Name = txtName.Text;
                Entity.Description = txtDescription.Text;

                //2.1.Get Reader Tab data               
                Entity.ReaderId = Convert.ToInt32(cmbReader.SelectedValue);
                Entity.PairedAcrId = Convert.ToInt32(cmbPairedReader.SelectedValue);
                Entity.ReaderConfigurationId = Convert.ToInt32(cmbReaderConfiguration.SelectedValue);
                Entity.DefaultModeId = Convert.ToInt32(cmbReaderMode.SelectedValue);
                Entity.LedModeId = Convert.ToInt32(cmbLEDMode.SelectedValue);
                Entity.OfflineModeId = Convert.ToInt32(cmbOfflineMode.SelectedValue);
                Entity.AlternateReaderId = Convert.ToInt32(cmbAltReader.SelectedValue);    
                Entity.AlternateReaderConfigurationId = Convert.ToInt32(cmbAltReaderConfiguration.SelectedValue);                                  

                //2. Door configuration tab
                Entity.AssociatedTimeZoneID = Convert.ToInt16(cmbTimeZoneForUnlock.SelectedValue); //TODO:
                Entity.StrikeOutputId = Convert.ToInt32(cmbOuput.SelectedValue);
                Entity.StrikeModeId = Convert.ToInt32(cmbStrikeMode.SelectedValue);
                Entity.DoorInputId = Convert.ToInt32(cmbInput.SelectedValue);
                Entity.Rex0InputId = Convert.ToInt32(cmbPrimaryREX.SelectedValue);
                Entity.Rex1InputId = Convert.ToInt32(cmbAlternativeREX.SelectedValue);

                //3. Antipassback TAB
                Entity.AntiPassbackModeId = Convert.ToInt32(cmbAntipassbackMode.SelectedValue);
                Entity.AntiPassbackAreaInId = Convert.ToInt32(cmbAreaIn.SelectedValue);
                Entity.AntiPassbackAreaToId = Convert.ToInt32(cmbAreaTo.SelectedValue);                

            }          
            base.SaveProperties(); //Entity.Save(); //to DB context                      
        }

        #endregion

        #region Handlers

        private void cmbReaderConfiguration_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblPairedReader.Visibility = cmbPairedReader.Visibility
                                = (Convert.ToInt32(cmbReaderConfiguration.SelectedValue) == (int)AccessReaderConfigModes.Master) //Master = 1
                                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden ;
        }    

        #endregion
    }
}
