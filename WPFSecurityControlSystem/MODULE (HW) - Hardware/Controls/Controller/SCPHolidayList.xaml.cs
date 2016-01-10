using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using IDenticard.AccessUI;
using System.Data;
using UIPrototype.Services;
using IDenticard.Common.DBConstant;
using System.ComponentModel;

namespace UIPrototype.MODULE__HW____Hardware.Controls.Pages
{
    /// <summary>
    /// Interaction logic for SCPHolidayList.xaml
    /// </summary>
    public partial class SCPHolidayList : UserControl
    {
        SCP Controller { get; set; }

        public SCPHolidayList()
        {
            InitializeComponent();
        }

        public SCPHolidayList(SCP controller):this()
        {
            this.Controller = controller;
            lvHolidays.ItemsSource = GetSCPHolidays(controller);
        }

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
                //var assigned =   DataSourceHelper.GetSCPAssignedHolidays(Entity.SCP_ID);  // IsAssigned = true being checked   
                //var free = DataSourceHelper.GetSCPUnassignHolidays(Entity.SCP_ID); // IsAssigned = false
            }
          
        }

        List<DTO.Holiday> NewHolidays { get; set; }

        //public void Save()
        //{
        //     //6. Tab Holiday, IPRanges
        //    foreach (DataRowView holidayRow in CurrentHolidaysView)
        //    {
        //        //Get full onfo from hash table: NewHolidays[holidayRow.HolidayID]
        //        var holiday = NewHolidays[Convert.ToInt32(holidayRow[__Holiday.ColumnHolidayID])];
        //        //holidayRow[__Holiday.ColumnHolidayID] = holiday.HolidayID; // assign to SCP newly created holiday 
        //        //holidayRow[___Global.ColumnName] = holiday.Name;
        //        //holidayRow[__SCP.ColumnSCPID] = Entity.SCP_ID; //current
        //        //holidayRow["IsAssigned"] = true;
                
        //        //6.1. Create in DB                               
        //        var holidayCollection = DataService.GetObjectsCollectionLink<Holiday>(IDenticard.Access.Common.TreeType.AccessSetting, null);//new HolidayCollection(Entity.SCP_ID, null);
        //        var gHoliday = holidayCollection.AddChild() as Holiday;
        //        gHoliday.Name = holiday.Name;//ResConstants.HolidayPropertiesPresidentsDay;
        //        gHoliday.Description = holiday.Description;//ResConstants.HolidayPropertiesPresidentsDay;
        //        gHoliday.Date = holiday.Date;//loopDate.AddDays(-1);
        //        //gHoliday.Type =  HOLIDAY_SYS_GEN_TYPE;
        //        gHoliday.Duration = holiday.Duration;
        //        gHoliday.Update(); // not create
                 
        //        //6.2. Assign the holiday to this controller
        //        //DataRowView view = (DataRowView)lvHolidays.Items[index];               
        //        Entity.AssignHolidays.Add(gHoliday.Holiday_ID);
                 
        //    }
        //}

        private void btnAddHoliday_Click(object sender, RoutedEventArgs e)
        {
            UIPrototype.Base.BasePropertiesDialog dialog = DialogsFactory.CreateHolidayDialog(Controller.SCP_ID);
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

                    holidayRow[__SCP.ColumnSCPID] = Controller.SCP_ID; //current controller
                    holidayRow["IsAssigned"] = true; // auto assigning

                    //Save information with a new Holiday into the temporary table
                    //if (NewHolidays == null) NewHolidays = new List<DTO.Holiday>();

                    //NewHolidays.Add(holiday);

                    //1. Create in DB
                    //var gHoliday = (Holiday)_Collection.AddChild();
                    //gHoliday.Name = ResConstants.HolidayPropertiesPresidentsDay;
                    //gHoliday.Description = ResConstants.HolidayPropertiesPresidentsDay;
                    //gHoliday.Date = loopDate.AddDays(-1);
                    //gHoliday.Type = HOLIDAY_SYS_GEN_TYPE;
                    //gHoliday.Duration = 0;
                    //gHoliday.Update();
                    //2. Assign to the controller
                    //DataRowView view = (DataRowView)lvHolidays.Items[index];
                    //_controller.AssignHolidays.Add(holiday.Holiday_ID);
                }

            }
        }      


    }
}
