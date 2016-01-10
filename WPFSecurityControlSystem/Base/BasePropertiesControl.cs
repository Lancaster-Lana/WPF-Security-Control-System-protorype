using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using IDenticard.Access.Common;
using WPFSecurityControlSystem.Utils;

namespace WPFSecurityControlSystem.Base
{
    public class BasePropertiesControl : UserControl, IPropertiesView, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Wraps entity to the object for manipulation with external sources
        /// </summary>
        public virtual dynamic Data { get; set; }

        protected int Parent_ID { get; set; }

        //public ErrorProvider EntityErrorProvider { get; set; }

        #endregion

        #region Constructor

        public BasePropertiesControl()
        {
            //Create error validation provider
            //EntityErrorProvider = new ErrorProvider();            
            //EntityErrorProvider.DataContext = this;
        }

        #endregion

        #region Methods

        protected virtual void RegisterVaidators()
        {
            //ErrorProvider.RegisterValidator(this, "Name",);
        }

        public virtual bool ValidateProperties()
        {
            return ErrorProvider.Validate(this);
        }

        public virtual void LoadProperties(dynamic entity)
        {
            Data = entity;
            RegisterVaidators();

            //Fill controls
            LoadFilterableControls();
        }

        protected virtual void LoadFilterableControls()
        {
        }

        public /*abstract*/ virtual void SaveProperties()
        {
            if (ValidateProperties())
            {
                //Data.Save() to database
            }
            else
                MessageBox.Show("Wrong data ! Fill the controls with proper values");
            //To the error provider summary
        }

        internal protected virtual void CreateNew(int parent_id)
        {
            Parent_ID = parent_id;
            //TODO: not always should b implemented
            //Data = HWConfigurationFactory.Create(parent_id);      
            //LoadProperties(entity); - then
        }

        internal protected virtual void CreateNew(System.Collections.CollectionBase parentCollection)
        {
            //Parent_ID = parentCollection.parent_id;
            var collection = parentCollection as AccessBOCollection;
            if (collection != null)
                Data = collection.AddChild();
            //TODO: not always should b implemented
            //Data = .Create(parent_id);      
            //LoadProperties(entity); - then
        }

        #endregion
    }

    /// <summary>
    /// Base class for wpf dialogs with single page
    /// </summary>
    public class BaseSingleControl<T> : BasePropertiesControl where T : class, new()
    {
        #region Overrides

        protected override void RegisterVaidators()
        {
            //if (Entity != null)
            //    ErrorProvider.RegisterValidator(this, Entity.Node.Name, ValidationFormat.Required);            
            //ErrorProvider.RegisterValidator(this, "Name", ValidationFormat.Required); //find in the visual tree UIElement with this binding   
            //Then others
        }

        #endregion

        #region Properties

        protected T Entity
        {
            get
            {
                return base.Data as T;
            }
            set
            {
                base.Data = value;
                OnPropertyChanged("Entity");
            }
        }

        #endregion

        #region Constructor

        public BaseSingleControl()
            : base()
        {
            //LoadFilterableControls();
            this.DataContext = Entity; //Mainly          
        }

        public BaseSingleControl(T entity)
            : this()
        {
            LoadProperties(entity);
        }

        #endregion

        #region Methods

        internal protected override void CreateNew(int parent_id)
        {
            Parent_ID = parent_id;
            T entity = new T();

            LoadProperties(entity);
        }

        /// <summary>
        /// Sealed - cannot be override, but instead LoadProperties(T entity)
        /// </summary>
        /// <param name="entity"></param>
        public override sealed void LoadProperties(dynamic entity)
        {
            var entConvert = entity as T; //Load the rest of all properties

            if (entConvert != null)
            {
                //Load common base object properties
                base.LoadProperties(entConvert); //txtName.Text = Entity.Name; etc.

                //Load rest (child class) properties
                this.LoadProperties(entConvert);
            }
        }

        public virtual void LoadProperties(T entity)
        {
            Entity = entity;
            //TODO:
            //txtTimeZone.Text = Entity.TimeZone;
            // etc.         
        }

        public override void SaveProperties()
        {
            //_entity.Node.Name = txtName.Text;
            //_entity.Id = 
            //_entity.Node = 

            if (ValidateProperties())
            {
                //Entity.Save() - to DB
            }
            else
            {
                MessageBox.Show("Wrong data ! Fill the controls with proper values");
                //Show error summary
            }
        }

        #endregion
    }

    /// <summary>
    /// Base class for wpf dialogs with properties of Access Bbusiness Objects (inherited IDenticard.Access.Common.AccessBO)
    /// </summary>
    public class BasePropertiesControl<T> : BasePropertiesControl where T : AccessBO
    {
        #region Overrides

        /// <summary>
        /// Wraps entity type to object to manipulate with external
        /// </summary>
        //public override object Data
        //{
        //    get
        //    {
        //        return this.Entity;
        //    }
        //}

        protected override void RegisterVaidators()
        {
            //if (Entity != null)
            //    ErrorProvider.RegisterValidator(this, Entity.Node.Name, ValidationFormat.Required);            
            //ErrorProvider.RegisterValidator(this, "Name", ValidationFormat.Required); //find in the visual tree UIElement with this binding   
            //Then others
        }

        #endregion

        #region Properties

        //T _entity;
        protected T Entity
        {
            get
            {
                return base.Data as T;
            }
            set
            {
                base.Data = value;
                OnPropertyChanged("Entity");
            }
        }

        System.Collections.CollectionBase _parentCollection = null;

        #endregion

        #region Constructor

        public BasePropertiesControl()
            : base()
        {
            //LoadFilterableControls();
            this.DataContext = Entity; //Mainly          
        }

        public BasePropertiesControl(System.Collections.CollectionBase parentCollection)
            : this()
        {
            //LoadFilterableControls();
            _parentCollection = parentCollection;
        }

        public BasePropertiesControl(int id)
            : this()
        {
            var entity = GetById(_parentCollection, id);
            LoadProperties(entity);
        }

        public BasePropertiesControl(T entity)
            : this()
        {
            LoadProperties(entity);
        }

        #endregion

        #region Methods

        internal protected override void CreateNew(System.Collections.CollectionBase parentCollection)
        {
            var collection = parentCollection as AccessBOCollection;
            Data = collection.AddChild();
        }

        internal protected virtual T GetById(System.Collections.CollectionBase parentCollection, int id)
        {
            if (parentCollection == null) return null;

            T entity = null;
            var ien = parentCollection.GetEnumerator();
            while (ien.MoveNext())
            {
                dynamic curr = ien.Current;
                if (curr is AccessBO)
                {
                    if (curr.Node_ID == id)
                    {
                        entity = curr as T;
                        break;
                    }
                }
                else if (curr.Id == id) 
                {
                    //entity = curr as T;//TODO: Id property should be present 
                    break;
                }
            }
            return entity;
        }

        public virtual void LoadProperties(int id)
        {
            var entity = GetById(_parentCollection, id);
            // Node.GetNode(X, id);
            LoadProperties(entity);
        }

        /// <summary>
        /// Sealed - cannot be override, but instead LoadProperties(T entity)
        /// </summary>
        /// <param name="entity"></param>
        public override sealed void LoadProperties(dynamic entity)
        {
            var entConvert = entity as T; //Load the rest of all properties

            if (entConvert != null)
            {
                //Load common base object properties
                base.LoadProperties(entConvert); //txtName.Text = Entity.Name; etc.

                //Load rest (child class) properties
                this.LoadProperties(entConvert);
            }
        }

        public virtual void LoadProperties(T entity)
        {
            Entity = entity;
            //TODO:
            //txtTimeZone.Text = Entity.TimeZone;
            // etc.       
            //LoadSecutity();
        }

        public override void SaveProperties()
        {
            //_entity.Node.Name = txtName.Text;
            //_entity.Id = 
            //_entity.Node = 

            if (/*Entity.*/ValidateProperties())
            {
                //Entity.Save() - to DB
            }
            else
            {
                MessageBox.Show("Wrong data ! Fill the controls with proper values");
                //Show error summary
            }
        }

        #endregion
    }

    //public class WpfHWPropertiesControl<T> : BasePropertiesControl<T> where T : AccessBO, new()
    //{
    //    protected internal virtual void CreateNew(AccessBOCollection parentCollection)
    //    {  
    //        T entity = parentCollection.AddChild() as T;
    //        //Data = entity;
    //        LoadProperties(entity);
    //    } 
    //}
}
