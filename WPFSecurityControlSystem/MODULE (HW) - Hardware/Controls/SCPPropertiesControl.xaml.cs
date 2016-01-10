using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Net;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Utils;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Common;
using WPFSecurityControlSystem.Services;

namespace WPFSecurityControlSystem.Controls
{
    [Export(typeof(SCP))]
    [Export("Controller")]
    public partial class SCPPropertiesControl : BasePropertiesControl<SCP>
    {
        #region Variables & Properties

        private Channel _channel { get; set; }
        private CommType _commType { get; set; }

        bool IsNew
        {
            get
            {
                if (Entity != null && Entity.Channel != null)
                    return Entity.Channel.Channel_ID == -1;
                return true;
            }
        }

        public ObservableCollection<IDenticard.AccessUI.IpAddressRange>  DHCPIPAddressesRanges { get; set; }
     
        private DataView GetSCPHolidays(SCP controller)
        {
            //var assignedToSCPHolidays = new ObservableCollection<IDenticard.Premisys.SCPHoliday>(Entity.SCPHolidays()/*.OfType<IDenticard.Premisys.SCPHoliday>()*/);
            var scpHolidaysDataView = DataService.GetSCPAvailableHolidays(controller.SCP_ID);
            DataTable extended = scpHolidaysDataView.Table;
            extended.Columns.Add("IsAssigned");
            foreach (DataRow holidayRow in extended.Rows)
                holidayRow["IsAssigned"] = Convert.ToInt16(holidayRow[__SCP.ColumnSCPID]) > -1;           

            //var scpHolidaysWraped = (from DataRow holidayRow in scpHolidaysDataView.Table.Rows //IsAssigned =true, where SCP_ID == Entity.SCP_ID or SCP_ID > -1
            //                         select new
            //                         {                                        
            //                             Holiday_ID = holidayRow[__Holiday.ColumnHolidayID],
            //                             Name = holidayRow[__Holiday.ColumnName],
            //                             Holiday_Date = holidayRow[__Holiday.ColumnHolidayDate],
            //                             IsAssigned = Convert.ToInt16(holidayRow[__SCP.ColumnSCPID]) > -1
            //                         });

            return extended.AsDataView();
        }

        public ICollectionView CurrentHolidaysView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(lvHolidays.ItemsSource);
            }
        }

        List<DTO.Holiday> NewHolidays { get; set; }

        #endregion
   
        #region Constructor

        public SCPPropertiesControl() : base() // LoadFilterableControls();
        {
            InitializeComponent();

            this.DataContext = this; // = VM to bind commands            
            cmbCommType.SelectionChanged += new SelectionChangedEventHandler(cmbCommType_SelectionChanged);
        }

        #endregion

        #region Overrides

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();
            //Register validators - or ValidationFactory.CreateValidator
            ErrorProvider.RegisterValidator(txtName);   
            ErrorProvider.RegisterValidator(cmbCommType);          
            ErrorProvider.RegisterValidator(cmbScpType);
            ErrorProvider.RegisterValidator(cmbPhysicalAddress);
            ErrorProvider.RegisterValidator(cmbTimeZone);
            ErrorProvider.RegisterValidator(cmbCardDatabase);
            ErrorProvider.RegisterValidator(cmbIOBautRate);

            ErrorProvider.RegisterValidator<Int32>(txtTransactionsLimit); //Required + type int

            ErrorProvider.RegisterValidator<Int32>(txtRetryCount);
            ErrorProvider.RegisterValidator<Int32>(txtIOTimeout); //required
            ErrorProvider.RegisterRangeValidator<Int32>(txtOfflineTime, 10000, 65000);

            //If redundancy enabled            
            ErrorProvider.RegisterValidator<Int32>(txtAltRetryCount);
                        
            //ErrorProvider.RegisterRangeValidator<Int32>(txtAltPoolDelay, 500, 9999);
            //ErrorProvider.RegisterRangeValidator<Int32>(txtAltTCPIPRetryConnect, 10000, 20000);            
            
            //Other DHCP tab
            //ErrorProvider.RegisterValidator(txtStartIPAddress, ValidationFormat.IPAddress);
            //ErrorProvider.RegisterValidator(txtEndIPAddress, ValidationFormat.IPAddress);
        }

        protected override void LoadFilterableControls()
        {
            base.LoadFilterableControls();

            //cmbCommType.ItemsSource = Enum.GetNames(typeof(CommType));           
            //1.Fill main tab
            cmbScpType.ItemsSource = DataService.GetScpTypes();
            cmbScpType.DisplayMemberPath = __SCPType.ColumnName;
            cmbScpType.SelectedValuePath = __SCPType.ColumnSCPTypeID;
        
            cmbRTSMode.ItemsSource = DataService.GetRTSModes();
            cmbAltRTSMode.ItemsSource = DataService.GetRTSModes();
            cmbRTSMode.DisplayMemberPath = cmbAltRTSMode.DisplayMemberPath = __RTSMode.ColumnName;
            cmbRTSMode.SelectedValuePath = cmbAltRTSMode.SelectedValuePath =__RTSMode.ColumnRTSModeID;

            cmbBautRate.ItemsSource = DataService.GetBautRates();
            cmbIOBautRate.ItemsSource = DataService.GetBautRates();
            cmbBautRate.DisplayMemberPath = cmbIOBautRate.DisplayMemberPath = __BaudRate.ColumnName;
            cmbBautRate.SelectedValuePath = cmbIOBautRate.SelectedValuePath = __BaudRate.ColumnBaudRateID;

            cmbTimeZone.ItemsSource = DataService.GetGMTRegion();
            cmbTimeZone.DisplayMemberPath = __GMTRegion.ColumnDescription;
            cmbTimeZone.SelectedValuePath = __GMTRegion.ColumnGMTID;

            cmbCardDatabase.ItemsSource = DataService.GetCardDababases();
            cmbCardDatabase.DisplayMemberPath = __CardDatabase.ColumnName;
            cmbCardDatabase.SelectedValuePath = __CardDatabase.ColumnCardDatabaseID;

            //2. Other Tabs                     
            DHCPIPAddressesRanges = new ObservableCollection<IpAddressRange>(DataService.GetAllDhcpIpAddressRange((short)Entity.SCP_ID)); //from AccessUI.SCP.AccessUI
            lvIPAddressesRange.ItemsSource = DHCPIPAddressesRanges;
            //lvIPAddressesRange.DataContext = this;

            //HolidaysView = GetSCPHolidays(Entity);
            lvHolidays.ItemsSource = GetSCPHolidays(Entity);
        }

        public override void LoadProperties(SCP entity)
        {
            base.LoadProperties(entity);

            if (Entity == null || Entity.Node_ID == -1) //set default values
            {
                cmbCommType.SelectedIndex = (int)Constants.DEFAULT_CommType; //'Network by default'
                cmbScpType.SelectedValue = (int)Constants.DEFAULT_ScpType;// IDenticard.Access.Common.ScpType.SCP_E; Expandable Reader ctrl
                cmbCardDatabase.SelectedValue = (int)Constants.DEFAULT_CardDatabase; 

                cmbBautRate.SelectedValue = cmbIOBautRate.SelectedValue = (int)Constants.DEFAULT_BaudRate;// = 38400;            
                txtIOTimeout.Text = Constants.DEFAULT_IOTimeout.ToString();
                cmbTimeZone.SelectedValue = (int)Constants.DEFAULT_GMTRegion;//Eastern Time (US & Canada)

                txtTransactionsLimit.Text = Constants.DEFAULT_TRANS_BUFFER.ToString();
                return;
            }

            //1. Load Security
            Entity.ReadSecurity();
            ctrlPermissions.LoadSecurity(Entity);

            //Fill controller controls
            //2.1. Common settings
            txtName.Text = Entity.Name;
            txtDescription.Text = Entity.Description;

            //2.2. Read channel settings
            _channel = Entity.Channel;//IDenticard.Premisys.Channel.Read((short)Entity.Channel_ID); //Get channel ID
            _commType = (CommType)_channel.CommType_ID;
            
            cmbCommType.SelectedIndex = (_commType == CommType.NetworkOut) ? 0 : 1; //Network out    

            if (_commType == CommType.Serial)
            {
                    cmbRTSMode.SelectedValue = _channel.RTSMode_ID;
                    cmbBautRate.SelectedValue = _channel.BaudRate_ID;

                    txtCommPort.Text = _channel != null ? _channel.PortNumber.ToString() : string.Empty;
                    txtCommString.Text = Entity.CommString;                
            }
            else
            {
                //Fill network related fields
                txtCommPort.Text = Convert.ToString(_channel.PortNumber);
                
                if (!IsNew) //for existing controller
                {
                    IPAddress ip; int port;

                    ConverterHelper.GetIpAddressAndPort(Entity.CommString, out ip, out port);

                    txtIPAddress.Text = Convert.ToString(ip);
                    txtIPPort.Text = Convert.ToString(port);
                }

                txtTCPIPRetryConnect.Text = Convert.ToString(_channel.TCPRetryConnectInterval);               
                txtPoolDelay.Text = Convert.ToString(Entity.PollDelay);
            }

            txtReplyTimeout.Text = _channel.SCPReplyTimeout;      //Controller ReplyTimeout      

            cmbScpType.SelectedValue = (int)Entity.SCPType;
            cmbPhysicalAddress.Text = Convert.ToString(Entity.PhysicalAddress);

            cmbTimeZone.SelectedValue = (int)Entity.GMT_ID;
            cmbCardDatabase.SelectedValue = Entity.CardDatabase_ID;
            cmbIOBautRate.SelectedValue = (int)Entity.MSPBaudRate;            
                       
            //cmbMSPPort.Text = ControllerEntity.MSPPort;  For Poe controller
            ckbEnableDowstreamCommunication.IsChecked = Entity.IsDownstreamEnabled; 

            //Other settings
            txtOfflineTime.Text = Convert.ToString(Entity.OfflineTime);
            txtTransactionsLimit.Text = Convert.ToString(Entity.TransBuffer);        
            txtIOTimeout.Text = Convert.ToString(Entity.MSPReplyTimeOut);
            txtRetryCount.Text = Convert.ToString(Entity.RetryCount);
           
            //2.3 Redundant port
            ckbUseRedundantPort.IsChecked = Entity.AltPortEnable == 1;
            if ((bool)ckbUseRedundantPort.IsChecked)// entity.RedundantChannel_ID != null)
            {
                Channel redundantChannel = Entity.RedundantChannel;// Channel.Read((short)entity.RedundantChannel_ID);
                cmbAltCommType.SelectedIndex = redundantChannel.CommType_ID;
                txtAltRetryCount.Text = Convert.ToString(Entity.AltRetryCount);
                txtAltPoolDelay.Text = Convert.ToString(Entity.AltPollDelay);
                txtAltReplyTimeout.Text = Convert.ToString(Entity.AltPortEnable);
            }

            //3. Fill data on other tabs
            tabDHCP.Visibility = Convert.ToInt16(cmbScpType.SelectedValue) == (int)ScpType.EP1501 //PoE controller
                        ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;                       
        }

        public override void SaveProperties()
        {
            //1.Save permissions
            ctrlPermissions.SaveSecurity();

            //1.Read all controls values to BO
            Entity.Name = txtName.Text;
            Entity.Description = txtDescription.Text;        

            //2. Channel settings - common for Network/Serial
            Entity.Channel.CommType_ID = (short)_commType;//(short)cmbCommType.SelectedValue;
            Entity.Channel.SCPReplyTimeout = txtReplyTimeout.Text;            

            if (_commType == CommType.Serial)
            {
                Entity.Channel.RTSMode_ID = Convert.ToInt16(cmbRTSMode.SelectedValue);
                Entity.Channel.BaudRate_ID = Convert.ToInt32(cmbBautRate.SelectedValue);
                Entity.Channel.PortNumber = txtCommPort.Text;

                Entity.CommString = txtCommString.Text;
            }
            else
            {
                Entity.PollDelay = Convert.ToInt32(txtPoolDelay.Text);
                Entity.Channel.TCPRetryConnectInterval = txtTCPIPRetryConnect.Text;

                IPAddress ipAddress;
                ConverterHelper.TryParseIP(txtIPAddress.Text, out ipAddress);

                Entity.CommString = ipAddress + "." + txtIPPort.Text;                 
            }
            
            //3. Controller settings
            Entity.CardDatabase_ID = Convert.ToInt16(cmbCardDatabase.SelectedValue);
            Entity.SCPType = Convert.ToInt16(cmbScpType.SelectedValue);
            Entity.PhysicalAddress = Convert.ToInt16(cmbPhysicalAddress.Text);
            Entity.GMT_ID = Convert.ToInt16(cmbTimeZone.SelectedValue);
            //Entity.MSPReplyTimeout = 
            Entity.MSPBaudRate = Convert.ToInt32(cmbIOBautRate.SelectedValue);
  
            //cmbMSPPort.Text = ControllerEntity.MSPPort;  
            Entity.IsDownstreamEnabled = ckbEnableDowstreamCommunication.IsEnabled && ckbEnableDowstreamCommunication.IsChecked == true;

            //4. Other settings
            Entity.OfflineTime = Convert.ToInt32(txtOfflineTime.Text);
            Entity.MSPReplyTimeOut = Convert.ToInt32(txtIOTimeout.Text); //TODO:
            
            Entity.TransBuffer = Convert.ToInt32(txtTransactionsLimit.Text);
            Entity.RetryCount = Convert.ToInt32(txtRetryCount.Text);
           
            //5. Redundant Port
            Entity.AltPortEnable = ckbUseRedundantPort.IsChecked == true ? 1: 0;
            if (ckbUseRedundantPort.IsChecked == true)
            {                
                //TODO:
                var redundantCommType = cmbAltCommType.SelectedIndex == 0 ? CommType.NetworkOut : CommType.Serial;

                Entity.RedundantChannel.CommType_ID = (int)redundantCommType;

                if(redundantCommType == CommType.Serial)
                {
                    Entity.RedundantChannel.BaudRate_ID = Convert.ToInt32(cmbAltRTSMode.SelectedValue);//TODO
                    Entity.RedundantChannel.RTSMode_ID = Convert.ToInt32(cmbAltRTSMode.SelectedValue); //TODO
                    
                    Entity.RedundantChannel.PortNumber = txtAltCOMPort.Text;//TODO
                    Entity.AltPortNumber = Convert.ToInt32(txtAltCOMPort.Text); 
                    //Entity.RedundantChannel.CommString =                     

                    Entity.AltCommString = txtAltCommString.Text;
                }
                else
                {
                    Entity.RedundantChannel.TCPRetryConnectInterval = txtAltTCPIPRetryConnect.Text; //TODO:
                    Entity.RedundantChannel.SCPReplyTimeout = txtAltReplyTimeout.Text; //TODO:
                    //Entity.RedundantChannel. = txtAltReplyTimeout.Text;
                    Entity.AltPollDelay = Convert.ToInt32(txtAltPoolDelay.Text);

                    int altPort = 0;                    
                    IPAddress altIp; 
                    if(ConverterHelper.TryParseIP(txtAltCommString.Text, out altIp) && Int32.TryParse(txtIPPort.Text, out altPort))
                        Entity.AltCommString = altIp + Constants.DOT_SEPARATOR.ToString() + altPort;
                }

                Entity.AltRetryCount = /*Entity.RedundantChannel.RetryCount =*/ Convert.ToInt32(txtAltRetryCount.Text);      
            }
            
            //6. Tab Holiday
            if (Entity.UnassignHolidays != null)
                Entity.UnassignHolidays.Clear();
            if (Entity.AssignHolidays != null)
                Entity.AssignHolidays.Clear();
            foreach (DataRowView holidayRow in CurrentHolidaysView)
            {
                //Get full onfo from hash table: NewHolidays[holidayRow.HolidayID]
                int holiday_ID = Convert.ToInt32(holidayRow[__Holiday.ColumnHolidayID]);
                bool isAssigned = Convert.ToBoolean(holidayRow["IsAssigned"]);
                //6.1. Create in DB   
                if (holiday_ID == -1)
                {
                    var holidayCollection = DataService.FindObjectsCollection<Holiday>(TreeType.AccessSetting, null);//new HolidayCollection(Entity.SCP_ID, null);
                    var gHoliday = holidayCollection.AddChild() as Holiday;// assign to SCP newly created holiday  
                    //gHoliday.Holiday_ID = Convert.ToInt32(holidayRow[__Holiday.ColumnHolidayID]);
                    gHoliday.Name = holidayRow[___Global.ColumnName].ToString();// holiday.Name;
                    gHoliday.Description = holidayRow[___Global.ColumnDescription].ToString();// holiday.Description;
                    //gHoliday.Date = holiday.Date;// loopDate.AddDays(-1);                    
                    //gHoliday.Duration = holiday.Duration;                    
                    //gHoliday.Name = ResConstants.HolidayPropertiesPresidentsDay;
                    //gHoliday.Description = ResConstants.HolidayPropertiesPresidentsDay;                    
                    //gHoliday.Type = HOLIDAY_SYS_GEN_TYPE;                                        
                    gHoliday.Update(); // do not create - it is created already, when Collection.AddChild()

                    holiday_ID = gHoliday.Holiday_ID;
                }

                //6.2. Assign the holiday to this controller
                //DataRowView view = (DataRowView)lvHolidays.Items[index];     
                if (isAssigned)
                {
                    if (Entity.AssignHolidays == null) Entity.AssignHolidays = new List<int>();
                    Entity.AssignHolidays.Add(holiday_ID);
                }
                else
                {
                    if (Entity.UnassignHolidays == null) Entity.UnassignHolidays = new List<int>();
                    Entity.UnassignHolidays.Add(holiday_ID);
                }
            }

           //7. DHCP - IPRanges for PoE controller:
            if (Entity.SCPType == (int)ScpType.EP1501)//PoE - save IPs ranges              
                Entity.IpRanges = DHCPIPAddressesRanges.ToList();                                                      

            //Commit Save
            base.SaveProperties();            
        }

        #endregion

        #region Handlers

        private void cmbScpType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {          
            //1. PoE controller DHCP tab
            tabDHCP.Visibility = ckbEnableDowstreamCommunication.Visibility =
                        Convert.ToInt16(cmbScpType.SelectedValue) == (int)ScpType.EP1501 
                        ? System.Windows.Visibility.Visible
                        : System.Windows.Visibility.Hidden;

            //2. Redundancy support, except of Standard or Compact controller type
            grpRedundantPort.Visibility = Convert.ToInt16(cmbScpType.SelectedValue) == (int)ScpType.SCP_2 //Standard 
                                       || Convert.ToInt16(cmbScpType.SelectedValue) == (int)ScpType.SCP_C //Compact
                                       ? System.Windows.Visibility.Hidden : Visibility.Visible;
            //ckbEnableDowstreamCommunication.IsEnabled = Entity != null && Entity.IsDownstreamEnabled || Entity == null

            //3. Baud Rate warning for IP controller
            grpBaudRate.Visibility = Convert.ToInt16(cmbScpType.SelectedValue) == (int)ScpType.EP2500
                                    ? System.Windows.Visibility.Visible
                                    : System.Windows.Visibility.Hidden;
        }

        private void cmbCommType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _commType = cmbCommType.SelectedIndex == 0 ? CommType.NetworkOut : CommType.Serial;

            if (_commType == CommType.NetworkOut) //Network
            {
                ErrorProvider.RegisterValidator(txtIPAddress, ValidationFormat.IPAddress, ComboBox.SelectedValueProperty);
                //ErrorProvider.RegisterValidator<Int32>(txtIPPort);
                ErrorProvider.RegisterRangeValidator<Int32>(txtPoolDelay, 500, 9999);
                ErrorProvider.RegisterRangeValidator<Int32>(txtReplyTimeout, 600, 5000); //600-5000 - for Network & 200-400 -Serial                

                txtReplyTimeout.Text = Constants.DEFAULT_ReplyTimeoutNetwork.ToString();

                //if (IsNew) //Fill with defaults               
                if (String.IsNullOrEmpty(txtIPPort.Text)) txtIPPort.Text = Constants.DEFAULT_IPPort.ToString();               
            }
            else //Serial
            {
                ErrorProvider.UnregisterValidator(txtIPAddress);
                //ErrorProvider.UnregisterValidator(txtIPPort);
                ErrorProvider.UnregisterValidator(txtPoolDelay);                
                ErrorProvider.RegisterRangeValidator<Int32>(txtReplyTimeout, 200, 400);

                txtReplyTimeout.Text = Constants.DEFAULT_ReplyTimeoutSerial.ToString();
                //if (IsNew) //Fill with defaults
                //{
                    if (cmbRTSMode.SelectedValue == null) cmbRTSMode.SelectedValue = (int)Constants.DEFAULT_RTSMode;
                    if(cmbBautRate.SelectedValue == null) cmbBautRate.SelectedValue = (int)Constants.DEFAULT_BaudRate;
                //}
            }
        }

        private void ckbEnableDowstreamCommunication_Checked(object sender, RoutedEventArgs e)
        {
            //TODO:
        }

        private void ckbUseRedundantPort_Checked(object sender, RoutedEventArgs e)
        {
            if (ckbUseRedundantPort.IsChecked == true)
            {
                ErrorProvider.RegisterValidator(txtAltIPAddress, ValidationFormat.IPAddress, ComboBox.SelectedValueProperty);
                //    ErrorProvider.RegisterValidator<Int32>(txtAltRetryCount);
                //    ErrorProvider.RegisterRangeValidator<Int32>(txtAltReplyTimeout, 600, 5000); //600-800 - for Network & 400-600 -Serial
            }
            else
            {
                ErrorProvider.UnregisterValidator(txtAltIPAddress);
                //    ErrorProvider.UnRegisterValidator(txtAltRetryCount);
                //    ErrorProvider.UnRegisterValidator(txtAltReplyTimeout); //600-800 - for Network & 400-600 -Serial 
            }
        }

        private void cmbAltCommType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Entity != null) //Loaded already
            {
                if (cmbAltCommType.SelectedIndex == 0)//Network
                {
                    ErrorProvider.RegisterRangeValidator<Int32>(txtAltPoolDelay, 500, 9999);
                    ErrorProvider.RegisterRangeValidator<Int32>(txtAltTCPIPRetryConnect, 10000, 20000);
                    ErrorProvider.RegisterValidator(txtAltIPAddress, ValidationFormat.IPAddress, ComboBox.SelectedValueProperty);
                    ErrorProvider.RegisterRangeValidator<Int32>(txtAltReplyTimeout, 600, 5000); //for Network

                    ErrorProvider.UnregisterValidator(cmbAltRTSMode);
                    ErrorProvider.UnregisterValidator(txtAltCOMPort);

                    //FillDefaults
                    //if (IsNew && ckbUseRedundantPort.IsChecked == true)
                    //{
                    if (String.IsNullOrEmpty(txtAltPoolDelay.Text)) txtAltPoolDelay.Text = Constants.DEFAULT_PoolDelay.ToString();
                    txtAltReplyTimeout.Text = Constants.DEFAULT_ReplyTimeoutNetwork.ToString();
                    if (String.IsNullOrEmpty(txtAltTCPIPRetryConnect.Text)) txtAltTCPIPRetryConnect.Text = Constants.DEFAULT_IPRetryConnect.ToString();
                    if (String.IsNullOrEmpty(txtAltRetryCount.Text)) txtAltRetryCount.Text = Constants.DEFAULT_RetryCount.ToString();
                    //}
                }
                else //Serial
                {
                    ErrorProvider.RegisterValidator(cmbAltRTSMode);
                    ErrorProvider.RegisterValidator(txtAltCOMPort);
                    ErrorProvider.RegisterRangeValidator<Int32>(txtAltReplyTimeout, 200, 400); //for Serial

                    ErrorProvider.UnregisterValidator(txtAltPoolDelay);
                    ErrorProvider.UnregisterValidator(txtAltTCPIPRetryConnect);
                    ErrorProvider.UnregisterValidator(txtAltIPAddress);

                    //FillDefaults
                    //if (IsNew && ckbUseRedundantPort.IsChecked == true)
                    //{
                    if (cmbAltRTSMode.SelectedValue == null) cmbAltRTSMode.SelectedValue = (int)Constants.DEFAULT_RTSMode;
                    txtAltReplyTimeout.Text = Constants.DEFAULT_ReplyTimeoutSerial.ToString();
                    if (String.IsNullOrEmpty(txtAltRetryCount.Text)) txtAltRetryCount.Text = Constants.DEFAULT_RetryCount.ToString();
                    //}
                }
            }
        }

        #region Other Tabs Handlers

        private void btnAddHoliday_Click(object sender, RoutedEventArgs e)
        {
            BasePropertiesDialog dialog = DialogsFactory.CreateHolidayDialog(Entity.SCP_ID);
            if (dialog.ShowDialog() == true)
            {
                DTO.Holiday holiday = dialog.Data as DTO.Holiday;     //holiday.Holiday_ID = generate;    
                var data = CurrentHolidaysView;
                if (data is BindingListCollectionView)
                {
                    BindingListCollectionView source = data as BindingListCollectionView;
                    DataRowView holidayRow = source.AddNew() as DataRowView;

                    holidayRow[__Holiday.ColumnHolidayID] = -1;//holiday.Holiday_ID; // assign to SCP newly created holiday 
                    holidayRow[___Global.ColumnName] = holiday.Name;
                    holidayRow[___Global.ColumnDescription] = holiday.Description;
                    //holidayRow[__Holiday.ColumnHolidayDate] = holiday.Date;
                    //holidayRow[__Holiday.ColumnDuration] = holiday.Duration;

                    holidayRow[__SCP.ColumnSCPID] = Entity.SCP_ID; //current controller
                    holidayRow["IsAssigned"] = true; // auto assigning

                    //Save information with a new Holiday into the temporary table
                    if (NewHolidays == null) NewHolidays = new List<DTO.Holiday>();

                    NewHolidays.Add(holiday);

                    //1. Create in DB - if save
                    //2. Assign to the controller
                }
            }
        }

        private void linkDeleteIPAddressFromRange_Click(object sender, RoutedEventArgs e)
        {
            var currItem = ((Hyperlink)sender).DataContext as IpAddressRange;
            //ICollectionView s = CollectionViewSource.GetDefaultView(lvIPAddressesRange.ItemsSource);
            //var currItem = lvIPAddressesRange.SelectedItem as IDenticard.AccessUI.IpAddressRange;
            if (currItem != null)
                DHCPIPAddressesRanges.Remove(currItem);
        }

        private void txtStartIPAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            IPAddress ipaddress = null;
            //if(!ValidationHelper.TryParseIP(txtStartIPAddress.Text, out ipaddress))
            //    txtStartIPAddress.BindingGroup.IsValid = false;
            //EntityErrorProvider.;
        }

        private void btnAddIPAddressesRange_Click(object sender, RoutedEventArgs e)
        {
            //var existingIPRanges = SCP.GetAllDhcpRangesExcluding(Entity.SCP_ID);

            IPAddress startIP = null;
            IPAddress endIP = null;
            if (ConverterHelper.TryParseIP(Convert.ToString(txtStartIPAddress.Value), out startIP) && ConverterHelper.TryParseIP(Convert.ToString(txtEndIPAddress.Value), out endIP))
            {
                if (ConverterHelper.CompareIP(startIP, endIP) == 1)
                {
                    MessageBox.Show(" Start IP address should be less than end IP address!", "Wrong IP range", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEndIPAddress.Clear();
                    return;
                }
                else if (DHCPIPAddressesRanges.Any(r => ConverterHelper.IpRangeContains(r, startIP))) //check existing range
                {
                    MessageBox.Show("Start IP address is already in use ! Please, enter another one", "IP address is use", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtStartIPAddress.Clear();
                }
                else if (DHCPIPAddressesRanges.Any(r => ConverterHelper.IpRangeContains(r, endIP)))
                {
                    MessageBox.Show("End IP address is already in use ! Please, enter another one", "IP address is use", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txtEndIPAddress.Clear();
                }
                else
                {
                    var ipRange = new IpAddressRange(startIP, endIP);

                    DHCPIPAddressesRanges.Add(ipRange);

                    txtStartIPAddress.Clear();
                    txtEndIPAddress.Clear();

                    //TODO: Refresh  - but not is this way !!!
                    lvIPAddressesRange.ItemsSource = DHCPIPAddressesRanges;
                }
            }
            else
                MessageBox.Show("Please, verify IP addresses range!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }       

        #endregion

        #endregion
    }
}
