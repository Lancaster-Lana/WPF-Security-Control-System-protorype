using System.Collections.Generic;
using System.Windows.Data;
using WPFSecurityControlSystem.Base;
using System.Collections.ObjectModel;
using WPFSecurityControlSystem.DTO;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// Interaction logic for CardFormatsControl.xaml
    /// </summary>
    public partial class CardFormatsControl
    {
        #region Overrides

        public override object Data
        {
            get
            {
                return AllItems;
            }
        }

        public override void SaveProperties()
        {
            Data = new CollectionView(listCardFormats.ItemsSource);

            base.SaveProperties();

        }

        #endregion

        #region Properties

        ObservableCollection<InfoColumn> _allItems;
        protected ObservableCollection<InfoColumn> AllItems 
        {
            get
            {
                return _allItems;
            }
            set
            {
                _allItems = value;
                OnPropertyChanged("AllItems");
            }
        }

        #endregion

        #region Constructor

        public CardFormatsControl()
        {
            InitializeComponent();

            this.DataContext = AllItems;
        }

        public CardFormatsControl(List<InfoColumn> cardFormats):this()
        {
            AllItems = new ObservableCollection<InfoColumn>(cardFormats);
            listCardFormats.ItemsSource = AllItems;
        }

        #endregion

    }
}
