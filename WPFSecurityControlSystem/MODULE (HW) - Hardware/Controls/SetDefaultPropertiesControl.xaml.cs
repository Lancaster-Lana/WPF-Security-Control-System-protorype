using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Data;
using IDenticard.AccessUI;
using IDenticard.Common.DBConstant;
using IDenticard.Premisys;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Common;
using WPFSecurityControlSystem.Services;
using WPFSecurityControlSystem.DTO;
using WPFSecurityControlSystem.Utils;
using Channel = IDenticard.AccessUI.Channel;
using SCP = IDenticard.AccessUI.SCP;


namespace WPFSecurityControlSystem.Controls
{
    [Export("SetDefaults")]
    public sealed partial class SetDefaultPropertiesControl : BaseSingleControl<ID_HardwareTemplateReadResult>
    {
        #region Variables & Properties

        /// <summary>
        /// DefaultSettings for controller, reader
        /// </summary>
        //public ID_HardwareTemplateReadResult DefaultSettingsEntity
        //{
        //    get
        //    {
        //        return base.Data as ID_HardwareTemplateReadResult;
        //    }
        //}
        
        protected SCP DefaultController { get; set;}

        protected Channel DefaultChannel
        {
            get
            {
                if (DefaultController == null) return null;

                var controller = new SCP(DefaultController.SCP_ID);
                return controller.Channel;
            }
        }

        protected int DefaultCommType
        {
            get
            {
                if (DefaultController == null) return -1;  
                return DefaultChannel.CommType_ID;
            }
            set
            {
                if (DefaultController == null) return;               
                DefaultChannel.CommType_ID = value;
            }
        }

        //protected SIO DefaultIOBoard { get; set; }       

        protected ACR DefaultReader { get; set; }

        protected Door DefaultDoor
        {
            get
            {
                return DefaultReader as Door;
            }
        }

        List<InfoColumn> _allSCPCardFormatsWrappedList;

        ///// <summary>
        ///// Card Formats should be dispalyed in the dialog table
        ///// </summary>
        //ObservableCollection<InfoColumn> _scpCardFormatsView;
        //public ObservableCollection<InfoColumn> SCPCardFormatsView
        //{
        //    get
        //    {
        //        return _scpCardFormatsView;
        //    }
        //    set
        //    {                
        //        _scpCardFormatsView = value;
        //        //_allSCPCardFormatsWrappedList - refresh draft values
        //        OnPropertyChanged("SCPCardFormatsView");
        //    }
        //}

        //public List<InfoColumn> ACRControlFlagsView { get; set; }

        public ListCollectionView AcrControlFlagsView 
        {
            get
            {
                return CollectionViewSource.GetDefaultView(gridControlFlags.ItemsSource) as ListCollectionView;
            }
        }

        #endregion

        #region Constructor

        public SetDefaultPropertiesControl()
            : base()
        {
            InitializeComponent();
        }           

        #endregion
  
        #region Overrides

        /// <summary>
        /// parent_id = Node_ID of an object, which settings assigned as default  
        /// </summary>
        /// <param name="parent_id"></param>
        internal protected override void CreateNew(int parent_id) //parent_id == namespace_id
        {
            //base.CreateNew(parent_id); - should nnt be used here
            if (this.Data == null)
            {
                var newDefaultSettings = new IDenticard.Premisys.ID_HardwareTemplateReadResult();
                newDefaultSettings.NameSpace_ID = (short)parent_id;

                this.Data = newDefaultSettings;

                //DefaultSettingsEntity.Node_ID = (short?)parent_id;

                //entity.NameSpace_ID = ?
                //entity.SioTypeFlag = ?
                //HardwareTemplate.ClearDefault(            
            }           
        }

        //protected internal override ID_HardwareTemplateReadResult GetById(int id)
        //{
        //    return HardwareTemplate.Read((short)id, null); //sioTypeFlag
        //}

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();
            //Register validators - or ValidationFactory.CreateValidator
            ErrorProvider.RegisterValidator(cmbCommType, ValidationFormat.Required);

            ErrorProvider.RegisterValidator(cmbScpType, ValidationFormat.Required);
            ErrorProvider.RegisterValidator(cmbTimeZone, ValidationFormat.Required);
            ErrorProvider.RegisterValidator(cmbCardDatabase, ValidationFormat.Required);
            ErrorProvider.RegisterValidator(cmbIOBautRate, ValidationFormat.Required);

            ErrorProvider.RegisterValidator(cmbReaderMode, ValidationFormat.Required); 
            ErrorProvider.RegisterValidator(cmbReaderConfiguration, ValidationFormat.Required);             
        }       

        /// <summary>
        /// Load combo controls and default values
        /// </summary>
        protected override void LoadFilterableControls()
        {
            try
            {
                base.LoadFilterableControls();
            
                //cmbCommType.ItemsSource = DataSourceHelper.GetScpTypes();
                //cmbCommType.DisplayMemberPath = __CommType.ColumnName;
                //cmbCommType.SelectedValuePath = __CommType.ColumnCommTypeID;

                cmbTimeZone.ItemsSource = DataService.GetGMTRegion();//Enum.GetNames(typeof(TimezoneModes));
                cmbTimeZone.DisplayMemberPath = __GMTRegion.ColumnDescription;
                cmbTimeZone.SelectedValuePath = __GMTRegion.ColumnGMTID;

                cmbTimeZoneForUnlock.ItemsSource = DataService.GetTimeZones();//Enum.GetNames(typeof(TimezoneModes));
                cmbTimeZoneForUnlock.DisplayMemberPath = "Node.Name";//__TimeZone.ColumnName;
                cmbTimeZoneForUnlock.SelectedValuePath = __TimeZone.ColumnTimeZoneID;

                cmbCardDatabase.ItemsSource = DataService.GetCardDababases();
                cmbCardDatabase.DisplayMemberPath = __CardDatabase.ColumnName;
                cmbCardDatabase.SelectedValuePath = __CardDatabase.ColumnCardDatabaseID;

                cmbScpType.ItemsSource = DataService.GetScpTypes();
                cmbScpType.DisplayMemberPath = __SCPType.ColumnName;
                cmbScpType.SelectedValuePath = __SCPType.ColumnSCPTypeID;

                cmbIOBautRate.ItemsSource = DataService.GetBautRates();
                cmbIOBautRate.DisplayMemberPath = __BaudRate.ColumnName;
                cmbIOBautRate.SelectedValuePath = __BaudRate.ColumnBaudRateID;

                cmbReaderMode.ItemsSource = DataService.AcrModes;
                cmbReaderMode.DisplayMemberPath = ___Global.ColumnName;//__AcrMode.ColumnName;
                cmbReaderMode.SelectedValuePath = "AcrMode_ID"; //__AcrMode.ColumnAcrModeID;
               
                cmbReaderProtocol.ItemsSource = DataService.GetCardFormatProtocols();
                cmbReaderProtocol.DisplayMemberPath = __ReaderCardDataFormatFlag.ColumnName;
                cmbReaderProtocol.SelectedValuePath = __CardFormatFunction.ColumnFunctionID;// "Function_ID";//__ReaderCardDataFormatFlag.ColumnFormatFlag;

                cmbReaderConfiguration.ItemsSource = DataService.GetReaderConfigurations();
                cmbReaderConfiguration.DisplayMemberPath = __ReaderConfiguration.ColumnName;
                cmbReaderConfiguration.SelectedValuePath = __ReaderConfiguration.ColumnReaderCfgID;            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override void LoadProperties(IDenticard.Premisys.ID_HardwareTemplateReadResult entity)
        {            
            //var template = entity as ID_HardwareTemplateReadResult;
            base.LoadProperties(entity);
            //this.DataContext = this; //TODO: create VM for SetDefauls view 

            try
            {
                //1. Reading default setting for all: Controller, Reader, IOBoard                        
                short SCP_NameSpace_ID = 5;
                var defaultSCPNode = IDenticard.Premisys.HardwareTemplate.Read(SCP_NameSpace_ID, null);
                if (defaultSCPNode != null && defaultSCPNode.Node_ID != null)
                    DefaultController = new SCP((int)defaultSCPNode.Node_ID);// SCP.Read((int)defaultSCPNode.Node_ID); //not assign            
                else
                    DefaultController = null;

                //ID_HardwareTemplateReadResult defaultChannelNode = HardwareTemplate.Read(3, null);
                //if (defaultChannelNode != null)
                //    DefaultChannel = Channel.Read((short)defaultChannelNode.Node_ID); //not assign    

                short ACR_NameSpace_ID = 70;
                var defaultACRNode = HardwareTemplate.Read(ACR_NameSpace_ID, null);//(int)IDenticard.Premisys.SioTypeFlagValue.Unknown);
                if (defaultACRNode != null && defaultACRNode.Node_ID != null)
                    DefaultReader = new Door((int)defaultACRNode.Node_ID, DefaultController);//IDenticard.Premisys.AccessControlReader.Read((int)defaultACRNode.Node_ID); //not assign     
                else
                    DefaultReader = null;

                //var defaultDoorNode = IDenticard.Premisys.HardwareTemplate.Read(ACR_NameSpace_ID, (int)DefaultController.Node_ID);
                //if (defaultDoorNode != null)
                //     DefaultDoor = new Door((int)defaultDoorNode.Node_ID, null);

                // var defaultSIONode = IDenticard.Premisys.HardwareTemplate.Read(7, (int)IDenticard.Premisys.SioTypeFlagValue.Unknown);
                // if (defaultSIONode != null)
                //     DefaultIOBoard= new SIO((int)defaultSIONode.Node_ID);//IDenticard.Premisys.SIO.Read((short)defaultSIONode.Node_ID); //not assign    

                //2. The first load - set default values                    
                if (DefaultController == null)
                {
                    cmbCommType.SelectedIndex = 0;//TODO: Network by default

                    cmbScpType.SelectedValue = Constants.DEFAULT_ScpType;
                    cmbCardDatabase.SelectedValue = Constants.DEFAULT_CardDatabase; //first
                    cmbIOBautRate.SelectedValue = Constants.DEFAULT_BaudRate;// 38400;            
                    txtIOTimeout.Text = Constants.DEFAULT_IOTimeout.ToString();
                    cmbTimeZone.SelectedValue = Constants.DEFAULT_GMTRegion;
                    cmbTimeZoneForUnlock.SelectedValue = 0; //TODO:  IDenticard.AccessUI.TimeZone.GetSystemGeneratedTimeZone(0)
                }
                else
                {
                    //Read default values
                    //var channel = DefaultChannel;//Channel.Read(DefaultController.Channel_ID);
                    //if (channel != null)
                    //    cmbCommType.SelectedIndex =  channel.CommType_ID == (short)IDenticard.Access.Common.CommType.NetworkOut ? 0 : 1;
                    cmbCommType.SelectedIndex = DefaultCommType == (short)IDenticard.Access.Common.CommType.NetworkOut ? 0 : 1;

                    cmbScpType.SelectedValue = DefaultController.SCPType;
                    cmbCardDatabase.SelectedValue = DefaultController.CardDatabase_ID; //first
                    cmbIOBautRate.SelectedValue = DefaultController.MSPBaudRate;// TODO: 38400;            
                    txtIOTimeout.Text = DefaultController.MSPReplyTimeOut.ToString(); //TODO:
                    cmbTimeZone.SelectedValue = DefaultController.GMT_ID;
                    cmbTimeZoneForUnlock.SelectedValue = DefaultController.DSTID; //TODO: 
                }

                if (DefaultReader == null)
                {
                    cmbReaderMode.SelectedValue = Constants.DEFAULT_ReaderMode;// AccessReaderModes.CardOnly;
                    cmbReaderProtocol.SelectedValue = Constants.DEFAULT_Protocol; //CardFormats.Wiegand; 
                    cmbReaderConfiguration.SelectedValue = Constants.DEFAULT_ReaderConfiguration;

                    //Door settings
                    cmbTimeZoneForUnlock.SelectedIndex = 0;
                    ckbIsFirstUnlock.IsChecked = false;
                }
                else
                {
                    //Read default values
                    cmbReaderMode.SelectedValue = DefaultReader.DefaultModeId;// AccessReaderModes.CardOnly;
                    cmbReaderProtocol.SelectedValue = DefaultReader.CardDataFormatFlag; //CardFormats.Wiegand;                 
                    cmbReaderConfiguration.SelectedValue = DefaultReader.ReaderConfigurationId;

                    cmbTimeZoneForUnlock.SelectedValue = DefaultDoor.AssociatedTimeZoneID;//DefaultReader.AssociatedTimeZoneID; TODO: 
                    ckbIsFirstUnlock.IsChecked = DefaultDoor.IsFirstCardUnlock;
                }

                //Fill reader ControlFlags - assigned and not
                //ACRControlFlagsView = DataService.GetACRControlFlagsView(DefaultReader);//DataService.AcrControlFlags;   
                gridControlFlags.ItemsSource = DataService.GetACRControlFlagsView(DefaultReader);                  
           
                //List with all possible card formats - for cardformats selector
                //if (_MaxCardFormats == -1)
                //    _MaxCardFormats = Lookup.ReadMaxCount(IDenticard.Access.Common.Constant.MaxCardFormats);
                //ACR.LoadCardDataMasks(out _CardholderAdditionIndices, out _AssetAdditionIndices, out _CardholderCancelIndices, out _AssetCancelIndices, _MaxCardFormats);
                //if (DefaultReader != null)                                    
                //    LoadAcrData(DefaultReader);                
                
                _allSCPCardFormatsWrappedList = DefaultReader != null
                                            ? DataService.GetSCPCardFormatsWrappedList(DefaultReader.ScpId)//TODO: DefaultController.SCPCardFormats();
                                            : DataService.GetSCPCardFormatsWrappedList(-1);
                //SCPCardFormatsView - only assigned and default                
                //SCPCardFormatsView = DataService.GetSCPCardFormatsFilterableView(_allSCPCardFormatsWrappedList, true); //Get the controller CardFormats filterable data to be displayed
                gridCardFormats.ItemsSource = DataService.GetSCPCardFormatsFilterableView(_allSCPCardFormatsWrappedList, true); //Get the controller CardFormats filterable data to be displayed;   // binding exists AS ItemsSource="{Binding SCPCardFormatsView}" 
            }
            catch (Exception ex)
            { 
                //TODO:
                MessageBox.Show(ex.Message);
            }
        }
   
        public override void SaveProperties()
        {
            //Read and save default controller properties
            if(DefaultController == null)
                DefaultController = DataService.CreateDefaultController(Parent_ID);                        
            DefaultController.SCPType = Convert.ToInt16(cmbScpType.SelectedValue);
            DefaultController.CardDatabase_ID = Convert.ToInt16(cmbCardDatabase.SelectedValue);
            DefaultController.GMT_ID = Convert.ToInt32(cmbTimeZone.SelectedValue);
            DefaultController.MSPReplyTimeOut = Convert.ToInt16(txtIOTimeout.Text);
            DefaultController.MSPBaudRate = Convert.ToInt32(cmbIOBautRate.SelectedValue);                       
            DefaultController.Update();  //TODO: save

            //Read and save default Channel settigs
            DefaultChannel.CommType_ID = cmbCommType.SelectedIndex == 0
                                        ? (short)IDenticard.Access.Common.CommType.NetworkOut
                                        : (short)IDenticard.Access.Common.CommType.Serial;
            DefaultChannel.Update();

            //Read and save default Reader settigs
            if (DefaultReader == null)
                 DefaultReader = DataService.CreateDefaultDoor(DefaultController.Node_ID);                              
            DefaultReader.DefaultModeId = Convert.ToInt16(cmbReaderMode.SelectedValue);
            DefaultReader.CardDataFormatFlag = Convert.ToInt16(cmbReaderProtocol.SelectedValue); //CardFormats.Wiegand;                 
            DefaultReader.ReaderConfigurationId = Convert.ToInt16(cmbReaderConfiguration.SelectedValue);            

            //TODO: from old (WinForm code): Get assigned Contol Flags
            DefaultReader.AcrControlFlags = GetControlFlags();
            //TODO: from old (WinForm code): Get CardFormats for SCP            
            DataService.ReassignPanelCardFormats(DefaultReader, _allSCPCardFormatsWrappedList);

            DefaultReader.Update();
            //DefaultReader.SetDefault();

            //IDenticard.Premisys.SCP controller = IDenticard.Premisys.SCP.Read((short)DefaultReader.ScpId);
            //var realScpCardFormats = controller.SCPCardFormats().Where(cf=> cf.SCP_ID == (short)DefaultDoor.ScpId).ToList();
            //controller.Update();

            DefaultDoor.AssociatedTimeZoneID = Convert.ToInt16(cmbTimeZoneForUnlock.SelectedValue);
            DefaultDoor.IsFirstCardUnlock = ckbIsFirstUnlock.IsChecked == true;
            DefaultDoor.Update();
            //DefaultDoor.SetDefault();

            //Save\set defaults:
            //Default Controller
            IDenticard.Premisys.HardwareTemplate.SetDefault(DefaultController.NameSpaceID, (short)DefaultController.Node_ID, null);
            //Default Channel
            IDenticard.Premisys.HardwareTemplate.SetDefault(3/*DefaultChannel.NameSpaceID*/, (short)DefaultChannel.Channel_ID, null);
            //Default Reader\Door
            IDenticard.Premisys.HardwareTemplate.SetDefault(DefaultDoor.NameSpaceID, (short)DefaultDoor.Node_ID, null);
            
            base.SaveProperties(); // inish saving DB context                                
        }

        #endregion

        #region CardFormats and Control Flags Business Logic

        /// <summary>
        /// The hashtable mapping acr id to its current card data format flag.
        /// </summary>
        private Hashtable _MapAcrIdToCardDataFormatFlag;

        /// <summary>
        /// The maximum number of card formats.
        /// </summary>
        private int _MaxCardFormats = -1;

        /// <summary>
        /// This array holds the mask associated with the card data format index for cardholders.
        /// </summary>
        private ACR.CardFormatMask[] _CardholderAdditionIndices;

        /// <summary>
        /// This array holds the mask associated with the card data format index for assets.
        /// </summary>
        private ACR.CardFormatMask[] _AssetAdditionIndices;

        /// <summary>
        /// This array holds the cancellation mask associated with the card data format index for cardholders.
        /// </summary>
        private ACR.CardFormatCancelMask[] _CardholderCancelIndices;

        /// <summary>
        /// This array holds the cancellation mask associated with the card data format index for assets.
        /// </summary>
        private ACR.CardFormatCancelMask[] _AssetCancelIndices;

        /// <summary>
        ///  TODO: Get assigned Contol Flags (as in old WinForm code)
        /// </summary>
        /// <returns></returns>
        private int GetControlFlags()
        {
            int newMask = 0;
            foreach (InfoColumn item in AcrControlFlagsView) //gridControlFlags;
            {
                //IDenticard.Cache.AcrControlFlag flag = item.Tag as IDenticard.Cache.AcrControlFlag;
                //item.AcrControlFlag1 = item.ID
                if (item.IsAssigned)
                {
                    newMask = newMask | Convert.ToInt32(item.ID);//AcrControlFlag1);
                    if (item.ID == "1") newMask = newMask | 2;
                    newMask = newMask | 0x20;
                }
            }
            return newMask;
        }


        /// <summary>
        /// Loads all the ACR data.
        /// </summary>
        private void LoadAcrData(ACR reader)
        {
            // Get all ACR's
            if (_MapAcrIdToCardDataFormatFlag == null)
            {
                _MapAcrIdToCardDataFormatFlag = new Hashtable();
            }
            else
            {
                _MapAcrIdToCardDataFormatFlag.Clear();
            }

            DataSet dsAcrs = Lookup.GetPanelAcrs(reader.ScpId);
            if (dsAcrs.Tables.Count > 0)
            {
                foreach (DataRow rowCurrent in dsAcrs.Tables[0].Rows)
                {
                    // Get the ACR id.
                    int acrId = Convert.ToInt32(rowCurrent[__AccessControlReader.ColumnACRID]);

                    // Get the card format flag.
                    int cardFormatFlag = ACR.ConvertFromDatabaseInt16(Convert.ToInt16(rowCurrent[__AccessControlReader.ColumnCardDataFormatFlag]));

                    // Add this to the hashtable.
                    _MapAcrIdToCardDataFormatFlag.Add(acrId, cardFormatFlag);
                }
            }
        }

        ///// <summary>
        ///// Method determines if a card format should be assigned to the panel.
        ///// </summary>
        //private void ReassignPanelCardFormats(ACR reader)
        //{
        //    int scpId = reader.ScpId;
        //    List<SCPCardFormat> refreshedCardFormatsList = new List<SCPCardFormat>();

        //    // Assign CardFormats being associated with this ACR if it hasn't been already.
        //    var scpAssignedCardFormats = IDenticard.Premisys.SCP.SCPCardFormats((short)reader.ScpId);            

        //    foreach (InfoColumn item in _allSCPCardFormatsWrappedList)
        //    {
        //        var cardFormatId = Convert.ToInt16(item.ID);                                
        //        var cardFormatAssignedToScp = scpAssignedCardFormats.Where(f => f.CardFormat_ID == cardFormatId).FirstOrDefault();
        //        int mercuryIndex = cardFormatAssignedToScp != null ? Convert.ToInt32(cardFormatAssignedToScp.Mercury_CardFormat_Index) : -1;

        //        SCPCardFormat cardFormatToUpdate = DataService.AllCardFormatsList.Where(cf => cf.CardFormatId == cardFormatId).FirstOrDefault();
        //        cardFormatToUpdate.MercuryIndex = mercuryIndex;
        //        cardFormatToUpdate.ScpId = scpId; 
        //        //OR SCPCardFormat cardFormatToUpdate = new SCPCardFormat(scpId, cardFormatId, mercuryIndex, , );
        //        // Assign\remove the format                       
        //        var updatedCardFormat = SetSCPCardFormatStatus(cardFormatToUpdate, item.IsAssigned);//DefaultReader.ScpId, cardFormatId, mercuryIndex, item.IsAssigned);  

        //        //ACR.UpdateCardFormat(scpId, cardFormatId, updatedCardFormat.MercuryIndex);           
        //    }            
        //}

        //private SCPCardFormat SetSCPCardFormatStatus(SCPCardFormat cardFormatInfo, bool assigned)//int scpId, int cardFormatId, int mercuryIndex)
        //{
        //    if (/*scpCardFormat == null &&*/ assigned) //mean - it non- assigned, but shold be assigned
        //    {                              
        //        //cardFormatInfo.ScpId = scpId;      
        //        cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Checked;
        //        if (cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Unassigned)
        //            cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Assigned; 

        //        int mercuryIndexAssigned = ACR.AssignCardFormatToScp(cardFormatInfo);               
        //        cardFormatInfo.MercuryIndex = mercuryIndexAssigned;
        //    } 

        //    else//should be removed
        //    {                
        //        //bool inUse = false;

        //        //// Check if any ACR's are using this value.  If so, then add this to the number 
        //        //// of formats for this panel.
        //        //foreach (DictionaryEntry entryCurrent in _MapAcrIdToCardDataFormatFlag)
        //        //{
        //        //    int acrId = Convert.ToInt32(entryCurrent.Key);
        //        //    int cardFormat = Convert.ToInt32(entryCurrent.Value);
        //        //    if ((cardFormat != -1) && (acrId != DefaultReader.AcrId))
        //        //    {
        //        //        DateTime modifiedTime = DateTime.MinValue;
        //        //        if ((Convert.ToInt32(_CardholderAdditionIndices[cardholderFormat.MercuryIndex]) & cardFormat) ==
        //        //            Convert.ToInt32(_CardholderAdditionIndices[cardholderFormat.MercuryIndex]))
        //        //        {
        //        //            inUse = true;
        //        //            break;
        //        //        }

        //        //        if ((Convert.ToInt32(_AssetAdditionIndices[cardholderFormat.MercuryIndex]) & cardFormat) ==
        //        //            Convert.ToInt32(_AssetAdditionIndices[cardholderFormat.MercuryIndex]))
        //        //        {
        //        //            inUse = true;
        //        //            break;
        //        //        }
        //        //    }
        //        //}
                
               
        //        cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Unchecked;
        //        //if(cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Assigned)
        //        cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Unassigned;                
        //        if(cardFormatInfo != null)//&& !inUse)
        //            ACR.RemoveCardFormatFromScp(cardFormatInfo);
        //    }                      

        //    return cardFormatInfo;
        //}

        #endregion

        #region Handlers

        private void btnMoreFormats_Click(object sender, RoutedEventArgs e)
        {
            var scpDraftCardFormatsList = _allSCPCardFormatsWrappedList;
                                   // GetSCPCardFormatsFilterableView(gridCardFormats.ItemsSource, false).Cast<InfoColumn>();
                                   // CollectionViewSource.GetDefaultView(gridCardFormats.ItemsSource).Cast<InfoColumn>().ToList();

            BasePropertiesDialog dlg = DialogsFactory.CreateCardFormatsDialog(scpDraftCardFormatsList);//CreateColumnsPickerDialog(scpDraftCardFormatsList, "Card Formats");            
            if (dlg.ShowDialog() == true)
            {
                var refreshSCPCardFormatsList = dlg.Data as IEnumerable<InfoColumn>;//CardFormat

                //Assign new CardFormats
                 if (refreshSCPCardFormatsList != null)
                     //SCPCardFormatsView = DataService.GetSCPCardFormatsFilterableView(refreshSCPCardFormatsList, true);                     
                     //TODO: rebind is not nessesary
                     gridCardFormats.ItemsSource = DataService.GetSCPCardFormatsFilterableView(refreshSCPCardFormatsList, true); ;
                     //allCardFormats.Where(cardFormat => cardFormat.IsAssigned);
            }
        }        

        private void btnAddTimeZone_Click(object sender, RoutedEventArgs e)
        {
            var timeZone = DataService.CreateTimeZone() as IDenticard.AccessUI.TimeZone;
            if (timeZone != null) //refresh TimeZones dropdown
            {
                cmbTimeZoneForUnlock.ItemsSource = DataService.GetTimeZones();
                cmbTimeZoneForUnlock.SelectedValue = timeZone.Node_ID;
            }
            //OR
            //var timeZone = new IDenticard.AccessUI.TimeZone(timeZoneId, null);
            //var formTimeZone = new IDenticard.AccessUI.TimeZoneProperties(timeZone);
            //if (formTimeZone.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    timeZone.Update();
                //RefreshTimeZones(currentlySelectedTimeZone.TimeZone_ID);
            //}

            ////For Edit
            //var linkCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
            //var currentlySelectedTimeZone = (TimeZone)dataGridView1.SelectedCells[0].Tag;

            //var timeZone = (TimeZone)linkCell.Tag;

            //var dialog = new TimeZoneProperties(timeZone);

            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
            //    timeZone.Update();
            //    RefreshDataGrid(currentlySelectedTimeZone.TimeZone_ID);
            //}
        }
        
        #endregion

    }
}

/*
 IDenticard.Premisys
 
[Flags]
public enum SioTypeFlagValue
{
    Unknown = 0,
    OneReaderBoard = 1,
    InputBoard = 2,
    OutputBoard = 4,
    TwoReaderBoard = 8,
    TwoReaderController = 16,
    S9000Rio = 32,
    S90004Reader = 64,
    S90002Reader = 128,
    PoeOneDoorBoard = 256,
    PoeOneDoorController = 512,
    SchlagePim = 1024
}*/

/*
private void LoadCardDataFormatTabControls(ACR reader)
{
    // Clear the listviews.
    //_ListViewCardholderCardFormats.Items.Clear();
    //_ListViewAssetCardFormats.Items.Clear();

    // Populate the listviews.
    var availableFormats = new SCPCardFormat[0];
    var assignedFormats = new SCPCardFormat[0];

    // Load the card formats.
    var cardFormats = Lookup.GetCardFormats(reader.ScpId, 1);

    var cardFormatsView = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(cardFormats.Tables[0],
        __SCPCardFormat.ColumnDBID, typeof(CardFormat).ToString());

    if (cardFormats.Tables.Count > 0)
    {
        // Populate the list views
        foreach (DataRowView dataRowViewCurrent in SCPCardFormatsView)
        {
            // Get the values.
            var scpId = IDenticard.AppContext.One.Util.Param.GetInt(dataRowViewCurrent[__SCPCardFormat.ColumnSCPID]);
            var cardFormatId = IDenticard.AppContext.One.Util.Param.GetInt(dataRowViewCurrent[__SCPCardFormat.ColumnDBID]);
            var mercuryIndex = IDenticard.AppContext.One.Util.Param.GetInt(dataRowViewCurrent[__SCPCardFormat.ColumnMercuryIndex]);
            var name = Convert.ToString(dataRowViewCurrent[__Node.ColumnName]);
            var description = Convert.ToString(dataRowViewCurrent[__Node.ColumnDescription]);

            // Create the new object.
            var cardholderFormatNew = new SCPCardFormat(scpId, cardFormatId, mercuryIndex, name, description);
            var assetFormatNew = new SCPCardFormat(scpId, cardFormatId, mercuryIndex, name, description);

            // Add it to the cardholder list view.
            //var cardholderItem = new ListViewItem(cardholderFormatNew.Name);
            //cardholderItem.Tag = cardholderFormatNew;
            //_ListViewCardholderCardFormats.Items.Add(cardholderItem);

            // Add it to the asset list view.
            //var assetItem = new ListViewItem(assetFormatNew.Name);
            //assetItem.Tag = assetFormatNew;
            //_ListViewAssetCardFormats.Items.Add(assetItem);
        }

        // Highlight the assigned rows.
        cardFormatsView.RowFilter = String.Format("{0} > -1", __SCPCardFormat.ColumnSCPID);

        foreach (DataRowView rowCurrent in cardFormatsView)
        {
            // Get the values.
            var cardFormatId = IDenticard.AppContext.One.Util.Param.GetInt(rowCurrent[__SCPCardFormat.ColumnDBID]);

            // Create the new object.
            foreach (ListViewItem itemCurrent in _ListViewCardholderCardFormats.Items)
            {
                var formatCurrent = (SCPCardFormat)itemCurrent.Tag;
                if (formatCurrent.CardFormatId == cardFormatId)
                {
                    // Update the listview item's color
                    itemCurrent.SubItems[0].BackColor = Color.Goldenrod;
                    formatCurrent.AssignStatus = SCPCardFormat.AssignedStatus.Assigned;
                    break;
                }
            }

            // Create the new object.
            foreach (ListViewItem itemCurrent in _ListViewAssetCardFormats.Items)
            {
                var formatCurrent = (SCPCardFormat)itemCurrent.Tag;
                if (formatCurrent.CardFormatId == cardFormatId)
                {
                    // Update the listview item's color
                    itemCurrent.SubItems[0].BackColor = Color.Goldenrod;
                    formatCurrent.AssignStatus = SCPCardFormat.AssignedStatus.Assigned;
                    break;
                }
            }
        }

        // Refresh both list views.
        //_ListViewCardholderCardFormats.Refresh();
        //_ListViewAssetCardFormats.Refresh();
    }

    // Determine which checkboxes get checked.    
    if (reader.CardDataFormatFlag > 0)
    {
        // Loop through the items to check the color to see what their tag is.  
        foreach (ListViewItem itemCurrent in _ListViewAssetCardFormats.Items)
        {
            if (itemCurrent.BackColor == Color.Goldenrod)
            {
                // See if this should be checked.
                SCPCardFormat cardFormat = (SCPCardFormat)itemCurrent.Tag;
                if ((Convert.ToInt32(_AssetAdditionIndices[cardFormat.MercuryIndex]) & _AccessControlReader.CardDataFormatFlag) ==
                    Convert.ToInt32(_AssetAdditionIndices[cardFormat.MercuryIndex]))
                {
                    // Check this box.
                    itemCurrent.Checked = true;
                    cardFormat.CheckStatus = SCPCardFormat.CheckedStatus.Checked;
                }
            }
        }

        // Loop through the visible check boxes to see what their tag is.  
        foreach (ListViewItem itemCurrent in _ListViewCardholderCardFormats.Items)
        {
            if (itemCurrent.BackColor == Color.Goldenrod)
            {
                // See if this should be checked.
                SCPCardFormat cardFormat = (SCPCardFormat)itemCurrent.Tag;
                if ((Convert.ToInt32(_CardholderAdditionIndices[cardFormat.MercuryIndex]) & _AccessControlReader.CardDataFormatFlag) ==
                    Convert.ToInt32(_CardholderAdditionIndices[cardFormat.MercuryIndex]))
                {
                    // Check this box.
                    itemCurrent.Checked = true;
                    cardFormat.CheckStatus = SCPCardFormat.CheckedStatus.Checked;
                }
            }
        }
    }

    // Load the ACR's
    LoadAcrData();

    // Sort the formats.
    //_ListViewCardholderCardFormats.Sort();
    //_ListViewAssetCardFormats.Sort();
}*/

/*

        private void UnassignPanelCardFormats(ACR reader)
        {
            bool formatRemoved = false;

            // Loop through the card formats to see if any of the unchecked formats were currently assigned.
            // If so, and neither the cardholder nor the asset list view has the item checked, see if this
            // one can be removed from being associated with the panel.
            foreach (InfoColumn item in _allSCPCardFormatsWrappedList)
            {
                var cardFormatId = Convert.ToInt16(item.ID);

                // Get the card formats.
                SCPCardFormat cardFormatInfo = DataService.AllCardFormatsList.Where(cf => cf.CardFormatId == cardFormatId 
                        && cf.ScpId == reader.ScpId).FirstOrDefault();//new SCPCardFormat(DefaultController.SCP_ID, cardFormatId, mercuryIndex, name, description);

                //if (cardFormatInfo != null)
                //    ACR.RemoveCardFormatFromScp(cardFormatInfo);
               
                // Check if this format should add to the number of formats for this panel
                if (!item.IsAssigned)// && (cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Assigned))
                {
                    bool inUse = false;

                    // Check if any ACR's are using this value.  If so, then add this to the number 
                    // of formats for this panel.
                    foreach (DictionaryEntry entryCurrent in _MapAcrIdToCardDataFormatFlag)
                    {
                        int acrId = Convert.ToInt32(entryCurrent.Key);
                        int cardFormat = Convert.ToInt32(entryCurrent.Value);
                        if ((cardFormat != -1) && (acrId != reader.AcrId))
                        {
                            DateTime modifiedTime = DateTime.MinValue;
                            if ((Convert.ToInt32(_CardholderAdditionIndices[cardFormatInfo.MercuryIndex]) & cardFormat) ==
                                Convert.ToInt32(_CardholderAdditionIndices[cardFormatInfo.MercuryIndex]))
                            {
                                inUse = true;
                                break;
                            }

                            if ((Convert.ToInt32(_AssetAdditionIndices[cardFormatInfo.MercuryIndex]) & cardFormat) ==
                                Convert.ToInt32(_AssetAdditionIndices[cardFormatInfo.MercuryIndex]))
                            {
                                inUse = true;
                                break;
                            }
                        }
                    }

                    // Check if this format can be removed
                    if (!inUse)
                    {
                        // Remove the card format from the SCP.                        
                        cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Unassigned; //TODO:                         
                        cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Unchecked;
                        cardFormatInfo.ScpId = -1;

                        ACR.RemoveCardFormatFromScp(cardFormatInfo);

                        // Set format removed to true.
                        formatRemoved = true;
                    }
                }
            }

            if (formatRemoved)
            {
                // Update all the ACR's just to force the user to reopen a display if this occurred outside
                // of their display by another user.
                foreach (DictionaryEntry entryCurrent in _MapAcrIdToCardDataFormatFlag)
                {
                    int acrId = Convert.ToInt32(entryCurrent.Key);
                    int cardFormat = Convert.ToInt32(entryCurrent.Value);

                    if (acrId != reader.AcrId)
                    {
                        // Just set it to the same value.  Time stamp gets set via this method.
                        ACR.UpdateCardFormat(reader.ScpId, acrId, cardFormat);
                    }
                }
            }
        }


private List<SCPCardFormat> AssignPanelCardFormats(ACR reader)
{
    List<SCPCardFormat> assignedCFs = new List<SCPCardFormat>();

    // Assign card formats being associated with this ACR if it hasn't been already.
    foreach (InfoColumn item in SCPCardFormatsView)
    {
        var cardFormatId = Convert.ToInt16(item.ID);

        // Get the card formats.
        SCPCardFormat cardFormatInfo = DataService.AllCardFormatsList.Where(cf => cf.CardFormatId == cardFormatId).FirstOrDefault();//new SCPCardFormat(DefaultController.SCP_ID, cardFormatId, mercuryIndex, name, description);

        // Determine if this format needs to be assigned
        if (item.IsAssigned)
        {
            if (cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Unassigned)
            {
                // Set the scp id's                        
                cardFormatInfo.ScpId = reader.ScpId;
                cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Assigned; //TODO:
                cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Checked;

                // Call the database method to add this format - only do it once for both cardholder and asset)
                int mercuryIndexAssigned = ACR.AssignCardFormatToScp(cardFormatInfo);
                        
                // Set the mercury indices now.                        
                cardFormatInfo.MercuryIndex = mercuryIndexAssigned;

                assignedCFs.Add(cardFormatInfo);
            }
        }
    }

    return assignedCFs;
}
*/

/*
private int ReadCardFormatFlag()
{
    // Reassign card formats that are no longer being used by ANY ACR for this panel. Must remove first to free up a mercury index.
    UnassignPanelCardFormats(DefaultReader);

    List<SCPCardFormat> updatedCardFormats = AssignPanelCardFormats(DefaultReader);

    // Assign card formats being associated with this ACR if it hasn't been already.
    // List<SCPCardFormat> updatedCardFormats = ReassignPanelCardFormats();            

    int cardFormatValue = 0;
    foreach (SCPCardFormat cardFormat in updatedCardFormats) //gridControlFlags;
    {
        //if (item.IsAssigned)
        if(cardFormat.AssignStatus == SCPCardFormat.AssignedStatus.Assigned)
        {
            //SCPCardFormat cardFormat = (SCPCardFormat)item.Tag;
            //_AssetAdditionIndices
            int maskValue = Convert.ToInt32(_CardholderAdditionIndices[cardFormat.MercuryIndex]);//Convert.ToInt32(item.ID);
                                    
            cardFormatValue |= maskValue;
        }
    }
    return cardFormatValue;
}
    //DefaultReader.CardDataFormatFlag =  SCPCardFormatsView
    //                                   .Where(cf => cf.IsAssigned)
    //                                   .Select(f => cardFormatValue |Convert.ToInt32(f.ID)).FirstOrDefault();

    //ACR.RemoveCardFormatFromScp(formatCardholderCurrent);
    //ACR.UpdateCardFormat(_AccessControlReader.ScpId, acrId, cardFormat);        
*/

/*
protected void HandleDefaultSave()
{
    // if this is a defaultable object type
    // handle any updates of the Set As Default checkbox
    if (_canDefault)
    {
        var single = (AccessBO)_AccessObject;

        if (_checkSetDefault.Checked)
        {
            // if the Set As Default checkbox is selected AND
            // this item is NOT already the default item
            // for its namespace type.. display a message box
            // asking the user to confirm the change
            IDenticard.Premisys.ID_HardwareTemplateReadResult currentDefaultNode = null;
            if (single.NameSpaceID == 17 || single.NameSpaceID == 19)
                currentDefaultNode = IDenticard.Premisys.HardwareTemplate.Read(single.NameSpaceID, SioTypeFlag);
            else
                currentDefaultNode = IDenticard.Premisys.HardwareTemplate.Read(single.NameSpaceID, null);

            var saveDefault = true;

            if (currentDefaultNode.Node_ID != null && single.Node_ID != (short)currentDefaultNode.Node_ID)
            {
                var dialogResult = MessageBox.Show(
                    string.Format("{0} is already set as the default item for {1}\n\nAre you sure you want to overwrite the currently assigned default?", currentDefaultNode.Name, currentDefaultNode.TypeName),
                    string.Format("Overwrite existing default for {0}", currentDefaultNode.TypeName),
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (dialogResult == DialogResult.No)
                    saveDefault = false;
            }

            if (saveDefault)
            {
                if (single.NameSpaceID == 17 || single.NameSpaceID == 19)
                    IDenticard.Premisys.HardwareTemplate.SetDefault(single.NameSpaceID, single.Node_ID, SioTypeFlag);
                else
                    IDenticard.Premisys.HardwareTemplate.SetDefault(single.NameSpaceID, single.Node_ID, null);
            }

        }
        else
        {
            Premisys.HardwareTemplate.ClearDefault(single.NameSpaceID, single.Node_ID);
        }
    }
}
   
*/

/*
        /// <summary>
        /// Event handles the load event for this dialog.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void PropertiesBase_Load(object sender, System.EventArgs e)
        {
            try
            {
                // assume the item cannot be defaulted and thus the Set As Default checkbox
                // should not be displayed
                _canDefault = false;
                var showAdvancedUi = true;     // assume the user is using AdvancedUi
                var _removePermission = false;
                               
                // This code would execute in design mode but we don't want this to be the case.
                if (!DesignMode)
                {
                    // Set the image list.
                    _LabelImage.ImageList = _MasterImageList;

                    // Set advanced ui.  Child classes needing to handle advanced items
                    // will override this method.  Base implementation here does nothing.
                    SetAdvancedUI();

                    // Call the child implemented method to set the default image.
                    if (_AccessObject is AccessBOCollection)
                    {
                        var collection = (AccessBOCollection)_AccessObject;

                        //check if collection is TimeZone interval or Icons and remove permissions
                        if (collection.NameSpaceID == 12 || collection.NameSpaceID == 46)
                            _removePermission = true;
                        _LabelImage.ImageIndex = collection.DefaultIcon;
                        this.Text = collection.Name;

                        //  Set the name and description
                        _TextBoxName.Text = collection.Name;
                        _TextBoxDescription.Text = collection.Description;

                        // Set the namespace id and database id.
                        _databaseId = collection.Node_ID;
                    }
                    else
                    {
                        var single = (AccessBO)_AccessObject;

                        //check if object is TimeZone interval, MapIcon or Icons and remove permissions
                        if (single.NameSpaceID == 47 || single.NameSpaceID == 13 || single.NameSpaceID == 45)
                            _removePermission = true;

                        _LabelImage.ImageIndex = single.DefaultIcon;
                        this.Text = single.Name;

                        //  Set the name and description
                        _TextBoxName.Text = single.Name;
                        _TextBoxDescription.Text = single.Description;

                        if (single.NameSpaceID == 17 || single.NameSpaceID == 19)
                            _canDefault = Premisys.NameSpace.CanDefault(single.NameSpaceID, SioTypeFlag);
                        else
                            _canDefault = Premisys.NameSpace.CanDefault(single.NameSpaceID);

                        // set the Set As Default check state appropriately
                        // to indicate if the current item is the template
                        // for defeaults
                        if (_canDefault)
                        {
                            short? defaultNodeId;
                            if (single.NameSpaceID == 17 || single.NameSpaceID == 19)
                                defaultNodeId = Premisys.HardwareTemplate.Read(single.NameSpaceID, SioTypeFlag).Node_ID;
                            else
                                defaultNodeId = Premisys.HardwareTemplate.Read(single.NameSpaceID, null).Node_ID;

                            var currentIsDefault = (defaultNodeId == single.Node_ID);

                            _checkSetDefault.Checked = currentIsDefault;
                        }

                        // Set the namespace id and database id.
                        _databaseId = single.Node_ID;
                    }
                    
                    // Load the security section
                    LoadSecurity();

                    // remove Premissions tab if flagged
                    if (_removePermission)
                        _TabControlProperties.TabPages.Remove(_tabPagePermissions);

                    bool.TryParse(AppContext.One.Settings.ShowAdvancedUI, out showAdvancedUi);

                    // if the object type allows default AND the ShowAdvanceUI is selected - show the checkbox
                    _checkSetDefault.Visible = _canDefault && showAdvancedUi;
                    _checkSetDefault.BringToFront();

                    // Load the controls for this form.
                    LoadControls();

                    // Load the filterable controls for this form.
                    LoadFilterableControls();
                }
            }
            catch (IDDataException de)
            {
                // Set Exception
                _DataExceptionOccurred = true;
                _DataException = de;
                AppContext.One.ExHandler.Handle(_DataException);

                // Report error to the user.
                MessageBox.Show(Constant.ExceptionFailAccessBoPropertyLoad, Constant.HeaderDataCorruptionError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Cancel and close the form.
                DialogResult = DialogResult.Cancel;
                Close();
            }
            catch (Exception ex)
            {
                //	Notify all of the trace listeners of the error
#if DEBUG
                if (Debugger.IsAttached) { Debugger.Break(); };
#endif
                Trace.WriteLine(Constant.LabelSource + ex.Source + Constant.Comma + Constant.LabelDescription + ex.Message);

                // Ensure the exception is logged.
                AppContext.One.ExHandler.Handle(ex);

                // Report error to the user.
                MessageBox.Show(Constant.ExceptionFailAccessBoPropertyLoad, Constant.HeaderLoadError,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Cancel and close the form.
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
*/