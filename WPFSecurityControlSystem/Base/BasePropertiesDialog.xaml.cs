using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFSecurityControlSystem.Base
{
    /// <summary>
    /// Interaction logic for BasePropertiesDialog.xaml
    /// </summary>
    public partial class BasePropertiesDialog : Window //where T : AccessUIBase, new()
    {
        #region Properties

        /// <summary>
        /// The control that represent current view
        /// </summary>
        protected UserControl ContentControl
        {
            get
            {
                return (UserControl)viewPropertiesControl.Content;
            }
            private set
            {
                if (value != null && value.Parent != null)
                {
                    ((ContentControl)(value.Parent)).Content = null; //disconnect from previous parent            
                }
                viewPropertiesControl.Content = value;
            }
        }

        /// <summary>
        /// Suplementary property to acces methods of the 
        /// </summary>
        private /*IPropertiesView*/BasePropertiesControl View
        {
            get
            {
                return (BasePropertiesControl)ContentControl;
            }
        }

        /// <summary>
        /// Data of the current(loaded) business object\entity\list
        /// can be AccessBOItem
        /// </summary>       
        public object Data
        {
            get
            {
                return View != null ? View.Data : null;
            }
        }

        public int ParentNodeId { get; set; }

        #endregion

        #region Constructor

        public BasePropertiesDialog()
        {
            InitializeComponent();
        }

        //public BasePropertiesDialog CreateBasePropertiesDialog<T>(T entity) where T : class
        //{
        //    BasePropertiesDialog dlg = new BasePropertiesDialog();

        //    var entityControl = ViewFactory.GreateViewFromType<T>();
        //    this.Content = entityControl;
        //    LoadProperties(entity); 
        //    return dlg;
        //}

        //public BasePropertiesDialog(ViewType viewType)
        //    : this()
        //{
        //    this.ContentControl = ViewFactory.GreateViewFromType(viewType); 
        //    var entity = CreateNew(ParentNodeId);  //TODO: stub
        //    LoadProperties(Data);        
        //}

        #region For NEW

        public BasePropertiesDialog(BasePropertiesControl content)
            : this()
        {
            this.ContentControl = content;
            var entity = CreateNew(ParentNodeId);  //TODO: stub
            LoadProperties(Data);
        }

        //public BasePropertiesDialog(List<IDenticard.Access.Common.LinkNode> parentCollection, UserControl entityControl)
        //    : this()
        //{
        //    //this.Data = data;
        //    this.ContentControl = entityControl;
        //    var entity = CreateNew(parentCollection);
        //    LoadProperties(Data);
        //}

        public BasePropertiesDialog(int parentNodeId, UserControl content)
            : this()
        {
            this.ParentNodeId = parentNodeId; //parent object for newly added 

            //var content = ViewFactory.GreateViewFromType(viewType);//entity

            this.Title = "New ";// + viewType.ToString();

            this.ContentControl = content;  //Load view control into parent page container           

            //Create new item: site, controller, ioboard, door, rtc.
            CreateNew(parentNodeId);  //Load properties of new entity
            LoadProperties(Data);
        }


        public BasePropertiesDialog(System.Collections.CollectionBase parentCollection, UserControl entityControl)
            : this()
        {
            //this.Data = data;
            this.ContentControl = entityControl;
            var entity = CreateNew(parentCollection);
            LoadProperties(Data);
        }

        #endregion

        public BasePropertiesDialog(dynamic data, UserControl entityControl)
            : this()
        {
            //this.Data = data;
            this.ContentControl = entityControl;
            LoadProperties(data);
        }

        //public WpfPropertiesDialog(AccessBO accessBOItem, Collection<IPropertiesContainer> tabPages)
        /*
        public BasePropertiesDialog(AccessUIBase entity, UserControl entityControl)
            : this()
        {
            //var entityControl = ViewFactory.GreateViewFromType(viewType);//entity            

            this.Title = entity != null && entity.Node != null ? entity.Node.Name : entity.GetType().Name;// viewType.ToString();

            this.ContentControl = entityControl;  //Load view control into parent page container           

            LoadProperties(entity);  //Load properties to the view control                     
        }*/

        #endregion

        #region Methods

        protected virtual object CreateNew(int parentNodeId)
        {
            if (this.View != null)
                this.View.CreateNew(parentNodeId);
            return Data; //data will be initialized with a newly created object
        }

        //protected virtual object CreateNew(List<IDenticard.Access.Common.LinkNode> parentCollection)
        //protected virtual object CreateNew(IDenticard.Access.Common.AccessBOCollection parentCollection)

        protected virtual object CreateNew(System.Collections.CollectionBase parentCollection)
        {
            if (this.View != null)
                this.View.CreateNew(parentCollection); //data will be initialized with a newly created object
            return Data;
        }

        protected virtual void LoadProperties(int id)
        {
            this.View.LoadProperties(id);
        }

        protected virtual void LoadProperties(List<IDenticard.Access.Common.LinkNode> collection)
        {
            //Data = entity;
            if (this.View != null)
                this.View.LoadProperties(collection);
        }

        protected virtual void LoadProperties(dynamic entity)
        {
            //Data = entity;            
            if (this.View != null)
                this.View.LoadProperties(entity);
        }

        protected virtual void SaveProperties()
        {
            //View read properies
            if (this.View != null) //&& this.View.Validate())
                this.View.SaveProperties(); //Entity.Save();            
        }

        public virtual bool Validate()
        {
            return View == null || View.ValidateProperties();
        }

        #endregion

        #region Commands\Event Handlers

        private void OnSave(object sender, ExecutedRoutedEventArgs e)
        {
            if (Validate())
            {
                DialogResult = true;
                SaveProperties();
            }
            //else show validation errors
        }

        private void OnClose(object sender, ExecutedRoutedEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        #endregion
    }
}
