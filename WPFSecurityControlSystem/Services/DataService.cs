using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Data;
using System.Collections;
using System.Windows;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Data;
using IDenticard;
using IDenticard.Access.Common;
using IDenticard.AccessUI;
using IDenticard.AccessUI.Collections;
using IDenticard.AccessUI.Collections.Global;
using IDenticard.AccessUI.Objects;
using IDenticard.AccessUI.Security;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.DTO;
using WPFSecurityControlSystem.Common;
using WPFSecurityControlSystem.MODULE.HWConfiguration;

namespace WPFSecurityControlSystem.Services
{
    public class DataService
    {
        #region Variables

        static List<IDenticard.Premisys.Site> _sites = null;
        static List<IDenticard.Premisys.AccessControlReader> _accessControlReaders = null;
        static List<IDenticard.Premisys.SCP> _controllers = null;
        static List<IDenticard.Premisys.SIO> _ioBoards = null;

        static DataView _allCardFormatsView;

        #endregion

        #region Defaults-Constants

        public static List<int> DefaultCardFormats
        {
            get
            {
                var defFormats = new List<int>() {
                    0,//Standard 26 bit
                    1,
                    3,//"Corporate 1000 format"
                    4,//"IDenticard 37 bit", "");                                       
                    32, //"IdentiSmart XceedID 32 Bit"             
                };
                return defFormats;

                //var def = (from DataRow cformat in AllCardFormatsView.Table.Rows
                //          select new InfoColumn
                //          {
                //               ID = Convert.ToString(cformat[__SCPCardFormat.ColumnDBID]),
                //               IsAssigned = Convert.ToInt32(cformat[__SCPCardFormat.ColumnSCPID]) == _defaultController.SCP_ID,
                //               Name = Convert.ToString(cformat["Name"])
                //          }).Where(i => defFormats.Contains(Convert.ToInt32(i.ID))).OrderBy(f =>f.ID).ToList();
                //return def;
            }
        }

        #endregion

        #region Properties

        #region Cashed data

        static List<IDenticard.Cache.SCPType> _scpTypes;
        public static List<IDenticard.Cache.SCPType> ScpTypes
        {
            get
            {
                if (_scpTypes == null)
                    _scpTypes = IDenticard.LookupData.SCPTypes;  //Lookup.GetDataTableLookup(__CommType.SPReadCommType);
                return _scpTypes;
            }
        }

        static List<IDenticard.Cache.CommType> _commTypes;
        public static List<IDenticard.Cache.CommType> CommTypes
        {
            get
            {
                if (_commTypes == null)
                    _commTypes = IDenticard.LookupData.CommTypes;  //Lookup.GetDataTableLookup(__CommType.SPReadCommType);
                return _commTypes;
            }
        }

        static List<IDenticard.Cache.AcrMode> _acrModes;
        public static List<IDenticard.Cache.AcrMode> AcrModes
        {
            get
            {
                if (_acrModes == null)
                    _acrModes = IDenticard.LookupData.AcrModes;
                //DataTable dt = Lookup.GetDataTableLookup(__Reader.SPParamMode);
                //return (dt != null) ? dt.DefaultView : null; 
                return _acrModes;
            }
        }

        static List<IDenticard.Cache.AcrMode> _acrOfflineModes;
        public static List<IDenticard.Cache.AcrMode> AcrOfflineModes
        {
            get
            {
                if (_acrOfflineModes == null)
                    _acrOfflineModes = (from m in IDenticard.LookupData.AcrModes
                                        where
                                              m.AcrMode_ID == (Int16)AccessReaderModes.DisabledNoRex ||
                                              m.AcrMode_ID == (Int16)AccessReaderModes.FacilityCodeOnly ||
                                              m.AcrMode_ID == (Int16)AccessReaderModes.Locked ||
                                              m.AcrMode_ID == (Int16)AccessReaderModes.Unlocked
                                        select m).ToList();

                return _acrOfflineModes;
            }
        }

        /// <summary>
        /// TODO:
        /// use :  _acrControlFlags = IDenticard.LookupData.AcrControlFlags after LicensingClient will be configured properly 
        /// </summary>
        static List<IDenticard.Cache.AcrControlFlag> _acrControlFlags;
        public static List<IDenticard.Cache.AcrControlFlag> AcrControlFlags
        {
            get
            {
                if (_acrControlFlags == null)
                {
                    //_acrControlFlags = IDenticard.LookupData.AcrControlFlags; - TODO: use  this code instead the next one 
                    //after LicensingClient will be configure properly in the App.config
                    using (var db = new IDenticard.Cache.LookupCacheDataContext(AppContext.One.Settings.SystemDBConnStr))
                    {
                        _acrControlFlags = (from f in db.AcrControlFlags
                                            orderby f.DisplayOrder
                                            select f).ToList();

                        //if (((IApplicationLicenseInfo)AppContext.One.Settings).PremisysLevel == PremisysLevel.Lite)
                        //if (AppContext.One.IsPremiSysIDAccess || AppContext.One.IsPremiSysIDAccessNoBadging)
                        {
                            var firstOrDefault = _acrControlFlags.FirstOrDefault(i => i.AcrControlFlag1 == 1024);
                            if (firstOrDefault != null)
                                firstOrDefault.IsVisible = false;
                        }
                    }
                }
                return _acrControlFlags;
            }
        }

        public static DataView AllCardFormatsView
        {
            get
            {
                //if (_allCardFormatsView == null)                
                _allCardFormatsView = GetCardFormats(0, -1);//-1 -all active and inactive

                return _allCardFormatsView;
            }
        }

        static List<IDenticard.AccessUI.SCPCardFormat> _allCardFormatsList;
        public static List<IDenticard.AccessUI.SCPCardFormat> AllCardFormatsList
        {
            get
            {
                //if (_allCardFormatsList == null)
                {
                    _allCardFormatsList = new List<IDenticard.AccessUI.SCPCardFormat>();

                    foreach (DataRowView row in AllCardFormatsView)
                    {
                        int scpId = row[__SCPCardFormat.ColumnSCPID] == null ? -1 : Convert.ToInt32(row[__SCPCardFormat.ColumnSCPID]);
                        int id = row[__SCPCardFormat.ColumnDBID] == null ? -1 : Convert.ToInt32(row[__SCPCardFormat.ColumnDBID]);
                        int mercuryIndex = row[__SCPCardFormat.ColumnMercuryIndex] == null ? -1 : Convert.ToInt32(row[__SCPCardFormat.ColumnMercuryIndex]);
                        string name = Convert.ToString(row[___Global.ColumnName]);
                        string description = Convert.ToString(row[___Global.ColumnDescription]);

                        IDenticard.AccessUI.SCPCardFormat cardFormat = new IDenticard.AccessUI.SCPCardFormat(scpId, id, mercuryIndex, name, description);
                        _allCardFormatsList.Add(cardFormat);
                    }
                }
                return _allCardFormatsList;
            }
        }

        #endregion

        public static List<IDenticard.Premisys.Site> Sites
        {
            get
            {
                try
                {
                    //if (_sites == null)//GetSites();
                    _sites = IDenticard.Premisys.Site.Enumerate();
                }
                catch (Exception ex) { MessageBox.Show("Check connection to DB", "Error"); }

                return _sites;
            }
        }

        public static List<IDenticard.Premisys.SCP> Controllers
        {
            get
            {
                try
                {
                    //if (_controllers == null)
                    _controllers = GetControllers();
                }
                catch (Exception ex) {/* MessageBox.Show("Check connection to DB"); */}

                return _controllers;
            }
        }

        public static List<IDenticard.Premisys.SIO> SIOBoards
        {
            get
            {
                try
                {
                    //if (_accessControlReaders == null)
                    _ioBoards = GetSIOBoards();
                }
                catch (Exception ex) {/* MessageBox.Show("Check connection to DB"); */}

                return _ioBoards;
            }
        }

        public static List<IDenticard.Premisys.AccessControlReader> AccessControlReaders
        {
            get
            {
                try
                {
                    //if (_accessControlReaders == null)
                    _accessControlReaders = GetAccessControlReaders();
                }
                catch (Exception ex) {/* MessageBox.Show("Check connection to DB"); */}

                return _accessControlReaders;
            }
        }

        /// <summary>
        /// Cards formats assigned to the current controller or can be assigned
        /// </summary>
        //List<InfoColumn> _assignedCardFormats;
        //public List<InfoColumn> SCPCardFormatsWrappedList
        //{
        //    get
        //    {
        //        var result = (from DataRow cformat in DataService.AllCardFormatsView.ToTable().Rows
        //                      select new InfoColumn
        //                      {
        //                          ID = Convert.ToString(cformat[__SCPCardFormat.ColumnDBID]),
        //                          Name = Convert.ToString(cformat[___Global.ColumnName]),
        //                          IsAssigned = Convert.ToInt32(cformat[__SCPCardFormat.ColumnSCPID]) == DefaultSettingsEntity.Node_ID//_defaultController.SCP_ID
        //                      }).ToList();
        //        return result;

        //        //var assignedCardFormats = SCP.SCPCardFormats(_defaultController.SCP_ID).Select(f => f.CardFormat_ID);
        //        //var res = IDenticard.AccessUI.Lookup.GetCardFormats(_defaultController.SCP_ID, 1);
        //    }
        //}

        #endregion

        #region Methods to retrive real data from PremiSys database

        public static List<IDenticard.Premisys.Site> GetSites()
        {
            return Sites;
        }

        public static List<IDenticard.Premisys.SCP> GetControllers()
        {
            return IDenticard.Premisys.SCP.Enumerate();
        }

        public static List<IDenticard.Premisys.SCP> GetSiteControllers(int site_id)
        {
            return Controllers.Where(s => s.SITE_ID == site_id).ToList();
        }

        public static List<IDenticard.Premisys.AccessControlReader> GetAccessControlReaders()
        {
            return IDenticard.Premisys.AccessControlReader.Enumerate();
        }

        public static List<IDenticard.Premisys.SIO> GetSIOBoards()
        {
            return IDenticard.Premisys.SIO.Enumerate();
        }

        public static List<IDenticard.Premisys.TimeZone> GetTimeZones()
        {
            return IDenticard.Premisys.TimeZone.Enumerate();
        }

        public static List<IDenticard.AccessUI.IpAddressRange> GetAllDhcpIpAddressRange(IDenticard.AccessUI.SCP controller)
        {
            if (controller.IpRanges != null)
                return controller.IpRanges;
            //return IDenticard.AccessUI.IpAddressRange.GetAllDhcpRanges(scp_id); 

            return new List<IDenticard.AccessUI.IpAddressRange>();
        }

        public static List<IDenticard.AccessUI.IpAddressRange> GetAllDhcpIpAddressRange(short scp_id)
        {
            IDenticard.AccessUI.SCP controller = new IDenticard.AccessUI.SCP(scp_id);
            return GetAllDhcpIpAddressRange(controller);
        }

        public static List<IDenticard.Premisys.Holiday> GetSCPAssignedHolidays(int scp_id)
        {
            IDenticard.AccessUI.SCP controller = new IDenticard.AccessUI.SCP(scp_id);
            var allHolidays = IDenticard.Premisys.Holiday.Enumerate();
            var assignedHolidays = (from IDenticard.Premisys.Holiday h in allHolidays
                                    where controller.AssignHolidays.Contains(h.Holiday_ID)
                                    select h).ToList();

            return assignedHolidays;
        }

        public static List<IDenticard.Premisys.Holiday> GetSCPUnassignHolidays(int scp_id)
        {
            IDenticard.AccessUI.SCP controller = new IDenticard.AccessUI.SCP(scp_id);
            var allHolidays = IDenticard.Premisys.Holiday.Enumerate();
            var availableHolidays = (from IDenticard.Premisys.Holiday h in allHolidays
                                     where controller.UnassignHolidays.Contains(h.Holiday_ID)
                                     select h).ToList();

            return availableHolidays;
        }

        public static DataView GetHolidayTypes()
        {
            DataSet ds = null;
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(AppContext.One.Settings.SystemDBConnStr))
                {
                    SqlCommand command = new SqlCommand(__DayFlag.SPReadDayFlag, sqlconn);
                    command.CommandType = CommandType.StoredProcedure;

                    SqlParameter param = new SqlParameter(__DayFlag.SPParamUsrDefined, SqlDbType.SmallInt);
                    AppContext.One.Util.Param.SetShort(param, Constants.USER_DEFINED_HOLIDAY);
                    command.Parameters.Add(param);

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        ds = new DataSet();
                        sqlconn.Open();

                        da.Fill(ds);
                    }
                }
            }
            catch (Exception exError)
            {
                //TODO:
            }

            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0].DefaultView : null;

        }

        public static DataView GetCardFormats(int scpId, int isActive)
        {
            DataSet res = Lookup.GetCardFormats(scpId, isActive);// IDenticard.Premisys.SCP.SCPCardFormats((short)scpId);
            var cardFormatsView = SecurityPermissionHelper.CreateFilteredView(res.Tables[0], __SCPCardFormat.ColumnDBID, typeof(IDenticard.AccessUI.CardFormat).ToString());
            return cardFormatsView; // (res != null && res.Tables.Count > 0) ? res.Tables[0].DefaultView : null;
        }

        public static DataView GetGMTRegion()
        {
            DataTable res = Lookup.GetDataTableLookup(__GMTRegion.SPReadGMTRegion);
            return res != null ? res.DefaultView : null;
        }

        public static DataView GetIOBoardTypes()
        {
            return GetIOBoardTypes(0);
        }

        public static DataView GetIOBoardTypes(int sioId)
        {
            var res = Lookup.GetDataTableLookup(__SIOType.SPReadSIOType, __SIO.SPParamSIOID, (short)sioId);
            return res != null ? res.DefaultView : null;
        }

        public static DataView GetScpTypes()
        {
            DataTable res = Lookup.GetDataTableLookup(__SCPType.SPReadSCPType);
            return res != null ? res.AsDataView() : null;
        }

        public static DataView GetBautRates()
        {
            return Lookup.GetDataTableLookup(__BaudRate.SPReadBaudRate).DefaultView;
        }

        public static DataView GetRTSModes()
        {
            DataTable dt = Lookup.GetDataTableLookup(__RTSMode.SPReadRTSMode);
            return (dt != null) ? dt.DefaultView : null; //col = __RTSMode.ColumnRTSModeID;           
        }

        public static DataView GetCardDababases()
        {
            DataTable dt = Lookup.GetDataTableLookup(__CardDatabase.SPLookupCardDatabase);

            //SecurityPermissionHelper.CreateFilteredView(Lookup.GetDataTableLookup(__CardDatabase.SPLookupCardDatabase),  __CardDatabase.ColumnCardDatabaseID, cardDatabaseTypeName);
            return (dt != null) ? dt.DefaultView : null; //col = __RTSMode.ColumnRTSModeID;   
        }

        public static DataView GetPairedReaders(Door door)
        {
            DataSet ds = Lookup.GetAvailableACRReader(door.ScpId, door.AcrId, door.PairedAcrId);

            return (ds != null && ds.Tables.Count > 0)
                    ? IDenticard.AccessUI.Security.SecurityPermissionHelper.CreateFilteredView(ds.Tables[0],
                    __AccessControlReader.ColumnACRID, typeof(Door).ToString())
                    : null;
        }

        /// <summary>
        /// Get primary readers for a door
        /// </summary>
        /// <param name="door"></param>
        /// <returns></returns>
        public static DataView GetDoorReaders(Door door)
        {
            var tempds = Lookup.GetAvailablePoints(door.ScpId, 2, door.ReaderId, true);//this is ElevatorProperties
            var parentController = new SCP(door.ScpId);
            ScpType controllerType = (ScpType)parentController.SCPType;
            //TODO:
            //if (controllerType == ScpType.EP1501 && parentController.IsDownstreamEnabled) 
            //    tempds = Lookup.RemoveOnboardReaderPort(parentController, tempds);

            return SecurityPermissionHelper.CreateFilteredView(tempds.Tables[0],
                __Reader.ColumnReaderID, typeof(Reader).ToString());
        }

        public static DataView GetReaderConfigurations()
        {
            DataTable dt = Lookup.GetDataTableLookup(__ReaderConfiguration.SPReadReaderConfiguration);
            return (dt != null) ? dt.DefaultView : null; //col = __RTSMode.ColumnRTSModeID;
            //LookupData.ReaderConfigurations
        }

        /// <summary>
        /// Get reader altenate configurations
        /// </summary>
        /// <returns></returns>
        public static DataView GetReaderAltConfigurations()
        {
            DataTable dt = Lookup.GetDataTableLookup(__AltReaderConfiguration.SPReadAltReaderConfiguration);
            return dt != null ? dt.DefaultView : null; //(dt != null && ds.Tables.Count > 0) ? ds.Tables[0].DefaultView : null;
        }

        public static DataView GetCardFormatProtocols()
        {
            DataTable dt = Lookup.GetDataTableLookup(__CardFormatFunction.SPReadCardFormatFunction);
            return dt != null ? dt.DefaultView : null; //(ds != null && ds.Tables.Count > 0) ? ds.Tables[0].DefaultView : null;
        }

        public static DataView GetSCPAvailableHolidays(int scp_id)
        {
            DataSet ds = Lookup.GetSCPAssigned(scp_id, __Holiday.SPLookupHoliday);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0].DefaultView : null;
        }

        /// <summary>
        /// Get list of assigned and not CardFormat flags for the AccessCi\ontrolReader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<InfoColumn> GetACRControlFlagsView(ACR reader)
        {
            var allArcFlags = DataService.AcrControlFlags;

            List<InfoColumn> assignedFlags = new List<InfoColumn>();//List<AcrControlFlag>();

            foreach (var flag in allArcFlags)
            {
                if (flag.IsVisible == true)
                {
                    InfoColumn item = new InfoColumn();
                    item.ID = flag.AcrControlFlag1.ToString();
                    item.Name = flag.Name;
                    item.IsAssigned = reader != null && (reader.AcrControlFlags & (int)flag.AcrControlFlag1) > 0
                                   || reader == null && flag.AcrControlFlag1 == (short)AccessReaderControlFlags.DenyAccessUnderDuress; // - the default by the use case requirements

                    assignedFlags.Add(item);
                }
            }

            return assignedFlags;
        }


        /// <summary>
        /// List of all CardFormats: assigned and non-assigned to the controller
        /// </summary>
        public static List<InfoColumn> GetSCPCardFormatsWrappedList(int scp_id)
        {
            List<InfoColumn> result = null;
            if (scp_id < 0)
            {
                result = (from SCPCardFormat cformat in AllCardFormatsList
                          select new InfoColumn()
                          {
                              ID = cformat.CardFormatId.ToString(),
                              Name = cformat.Name,
                              IsAssigned = cformat.CardFormatId == Constants.DEFAULT_CardFormat //Only default CardFormats
                          }).ToList();
            }
            else
            {
                var scp = IDenticard.Premisys.SCP.Read((short)scp_id);

                //var assignedCardFormats = SCP.SCPCardFormats(_defaultController.SCP_ID).Select(f => f.CardFormat_ID);
                //var res = IDenticard.AccessUI.Lookup.GetCardFormats(_defaultController.SCP_ID, 1);

                var assignedCardFormatIDs = new List<short?>();
                if (scp != null)
                    assignedCardFormatIDs = scp.SCPCardFormats()
                                            .Where(cf => cf.SCP_ID == (short)scp_id)
                                            .Select(c => c.CardFormat_ID).Distinct().ToList();

                result = (from IDenticard.AccessUI.SCPCardFormat cformat in AllCardFormatsList
                          select new InfoColumn()//IDenticard.AccessUI.SCPCardFormat(ScpId, CardFormatId, Name)
                          {
                              ID = cformat.CardFormatId.ToString(),
                              Name = cformat.Name,
                              IsAssigned = assignedCardFormatIDs.Contains((short)cformat.CardFormatId)
                              //IsAssigned = Convert.ToInt32(cformat[__SCPCardFormat.ColumnSCPID]) == scp_id
                          }).ToList();
            }
            return result;
        }

        /// <summary>
        /// Filter Card Formats by assigned and default(even non-assigned. See the use case requirements)   
        /// </summary>
        /// <param name="source"></param>
        /// <param name="useFilter"></param>
        /// <returns></returns>
        public static ObservableCollection<InfoColumn> GetSCPCardFormatsFilterableView(IEnumerable source, bool useFilter)
        {
            var sourceWithFilter = CollectionViewSource.GetDefaultView(source);
            if (sourceWithFilter != null)
                sourceWithFilter.Filter = !useFilter ? null
                                        : new Predicate<object>(delegate(object item)
                                        {
                                            var col = item as InfoColumn;
                                            return col != null && col.IsAssigned
                                                || DefaultCardFormats.Contains(Convert.ToInt32(col.ID));//see use case requirwmwnts
                                        });
            return new ObservableCollection<InfoColumn>(sourceWithFilter.Cast<InfoColumn>());
        }

        /// <summary>       
        /// Filter Card Formats by which are assigned and default(even non-assigned) - See the use case requirements
        /// </summary>
        /// <param name="useFilter"></param>
        /// <returns></returns>
        public static ObservableCollection<InfoColumn> GetSCPCardFormatsFilterableView(bool useFilter)
        {
            var allSCPCardFormatsWrappedList = DataService.GetSCPCardFormatsWrappedList(-1);
            return GetSCPCardFormatsFilterableView(allSCPCardFormatsWrappedList, useFilter);
        }


        /// <summary>
        /// Method determines if a card format should be assigned to the panel.
        /// </summary>
        public static void ReassignPanelCardFormats(ACR reader, List<InfoColumn> scpCardFormatsRefreshList)
        {
            int scpId = reader.ScpId;
            //List<SCPCardFormat> refreshedCardFormatsList = new List<SCPCardFormat>();

            // Assign CardFormats being associated with this ACR if it hasn't been already.
            var scpAssignedCardFormats = IDenticard.Premisys.SCP.SCPCardFormats((short)reader.ScpId);

            foreach (InfoColumn item in scpCardFormatsRefreshList)
            {
                var cardFormatId = Convert.ToInt16(item.ID);
                var cardFormatAssignedToScp = scpAssignedCardFormats.FirstOrDefault(f => f.CardFormat_ID == cardFormatId);
                int mercuryIndex = cardFormatAssignedToScp != null ? Convert.ToInt32(cardFormatAssignedToScp.Mercury_CardFormat_Index) : -1;

                SCPCardFormat cardFormatToUpdate = DataService.AllCardFormatsList.Where(cf => cf.CardFormatId == cardFormatId).FirstOrDefault();
                cardFormatToUpdate.MercuryIndex = mercuryIndex;
                cardFormatToUpdate.ScpId = scpId;
                //OR SCPCardFormat cardFormatToUpdate = new SCPCardFormat(scpId, cardFormatId, mercuryIndex, , );
                // Assign\remove the format                       
                var updatedCardFormat = SetSCPCardFormatStatus(cardFormatToUpdate, item.IsAssigned);//DefaultReader.ScpId, cardFormatId, mercuryIndex, item.IsAssigned);  

                //ACR.UpdateCardFormat(scpId, cardFormatId, updatedCardFormat.MercuryIndex);           
            }
        }

        public static IDenticard.AccessUI.SCPCardFormat SetSCPCardFormatStatus(IDenticard.AccessUI.SCPCardFormat cardFormatInfo, bool assigned)//int scpId, int cardFormatId, int mercuryIndex)
        {
            if (/*scpCardFormat == null &&*/ assigned) //mean - it non- assigned, but shold be assigned
            {
                //cardFormatInfo.ScpId = scpId;      
                cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Checked;
                if (cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Unassigned)
                    cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Assigned;

                int mercuryIndexAssigned = ACR.AssignCardFormatToScp(cardFormatInfo);
                cardFormatInfo.MercuryIndex = mercuryIndexAssigned;
            }

            else//should be removed
            {
                //bool inUse = false;

                //// Check if any ACR's are using this value.  If so, then add this to the number 
                //// of formats for this panel.
                //foreach (DictionaryEntry entryCurrent in _MapAcrIdToCardDataFormatFlag)
                //{
                //    int acrId = Convert.ToInt32(entryCurrent.Key);
                //    int cardFormat = Convert.ToInt32(entryCurrent.Value);
                //    if ((cardFormat != -1) && (acrId != DefaultReader.AcrId))
                //    {
                //        DateTime modifiedTime = DateTime.MinValue;
                //        if ((Convert.ToInt32(_CardholderAdditionIndices[cardholderFormat.MercuryIndex]) & cardFormat) ==
                //            Convert.ToInt32(_CardholderAdditionIndices[cardholderFormat.MercuryIndex]))
                //        {
                //            inUse = true;
                //            break;
                //        }

                //        if ((Convert.ToInt32(_AssetAdditionIndices[cardholderFormat.MercuryIndex]) & cardFormat) ==
                //            Convert.ToInt32(_AssetAdditionIndices[cardholderFormat.MercuryIndex]))
                //        {
                //            inUse = true;
                //            break;
                //        }
                //    }
                //}


                cardFormatInfo.CheckStatus = SCPCardFormat.CheckedStatus.Unchecked;
                //if(cardFormatInfo.AssignStatus == SCPCardFormat.AssignedStatus.Assigned)
                cardFormatInfo.AssignStatus = SCPCardFormat.AssignedStatus.Unassigned;
                if (cardFormatInfo != null)//&& !inUse)
                    ACR.RemoveCardFormatFromScp(cardFormatInfo);
            }

            return cardFormatInfo;
        }

        #region TREE methods (from WinForm Tree)

        #region Cached data

        internal static List<LinkNode> TreeHWConfigNodesList = null;
        internal static List<LinkNode> TreeHWConfigRecursiveNodesList = null;

        internal static List<LinkNode> TreeAccessSettingNodesList = null;

        internal static List<LinkNode> TreeGlobalNodesList = null;

        #endregion

        public static List<LinkNode> GetTreeHWConfigData()
        {
            try
            {
                TreeHWConfigNodesList = LinkNode.CreateTree(TreeType.Hardware);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

            return TreeHWConfigNodesList;
        }

        public static List<LinkNode> GetTreeAccessSettingData()
        {
            try
            {
                TreeAccessSettingNodesList = LinkNode.CreateTree(TreeType.AccessSetting);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return TreeAccessSettingNodesList;
        }

        public static List<LinkNode> GetTreeGlobalData()
        {
            try
            {
                TreeGlobalNodesList = LinkNode.CreateTree(TreeType.Global);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return TreeGlobalNodesList;
        }

        public static List<LinkNode> GetTreePluginsData()
        {
            try
            {
                TreeGlobalNodesList = LinkNode.CreateTree(TreeType.DeviceDrivers);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return TreeGlobalNodesList;
        }

        /// <summary>
        /// HW TreeView data
        /// </summary>
        /// <returns></returns>
        public static ObservableCollection<LinkNode> GetHWTreeData()
        {
            var hwTreeNodes = DataService.GetTreeHWConfigData();

            //Adopt HWTree to display in WPF HW Tree
            if (hwTreeNodes != null)
                return new ObservableCollection<LinkNode>(hwTreeNodes);

            return null;
            //OR
            //var result = new ObservableCollection<HWTreeViewItem>();
            //foreach (LinkNode node in hwTreeNodes)
            //{
            //    var convertednode = new HWTreeViewItem(node);//BuildSubTree(node);
            //    result.Add(convertednode);
            //}
            //return result;
            //OR
            /*else //TODO : REAL STUB to display all nodes of the HW tree
            {
                WPFTreeViewItem allSitesFolder = new WPFTreeViewItem(Constants.SitesFolder, true); //Root node    
                allSitesFolder.ID = "S";
                allSitesFolder.IsExpanded = true;
                var allSitesSubFolders = new ObservableCollection<WPFTreeViewItem>();

                if (Sites != null)
                foreach (IDenticard.Premisys.Site site in Sites)
                {
                    WPFTreeViewItem siteFolder = new WPFTreeViewItem(allSitesFolder, site.Node.Name);
                    siteFolder.ID = "s_" + site.Node.Node_ID;
                    siteFolder.Tag = site;
                    siteFolder.IsExpanded = true;

                    WPFTreeViewItem siteCONTROLLERSFolder = new WPFTreeViewItem(siteFolder, Constants.ControllersFolder, true);
                    siteCONTROLLERSFolder.ID = "C";
                    siteCONTROLLERSFolder.Tag = site;

                    //var siteControllerOfAllChannels = new List<IDenticard.Premisys.SCP>(); //(from ch in site.Channels select ch.Controllers).ToList();
                    var siteAllControllersFolders = new ObservableCollection<WPFTreeViewItem>(); //(from ch in site.Channels select ch.Controllers).ToList();

                    foreach (IDenticard.Premisys.Channel channelOfTheSite in site.Channels)
                    {
                        //siteControllerOfAllChannels.AddRange(channelOfTheSite.Controllers);
                        foreach (IDenticard.Premisys.SCP controller in channelOfTheSite.Controllers)
                        {
                            WPFTreeViewItem controllerFolder = new WPFTreeViewItem(siteCONTROLLERSFolder, controller.Node.Name);
                            controllerFolder.ID = "c_" + controller.Node.Node_ID;
                            controllerFolder.Tag = controller;

                            //--------------Sub folders of the  controller------
                            WPFTreeViewItem IOBoardsFolder = new WPFTreeViewItem(controllerFolder, Constants.IOBoardsFolder, true);
                            IOBoardsFolder.ID = "I";
                            IOBoardsFolder.Tag = controller;

                            if (controller.IoBoards != null)
                                IOBoardsFolder.Items = new ObservableCollection<WPFTreeViewItem>(//controller.IoBoards; //Convert to 
                                                               (from IDenticard.Premisys.SIO ioboard in controller.IoBoards
                                                                select new WPFTreeViewItem
                                                                {
                                                                    ID = "i_" + ioboard.Node.Node_ID,
                                                                    Parent = IOBoardsFolder, //TODO;
                                                                    Name = ioboard.Node.Name,
                                                                    Tag = ioboard

                                                                }));
                            WPFTreeViewItem monitorPointsFolder = new WPFTreeViewItem(controllerFolder, Constants.MonitorPointsFolder, true);
                            monitorPointsFolder.ID = "M";
                            monitorPointsFolder.Tag = controller;//TODO;
                            //monitorPointsFolder.Items = controller.EnumerateMonitorPoint();

                            WPFTreeViewItem controlPointsFolder = new WPFTreeViewItem(controllerFolder, Constants.ControlPointsFolder, true);
                            controlPointsFolder.ID = "P";
                            controlPointsFolder.Tag = controller; //TODO;
                            //controlPointsFolder.Items = controller.EnumerateControlPoint();

                            WPFTreeViewItem doorsFolder = new WPFTreeViewItem(controllerFolder, Constants.DoorsFolder, true);
                            doorsFolder.ID = "D";
                            doorsFolder.Tag = controller;//TODO;
                            //doorsFolder.Items = controller.EnumerateDoor();

                            WPFTreeViewItem elevatorsFolder = new WPFTreeViewItem(controllerFolder, Constants.ElevatorsFolder, true);
                            elevatorsFolder.ID = "E";
                            elevatorsFolder.Tag = controller;//TODO;
                            //elevatorsFolder.Items = controller.EnumerateDoor();

                            //..AddcontrollerIOBoardsFolder, MonitorPointsFolder, as subFolders of the controllerFolder
                            controllerFolder.Items = new ObservableCollection<WPFTreeViewItem>()
                                                {
                                                    IOBoardsFolder,
                                                    monitorPointsFolder,
                                                    controlPointsFolder,
                                                    doorsFolder,
                                                    elevatorsFolder
                                                };

                            //.Add each controller folder to 
                            siteAllControllersFolders.Add(controllerFolder);

                            //Add each controller of the channel
                            //siteControllerOfAllChannels.Add(controller);                                             
                        }
                    }
                    siteCONTROLLERSFolder.Items = siteAllControllersFolders;//siteControllerOfAllChannels;

                    //--Left of initialization--
                    siteFolder.Items = new ObservableCollection<WPFTreeViewItem>() { siteCONTROLLERSFolder };//with header of CONTROLLER(S)

                    allSitesSubFolders.Add(siteFolder);
                }
                allSitesFolder.Items = allSitesSubFolders;
            }
            return new ObservableCollection<WPFTreeViewItem>() { allSitesFolder };//new ObservableCollection<object> { allSitesFolder };
             */
        }

        //private void NavigationSiteChanged(object sender, EventArgs e)
        //{
        //    NavigateToTreeNode(sender as HWTreeViewItem);
        //}  

        #endregion

        #region Business Object & Collections methods

        public static AccessBO GetObject(LinkNode hwElementNode)
        {
            if (hwElementNode.AccessObjectLink == null)
                hwElementNode.AccessObjectLink = CreateAccessObject(hwElementNode);//IDenticard.AccessUI.LinkNodeManager.Instance.CreateAccessObject(hwElementNode);
            var entity = hwElementNode.AccessObjectLink; //Tag; 

            if (entity != null)
            {
                if (entity.Link == null) //Strange, but re-associate with hwLinkNode
                    entity.Link = hwElementNode;
            }
            return entity;
        }

        public static AccessBOCollection GetObjectsCollection(LinkNode hwParentNode)
        {
            if (hwParentNode.AccessCollectionLink == null)
                hwParentNode.AccessCollectionLink = CreateAccessCollection(hwParentNode);
            return hwParentNode.AccessCollectionLink;
        }

        /// <summary>
        /// Find link especially for non-displayed Access BOs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bobject"></param>
        /// <param name="parentNode_ID"></param>
        /// <returns></returns>
        public static T GetNodeObject<T>(int parentNode_ID, int node_ID) where T : AccessBO
        {

            var bobject = default(T);
            var parentCollection = FindObjectsCollection<T>(TreeType.Hardware, parentNode_ID);
            var ien = parentCollection.GetEnumerator();//Children;
            while (ien.MoveNext())
            {
                if (((T)ien.Current).Node_ID == node_ID)
                {
                    bobject = (T)ien.Current;
                    //linkT = bobject.Link;
                    break;
                }
            }

            if (bobject != null) return bobject;
            //TODO:
            LinkNode linkT = FindObjectsCollectionLink<T>(TreeType.Hardware, parentNode_ID);

            if (linkT.Children != null)
            {
                var bLink = linkT.Children.Where(c => c.Id == node_ID).FirstOrDefault();
                return (T)GetObject(bLink);
            }

            return default(T);
        }

        /// <summary>
        /// Retrieve object information from 
        /// Find subCollection by Parent object to add        
        /// </summary>
        /// <typeparam name="T">object type to be added in sub collection of the parent node</typeparam>
        /// <param name="allTreeLinkNodes">Already exising\loaded data in the HWConfiguration tree </param>
        /// <param name="parentNodeId"></param>
        /// <returns></returns>
        public static AccessBOCollection GetObjectsCollection<T>(List<LinkNode> allTreeLinkNodes, int parentNodeId)
        {
            //Get all subCollections of the parent node
            List<LinkNode> subCollections = allTreeLinkNodes.Where(subLink => subLink.IsCollection && subLink.Id == parentNodeId).ToList();

            //Find suitable subCollection lonk having children of T type
            LinkNode TCollectionLink = subCollections.Where(subCollection => subCollection.UiId.StartsWith(typeof(T).FullName)).FirstOrDefault();

            //Get collection data
            AccessBOCollection TCollection = null;
            if (TCollectionLink != null)
                TCollection = GetObjectsCollection(TCollectionLink);

            return TCollection;
        }

        public static LinkNode FindObjectsCollectionLink<T>(TreeType treeType, int? parentNodeId) where T : AccessBO
        {
            List<LinkNode> allTreeLinkNodes = null;
            List<LinkNode> subCollections = null;
            if (treeType == TreeType.Hardware) //search in HWConfiguration Tree
            {
                allTreeLinkNodes = TreeHWConfigRecursiveNodesList; //all HW Tree nodes links to business objects
            }
            else if (treeType == TreeType.AccessSetting)
            {
                allTreeLinkNodes = TreeAccessSettingNodesList != null && TreeAccessSettingNodesList.Count > 0
                                  ? TreeAccessSettingNodesList[0].Children : null;
            }

            subCollections = allTreeLinkNodes.Where(subLink => subLink.IsCollection && subLink.UiId.StartsWith(typeof(T).FullName)).ToList();

            if (parentNodeId != null && parentNodeId > -1)//if parent node is defined - filter collection with it
                subCollections = subCollections.Where(subLink => subLink != null
                                                      && subLink.Parent.Id == parentNodeId).ToList();

            //Find the link node to the collection having items of T type
            LinkNode parentCollectionLink = subCollections.FirstOrDefault();

            return parentCollectionLink;
        }


        /// <summary>
        /// Returns a collection of T type (as result of search over build Tree : HWConfiguration)
        /// </summary>
        /// <typeparam name="T">child collection objects type</typeparam>
        /// <param name="parentNodeId">Node having collection with objects of T type </param>
        /// <returns></returns>
        public static AccessBOCollection FindObjectsCollection<T>(TreeType treeType, int? parentNodeId) where T : AccessBO
        {
            List<LinkNode> allTreeLinkNodes = null;
            List<LinkNode> subCollections = null;
            if (treeType == TreeType.Hardware) //search in HWConfiguration Tree
            {
                allTreeLinkNodes = TreeHWConfigRecursiveNodesList; //all HW Tree nodes links to business objects
            }
            else if (treeType == TreeType.AccessSetting)
            {
                allTreeLinkNodes = TreeAccessSettingNodesList != null && TreeAccessSettingNodesList.Count > 0
                                  ? TreeAccessSettingNodesList[0].Children : null;
            }

            subCollections = allTreeLinkNodes.Where(subLink => subLink.IsCollection && subLink.UiId.StartsWith(typeof(T).FullName)).ToList();

            if (parentNodeId != null && parentNodeId > -1)//if parent node is defined - filter collection with it
                subCollections = subCollections.Where(subLink => subLink != null
                                                      && subLink.Parent.Id == parentNodeId).ToList();

            //Find the link node to the collection having items of T type
            LinkNode parentCollectionLink = subCollections.FirstOrDefault();

            if (parentCollectionLink == null) return null;

            var collection = GetObjectsCollection(parentCollectionLink);
            return collection;
        }

        /// <summary>
        /// TODO: Should be replaced with IDenticard.AccessUI.LinkNodeManager.Instance.CreateAccessObject
        /// </summary>
        /// <param name="linkNode"></param>
        /// <returns></returns>
        public static AccessBO CreateAccessObject(LinkNode linkNode)
        {
            AccessBO objectToCreate = null;

            try
            {
                var uiId = linkNode.UiId;

                // Put in case for possible plugins.
                if (linkNode.Type == TreeType.DeviceDrivers) uiId = typeof(Plugin).FullName;


                Assembly assem = Assembly.Load("IDenticard.AccessUI");
                ConstructorInfo constructor = null;

                try
                {
                    constructor = assem.GetType(uiId).GetConstructor(new Type[] { typeof(Int32) });
                    objectToCreate = (AccessBO)constructor.Invoke(new object[] { linkNode.Id });
                    //new IDenticard.AccessUI.SCP(linkNode);
                }
                catch //If cannot be created 
                {
                    constructor = assem.GetType(uiId).GetConstructor(new Type[] { typeof(LinkNode) });
                    objectToCreate = (AccessBO)constructor.Invoke(new object[] { linkNode });
                }
            }
            catch
            {
                //TODO: any constructors for BOs in IDenticard.AccessUI works
            }

            return objectToCreate;
        }

        /// <summary>
        /// TODO: Should be replaced with IDenticard.AccessUI.LinkNodeManager.Instance.CreateAccessObject
        /// </summary>
        /// <param name="linkNode"></param>
        /// <returns></returns>
        public static AccessBOCollection CreateAccessCollection(LinkNode linkNode)
        {
            var uiId = linkNode.UiId;

            // Put in case for possible plugins.
            if (linkNode.Type == TreeType.DeviceDrivers && linkNode.UiId != typeof(DeviceDriverCollection).FullName)
                uiId = typeof(PluginCollection).FullName;
            Assembly assem = Assembly.Load("IDenticard.AccessUI");

            var constructor = assem.GetType(uiId).GetConstructor(new Type[] { typeof(LinkNode), typeof(Int32) });
            var objectToCreate = (AccessBOCollection)constructor.Invoke(new object[] { linkNode, GetMaximums(linkNode) });

            foreach (var childLinkNode in linkNode.Children)
            {
                if (!childLinkNode.IsCollection && childLinkNode.AccessObjectLink == null)
                {
                    childLinkNode.AccessObjectLink = CreateAccessObject(childLinkNode);
                    //objectToCreate.Add(childLinkNode.AccessObjectLink);
                }
                else if (childLinkNode.IsCollection && childLinkNode.AccessCollectionLink == null)
                {
                    // Only create child collections if they aren't a child of one of the 
                    // collections which have other collections as their direct children.
                    if (linkNode.UiId != typeof(AssetCollection).FullName &&
                        linkNode.UiId != typeof(GlobalContainerCollection).FullName &&
                        linkNode.UiId != typeof(GlobalTriggerCollection).FullName &&
                        linkNode.UiId != typeof(DeviceDriverCollection).FullName)
                    {
                        childLinkNode.AccessCollectionLink = CreateAccessCollection(childLinkNode);
                    }
                }
            }

            //var nodes = (from LinkNode node in linkNode.Children
            //            select CreateAccessObject(node));//AccessBOCollection            

            return objectToCreate;
        }

        #region Maximun TreeNodes

        /// <summary>
        /// Method returns the correct maximum based on the node.
        /// </summary>
        /// <param name="linkNode">The node</param>
        /// <returns>The maximum for this collection.</returns>
        public static int GetMaximums(LinkNode linkNode)
        {
            int maximums = 16;
            //TODO: from LinkNodeManager

            //if (linkNode.UiId == typeof(AccessGroupGlobalCollection).FullName)
            //    maximums = MaxGlobalAccessGroups;
            //else if (linkNode.UiId == typeof(MonitorPointGlobalCollection).FullName)
            //    maximums = MaxGlobalMonitorPoints;
            //else if (linkNode.UiId == typeof(ControlPointGlobalCollection).FullName)
            //    maximums = MaxGlobalControlPoints;
            //else if (linkNode.UiId == typeof(DoorGroupCollection).FullName)
            //    maximums = MaxGlobalDoors;
            //else if (linkNode.UiId == typeof(ElevatorGroupCollection).FullName)
            //    maximums = MaxGlobalElevators;
            //else if (linkNode.UiId == typeof(TriggerCollection).FullName)
            //    maximums = MaxTriggers;
            //else if (linkNode.UiId == typeof(TriggerProcedureCollection).FullName)
            //    maximums = MaxProcedures;
            //else if (linkNode.UiId == typeof(TriggerActionGroupCollection).FullName)
            //    maximums = MaxActionGroups;
            //else if (linkNode.UiId == typeof(TriggerActionCollection).FullName)
            //    maximums = MaxActions;
            //else if (linkNode.UiId == typeof(AlarmPointGroupCollection).FullName)
            //    maximums = MaxMonitorPointGroups;

            return maximums;
        }

        #endregion

        public static Site FindParentSiteFromChildLink(LinkNode currentNode)
        {
            if (currentNode == null) return null;
            //Find Site node
            var siteNode = currentNode;
            while (!siteNode.UiId.Equals(typeof(Site).FullName))
            {
                siteNode = siteNode.Parent;
                if (siteNode == null)
                    break;
            }

            //Navigate current site 
            if (siteNode != null)
            {
                if (siteNode.AccessObjectLink == null)
                    siteNode.AccessObjectLink = CreateAccessObject(siteNode);
                if (siteNode.AccessObjectLink != null)
                    return (Site)siteNode.AccessObjectLink;
            }
            return null;
        }

        public static void Create(AccessBO entity)
        {
            try
            {
                entity.Create();
            }
            catch
            {
                //TODO: show errors
            }
        }

        public static void Update(AccessBO entity)
        {
            entity.Update();
        }

        public static void Update(LinkNode link, AccessBO entity)
        {
            try
            {
                //Update collections
                link.UpdateName(entity.Name);// link.Name = entity.Name;
                //link.AccessObjectLink = entity;

                //TODO: Update the node data in collection, as linkNode is not updated automatically in tree after its name is changed
                //var parentLinks = link.Parent.Children;
                //int indexInCollection = parentLinks.IndexOf(link);
                //parentLinks[indexInCollection].Name = link.Name;
                //parentLinks[indexInCollection].AccessObjectLink = entity;

                link.UpdateLinkNodeFromObject();
                //Update in DB
                entity.Update();
            }
            catch (Exception ex)
            {
                //TODO:
            }
        }

        public static void DeleteLinkNode(LinkNode item, out LinkNode prevNode)
        {
            //TODO: Detect previos node to be navigated 
            int index = item.Parent.Children.IndexOf(item);
            prevNode = index > 0 ? item.Parent.Children[index - 1] : item.Parent;

            //Remove from collection for sure
            item.Parent.Children.Remove(item);
            //var parentCollection = GetObjectsCollection(item.Parent); //*Tag as AccessUIBase;
            //parentCollection.RemoveChild(item.AccessObjectLink);  

            //Destroy DB object
            var entity = GetObject(item);
            if (entity != null)
                entity.Delete();
        }

        #endregion

        #region Command operations (SET Defaults)

        public static AccessBO CreateTimeZone()
        {
            var assetCollection = RefreshAssetCollection();
            var timeZoneCollection = assetCollection.TimeZones;
            AccessBO newTimeZoneCreated = null;

            try
            {
                newTimeZoneCreated = (AccessBO)timeZoneCollection.AddChild();
                if (newTimeZoneCreated != null)
                {
                    var result = System.Windows.Forms.DialogResult.Abort;

                    // Display the Propertire Dialog for the new Child
                    result = newTimeZoneCreated.PropertiesDialog(null/*_imageList*/, true);
                    if (result == System.Windows.Forms.DialogResult.Cancel)
                    {
                        // If the user cancels the form, the new item must be destroyed.
                        timeZoneCollection.RemoveChild(newTimeZoneCreated);
                        newTimeZoneCreated.Delete();
                        newTimeZoneCreated = null;
                    }
                    else
                    {
                        // Set the variable
                        var newTimeZoneCreatedId = newTimeZoneCreated.Node_ID;

                        // Show a message Box about downloading for the timezone
                        System.Windows.MessageBox.Show(IDenticard.AccessUI.ResUI.TimeZoneCreatedDownloadMsg, IDenticard.AccessUI.ResUI.TimeZoneCreation, MessageBoxButton.OK);

                        // re-read the asset collection so any timezone added is found
                        // LoadTimeSelections(newTimeZoneCreatedName);
                        //_comboBoxPointSelection.ItemsSource = LoadTimeZones();

                        //_comboBoxPointSelection.SelectedValue = _timeZoneItem.Node_ID;
                        RefreshAssetCollection(); //Refresh
                    }
                }
            }
            catch (IDenticard.Data.IDDataException de)
            {
                if (AppContext.One.ExHandler.Handle(de))
                    throw;
            }
            catch (Exception ex)
            {
                //TODO:
            }

            return newTimeZoneCreated;
        }

        public static AssetCollection RefreshAssetCollection()
        {
            var accessSettingLinkNode = LinkNode.CreateTree(TreeType.AccessSetting);
            if (accessSettingLinkNode.Count > 0)
            {
                var linkNode = accessSettingLinkNode[0];
                linkNode.AccessCollectionLink = CreateAccessCollection(linkNode);
                var assetCollection = (AssetCollection)linkNode.AccessCollectionLink;
                return assetCollection;
            }
            return null;
        }

        //public static void SetDefaultChannelCommType(int defaultControllerID, int commTypeId)
        //{
        //    IDenticard.AccessUI.SCP controller = new IDenticard.AccessUI.SCP(defaultControllerID);
        //    controller.Channel.CommType_ID = commTypeId;           
        //}

        /// <summary>
        /// TODO: Stub
        /// </summary>
        /// <param name="parentLink"></param>
        /// <returns></returns>
        public static SCP CreateDefaultController(int parentLinkID)
        {
            var parentSCPCollection = FindObjectsCollection<IDenticard.AccessUI.SCP>(TreeType.Hardware, -1);
            var newDefaultController = parentSCPCollection == null ? null
                                    : parentSCPCollection.AddChild() as SCP;

            return newDefaultController;
            //return  newDefaultController ==null  ? null : IDenticard.Premisys.SCP.Read(newDefaultController.Node_ID);
        }

        public static Door CreateDefaultDoor(int parentControllerID)
        {
            var parentDoorCollection = FindObjectsCollection<Door>(TreeType.Hardware, parentControllerID);
            var newDefaultDoor = parentDoorCollection == null ? null
                                  : parentDoorCollection.AddChild() as Door;

            return newDefaultDoor;
        }

        //public static IDenticard.AccessUI.ACR CreateDefaultReader(int parentControllerID)
        //{
        //    var parentACRCollection = FindObjectsCollection<IDenticard.AccessUI.ACR>(TreeType.Hardware, parentControllerID);
        //    var newDefaultReader = parentACRCollection == null ? null
        //                          : parentACRCollection.AddChild() as IDenticard.AccessUI.ACR;

        //    return newDefaultReader;            
        //}          

        /// <summary>
        /// TODO: Stub
        /// Generate doors for hwConfiguration 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static List<LinkNode> GenerateDoors(HWDoorsConfiguration hwConfiguration)
        {
            List<LinkNode> stubNodes = new List<LinkNode>();

            for (int i = 0; i < hwConfiguration.Count; i++)
            {
                Door newDoor = hwConfiguration.DoorsCollection.AddChild() as Door;
                //TODO:
                //newDoor.AcrId = 
                //newDoor.SubType_ID = hwConfiguration.BoardType;
                //newDoor.ScpId = ;
                //newDoor.OfflineModeId

                stubNodes.Add(newDoor.Link);
            }
            return stubNodes;
        }

        #endregion

        #region Temporary IOBoard methods (should be replaced)
        /// <summary>
        /// Geta a DataSet containing available options for nex inputs, outputs or readers
        /// </summary>
        /// <remarks>
        /// Checks for SIO points allready assigned.
        /// </remarks>
        /// <returns>DataSet containing available options for nex inputs, outputs or readers</returns>
        public static DataSet GetChainedSios(IDenticard.AccessUI.SIO sio, int mode)
        {
            DataSet tempds = null;
            try
            {
                using (SqlConnection sqlconn = new SqlConnection(AppContext.One.Settings.SystemDBConnStr))
                {
                    SqlCommand Command = new SqlCommand(__SIO.SPReadNextSIO, sqlconn);
                    Command.CommandType = CommandType.StoredProcedure;

                    SqlParameter param;
                    param = new SqlParameter(__SIO.SPParamSCPID, SqlDbType.SmallInt);
                    AppContext.One.Util.Param.SetInt(param, sio.SCP_ID);
                    Command.Parameters.Add(param);

                    param = new SqlParameter(__SIO.SPParamSIOID, SqlDbType.SmallInt);
                    AppContext.One.Util.Param.SetInt(param, sio.SIO_ID);
                    Command.Parameters.Add(param);

                    param = new SqlParameter(__SIO.SPParamMode, SqlDbType.SmallInt);
                    AppContext.One.Util.Param.SetInt(param, mode);
                    Command.Parameters.Add(param);

                    SqlDataAdapter da = new SqlDataAdapter(Command);
                    tempds = new DataSet("Yada");

                    sqlconn.Open();
                    da.Fill(tempds);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //#if DEBUG
                //                if (Debugger.IsAttached) { Debugger.Break(); };
                //#endif
                //                Trace.WriteLine(Constant.LabelSource + excError.Source + Constant.Comma + Constant.LabelDescription + excError.Message);

                //                ReadDataException dataException = new ReadDataException(excError, Constant.ExceptionFailAccessBoRead, this.GetType().Name);
                //                if (AppContext.One.ExHandler.Handle(dataException))
                //                    throw dataException;
            }
            return tempds;
        }

        public static DataSet GetSIOBoardSCPChannels(SIO ioboard)
        {
            DataSet ds = null;
            try
            {
                using (var connection = new SqlConnection(AppContext.One.Settings.SystemDBConnStr))
                {
                    var command = new SqlCommand(__SCPChannel.SPReadSCPChannel, connection);
                    command.CommandType = CommandType.StoredProcedure;

                    var param = new SqlParameter(__SCPChannel.SPParamSCPID, SqlDbType.SmallInt);
                    AppContext.One.Util.Param.SetInt(param, ioboard.SCP_ID);
                    command.Parameters.Add(param);

                    var adapter = new SqlDataAdapter(command);
                    ds = new DataSet();

                    connection.Open();
                    adapter.Fill(ds);
                }
            }
            catch (Exception excError)
            {
                //#if DEBUG
                //                if (Debugger.IsAttached) { Debugger.Break(); };
                //#endif
                //                Trace.WriteLine(Constant.LabelSource + excError.Source + Constant.Comma + Constant.LabelDescription + excError.Message);

                //                var dataException = new ReadDataException(excError, Constant.ExceptionFailAccessBoRead, this.GetType().Name);
                //                if (AppContext.One.ExHandler.Handle(dataException))
                //                    throw dataException;
                //            }
            }
            return ds;

        }

        #endregion

        #region Temporary Door methods (should be replaced)

        public static DataView GetAvailablePoints(int scpId, int mode, int currentSelectedPoint, bool includePim)
        {
            var ds = Lookup.GetAvailablePoints(scpId, mode, currentSelectedPoint, includePim);

            return (ds != null && ds.Tables.Count > 0) ? SecurityPermissionHelper.CreateFilteredView(ds.Tables[0],
                __OutputPoint.ColumnOutputID, typeof(Output).ToString()) : null;
        }

        public static DataView GetStrikeModeDataView()
        {
            var ds = Lookup.GetLookUp(__StrikeMode.SPReadStrikeMode);
            return (ds != null && ds.Tables.Count > 0) ? ds.Tables[0].DefaultView : null;
        }

        #endregion

        #endregion
    }
}
