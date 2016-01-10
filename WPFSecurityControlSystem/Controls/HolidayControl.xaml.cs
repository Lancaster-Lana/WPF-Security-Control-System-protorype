using System;
using IDenticard.Common.DBConstant;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.DTO;
using WPFSecurityControlSystem.Utils;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// Interaction logic for AddHoliday.xaml
    /// </summary>
    public partial class HolidayControl : BaseSingleControl<Holiday>//, IDataErrorInfo
    {
        int parent_ID = -1;

        #region Properties

        //public override object Data
        //{
        //    get
        //    {
        //        return this.Entity;
        //    }
        //}

        //DTO.Holiday _entity;
        //protected DTO.Holiday Entity
        //{
        //    get
        //    {
        //        if (_entity == null)
        //            _entity = new DTO.Holiday();// Holiday(); //no parent - then init
        //        return _entity;
        //    }
        //    set
        //    {
        //        _entity = value;
        //        OnPropertyChanged("Entity");
        //    }
        //}

        #endregion

        #region Contructor

        public HolidayControl()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();

            ErrorProvider.RegisterValidator(txtName);
            ErrorProvider.RegisterValidator<Int32>(txtAdditionalDay); //only int type
            ErrorProvider.RegisterValidator(dtpHolidayDate, ValidationFormat.Date);
        }

        //protected internal override void CreateNew(int parent_id)
        //{
        //    base.CreateNew(parent_id);
        //    Data = new DTO.Holiday();
        //}

        /// <summary>
        /// ID_HolidayCreate, ID_HolidayRead, ID_HolidayUpdate, ID_HolidayDelete
        /// ID_SCPHolidayInsert, ID_SCPHolidayDelete
        /// </summary>
        /// <param name="scp_id"></param>
        /// <returns></returns>
        //protected internal override void CreateNew(CollectionBase parentCollection)
        //{
        ////1. Crete a new holiday
        //    IDenticard.AccessUI.HolidayCollection holidaysCollection = new IDenticard.AccessUI.HolidayCollection(Parent, ID);
        //    Data = holidaysCollection.AddChild();
        //    base.CreateNew(parentCollection);
        //   //Entity = base.CreateNew(scp_id);                                          

        //    //2. Assign it to the current SCP - if save
        //   //var newHoliday = new SCPHoliday();
        //   //newHoliday.SCP_ID = (short)scp_id;
        //   //newHoliday.Holiday_ID = Entity.Holiday_ID; // join to holiday data  
        //}


        //protected internal override Holiday GetById(int id)
        //{
        //    return  Holiday.Read((short)id);
        //}

        protected override void LoadFilterableControls()
        {
            cmbHolidayType.ItemsSource = WPFSecurityControlSystem.Services.DataService.GetHolidayTypes();//dsHolidayType.Tables[0].DefaultView;
            cmbHolidayType.DisplayMemberPath = __DayFlag.ColumnName;
            cmbHolidayType.SelectedValuePath = __DayFlag.ColumnDayFlag;
            cmbHolidayType.SelectedValue = Entity.Type;
        }

        public override void LoadProperties(DTO.Holiday entity)
        {
            //this.Entity = data as SCPHoliday;
            base.LoadProperties(entity);

            //var entity = (DTO.Holiday)Data;
            txtName.Text = entity.Name;
            txtDescription.Text = entity.Description;
            txtAdditionalDay.Text = Convert.ToString(entity.Duration);
            //cmbHolidayType.Text = entity.Type;
            dtpHolidayDate.DisplayDate = entity.Date;
            return;
            /*
            //OR    
            if (Data is IDenticard.AccessUI.Holiday)     
            {
                var entity = (IDenticard.AccessUI.Holiday)Data;
                txtName.Text =  entity.Name;
                dtpHolidayDate.DisplayDate = entity.Date;
                return;
            }

            //OR
            short? entityID = -1;        
            IDenticard.Premisys.Holiday entity2 = null;
                                   
            if (Data is IDenticard.Premisys.Holiday)
                entity2 = (IDenticard.Premisys.Holiday)Data;
            else if (Data is IDenticard.Premisys.SCPHoliday)     
            {
                entityID = ((IDenticard.Premisys.SCPHoliday)Data).Holiday_ID;    
                entity2 = entityID != null ? IDenticard.Premisys.Holiday.Read((short)entityID) : null;
            }

            //Fill data
            if (entity2 == null) return;
            
            if(entity2.Holiday_Date != null)
                    dtpHolidayDate.DisplayDate = (DateTime)entity2.Holiday_Date;
            else dtpHolidayDate.Text = string.Empty;
            if (entity2 == null || entity2.Node == null) return;

            txtName.Text = (entity2 == null || entity2.Node == null) ? string.Empty : entity2.Node.Name;       
            */
        }

        public override void SaveProperties()
        {
            Entity.Name = txtName.Text;
            Entity.Description = txtDescription.Text;
            Entity.Duration = Convert.ToInt32(txtAdditionalDay.Text);
            if (dtpHolidayDate.SelectedDate != null)
                Entity.Date = (DateTime)dtpHolidayDate.SelectedDate;

            Entity.Type = Convert.ToInt32(cmbHolidayType.SelectedValue);
            Entity.IsAssigned = true;

            //TODO: GENERATE holiday id
            //int newDatabaseID = IDenticard.Access.Common.AccessCommonLookup.FindNextSmallintDatabaseID(__Holiday.TableHoliday, __Holiday.ColumnHolidayID);           
            ///Entity.Holiday_ID = Convert.ToString(newDatabaseID);

            base.SaveProperties();
            //1.Create a holiday
            //IDenticard.AccessUI.HolidayCollection collection = new IDenticard.AccessUI.HolidayCollection();                  
            //var holi = collection.AddChild() as IDenticard.AccessUI.Holiday;
            //holi.Name = txtName.Text;            

            //2. Assign it to the current SCP
            //var newHoliday = new Holiday(-1, null); // h.AddChild();
            //newHoliday.SCP_ID = this.SCP_ID;//Entity.SCP_id;
            //newHoliday.Holiday_ID = Entity.Holiday_ID; // join to holiday data           
        }
    }
}