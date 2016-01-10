using System;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel.Composition;
using IDenticard.AccessUI;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Services;
using System.Data;
using WPFSecurityControlSystem.Utils;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// A base dialog class for IOBoard Properties 
    /// </summary>
    //[Export(typeof(SIO))]
    [Export("I/O Board")]
    [Export("SIO")]
    public sealed partial class SIOPropertiesControl : BasePropertiesControl<SIO>
    {
        #region Properties

        DataSet _scpChannelsDataSet;
        protected DataView ScpChannelsView
        {
            get
            {
                var portRowFilter = String.Format("Port = {0}", cmbMSPPort.Text);//.SelectedItem);
                if (_scpChannelsDataSet == null || _scpChannelsDataSet.Tables.Count == 0)
                    _scpChannelsDataSet = DataService.GetSIOBoardSCPChannels(Entity);
                return new System.Data.DataView(_scpChannelsDataSet.Tables[0], portRowFilter, __SCPChannel.ColumnTR, System.Data.DataViewRowState.CurrentRows);
            }
        }

        #endregion

        #region Contructor

        public SIOPropertiesControl():base()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();
            ErrorProvider.RegisterValidator(txtName);

            ErrorProvider.RegisterValidator(cmbChannelIn);
            ErrorProvider.RegisterValidator(cmbChannelOut);

            ErrorProvider.RegisterValidator<Int32>(txtRetryCount);            
        }

        /// <summary>
        /// Load combo controls and default values
        /// </summary>
        protected override void LoadFilterableControls()
        {
            cmbIPhysicalAddress.ItemsSource = Enumerable.Range(0, 32);
        }

        public override void LoadProperties(SIO entity)
        {
            base.LoadProperties(entity);

            if (Entity == null) return; //TODO:

            //1. Load Security
            Entity.ReadSecurity();
            ctrlPermissions.LoadSecurity(entity);

            //2. Fill IOBoard properties
            //Common properties
            txtName.Text = entity.Name;
            txtDescription.Text = entity.Description;
            ckbEnabled.IsChecked = entity.Enable;

            //IOBoard properties
            cmbIOType.SelectedValue = entity.SIOType_ID;

            var dtTypes = DataService.GetIOBoardTypes(entity.SIO_ID);
            cmbIOType.DisplayMemberPath = __SIOType.ColumnName;
            cmbIOType.SelectedValuePath = __SIOType.ColumnSIOTypeID;
            cmbIOType.ItemsSource = dtTypes;
            cmbIOType.SelectedValue = entity.SIOType_ID;

            FillGridCounts(entity);

            // Populate Channel In and Channel Out Combo
            cmbChannelIn.ItemsSource = ScpChannelsView;
            cmbChannelOut.ItemsSource = ScpChannelsView;

            //Set physical address
            cmbMSPPort.Text = Convert.ToString(entity.Port);
            cmbIPhysicalAddress.SelectedValue = entity.PhysicalAddress;
            txtRetryCount.Text = Convert.ToString(entity.SioRetryCount);
            ckbReverseIOOrder.IsChecked = entity.Reverse; 

            // Populate Channel In and Channel Out Combo
            cmbChannelIn.DisplayMemberPath = __SCPChannel.ColumnTR;
            cmbChannelIn.SelectedValuePath = __SCPChannel.ColumnSCPChannelID;//__SIOType.ColumnInputCnt;          
            cmbChannelIn.SelectedValue = entity.ChannelIn;

            cmbChannelOut.DisplayMemberPath = __SCPChannel.ColumnTR;
            cmbChannelOut.SelectedValuePath = __SCPChannel.ColumnSCPChannelID;//__SIOType.ColumnOutputCnt;
            cmbChannelOut.SelectedValue = entity.ChannelOut;

            //Load Filterable Controls, depending on SIO data
            var dataSetChainedInput = DataService.GetChainedSios(entity, 0);
            cmbChannelNextIn.ItemsSource = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(
                                    dataSetChainedInput.Tables[0], __SIO.ColumnSIOID, typeof(SIO).ToString());
            cmbChannelNextIn.DisplayMemberPath = __SIOType.ColumnName;
            cmbChannelNextIn.SelectedValuePath = __SIOType.ColumnInputCnt;
            cmbChannelNextIn.SelectedValue = entity.NextSIOIn;

            var dataSetChainedOutput = DataService.GetChainedSios(entity, 1);
            cmbChannelNextOut.ItemsSource = IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(
                                    dataSetChainedOutput.Tables[0], __SIO.ColumnSIOID, typeof(SIO).ToString());
            cmbChannelNextOut.DisplayMemberPath = __SIOType.ColumnName;
            cmbChannelNextOut.SelectedValuePath = __SIOType.ColumnOutputCnt;
            cmbChannelNextOut.SelectedValue = entity.NextSIOOut;

            var dataSetChainedReader = DataService.GetChainedSios(entity, 2); //entity.Readers
            cmbNextReader.ItemsSource =  IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(dataSetChainedReader.Tables[0],
                __SIO.ColumnSIOID, typeof(SIO).ToString());
            cmbNextReader.DisplayMemberPath = __Node.ColumnName;
            cmbNextReader.SelectedValuePath = __SIO.ColumnSIOID;
            cmbNextReader.SelectedValue = entity.NextSIOReader;
        }

        public override void SaveProperties()
        {            
            //1.Save permissions
            ctrlPermissions.SaveSecurity();          

            //2.Save ioboard properties                
            Entity.Name = txtName.Text;
            Entity.Description = txtDescription.Text;
            Entity.Enable = ckbEnabled.IsChecked == true;

            Entity.SIOType_ID = Convert.ToInt32(cmbIOType.SelectedValue);

            Entity.Port = Convert.ToInt32(cmbMSPPort.Text);
            Entity.PhysicalAddress = Convert.ToInt16(cmbIPhysicalAddress.Text);
            Entity.SioRetryCount = Convert.ToInt32(txtRetryCount.Text);
            Entity.Reverse = ckbReverseIOOrder.IsChecked == true;
                               
            Entity.ChannelIn = Convert.ToInt32(cmbChannelIn.SelectedValue);
            Entity.ChannelOut = Convert.ToInt32(cmbChannelOut.SelectedValue);
            Entity.NextSIOIn = Convert.ToInt32(cmbChannelNextIn.SelectedValue);
            Entity.NextSIOOut = Convert.ToInt32(cmbChannelNextOut.SelectedValue);
            Entity.NextSIOReader = Convert.ToInt32(cmbNextReader.SelectedValue);            
            
            base.SaveProperties();
        }

        #endregion

        #region Methods

        private void FillGridCounts(SIO ioBoard)
        {
            gridCounts.Items.Clear();
            if (ioBoard.NumOfInputs == -1) ioBoard.NumOfInputs = 0;
            if (ioBoard.NumOfOutputs == -1) ioBoard.NumOfOutputs = 0;
            if (ioBoard.NumOfReaders == -1) ioBoard.NumOfReaders = 0;
            gridCounts.Items.Add(ioBoard);
        }

        #endregion

        #region Handler

        /// <summary>
        /// Handles MSP Port Selection Changes
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void cmbMSPPort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //_msgWarningNoAddressShawn = false;

                // Populate Channel In and Channel Out Combo
                cmbChannelIn.ItemsSource = ScpChannelsView; //Reload MSP Control data
                cmbChannelOut.ItemsSource = ScpChannelsView;                

                if (Entity.ChannelIn == -1)
                    cmbChannelIn.SelectedIndex = 0;
                else
                    cmbChannelIn.SelectedIndex = Entity.ChannelIn;

                if (Entity.ChannelOut == -1)
                    cmbChannelOut.SelectedIndex = 0;
                else
                    cmbChannelOut.SelectedIndex = Entity.ChannelOut;

                if (Entity.SIOType_ID == -1)
                {
                    //#1998 - save previous selected value
                    int selectedSioType = Convert.ToInt32(cmbIOType.SelectedValue);

                    //TODO:
                    //if (cmbMSPPort.SelectedIndex == 0)
                    //    PopulateSioTypeComboBox(1);
                    //else
                    //    PopulateSioTypeComboBox(2);

                    //#1998 - restore previous selected value
                    cmbIOType.SelectedValue = selectedSioType;
                }
                //else TODO:
                    //if(cmbSIOAddress.Visible)
                    //PopulateSIOAdressesComboBox((IDenticard.Access.Common.SIOType)Entity.SIOType_ID, Entity.SIO_ID);//Load sioCombo with new range after MSP changed, but for a new Board
            }
            catch (Exception ex)
            {
                //AppContext.One.ExHandler.Handle(ex);
                
                //HandlePropertiesDialogError();
            }
        }

        /// <summary>
        /// Handles SIO Type Selection Changes.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void cmbIOType_SelectionChanged(object sender, System.EventArgs e)
        {
            try
            {
               //TODO:
            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        #endregion

    }
}
