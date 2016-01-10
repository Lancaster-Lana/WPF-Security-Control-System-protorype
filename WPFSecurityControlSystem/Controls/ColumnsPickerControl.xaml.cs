using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.DTO;

namespace WPFSecurityControlSystem.Controls
{
    public partial class ColumnsPickerControl : BasePropertiesControl
    {
        #region Properties

        public override object Data
        {
            get
            {
                return AllItems;
            }
        }

        string _sourceListHeader = "Available Columns";
        public string SourceListHeader
        {
            get { return _sourceListHeader; }
            set { _sourceListHeader = value; }
        }

        string _targetListHeader = "Displayed Columns";
        public string TargetListHeader
        {
            get { return _targetListHeader; }
            set { _targetListHeader = value; }
        }

        public List<InfoColumn> AllItems { get; set; }

        public List<InfoColumn> SourceList
        {
            get
            {
                return AllItems.Where(i => !i.IsAssigned).ToList();
            }
        }

        public List<InfoColumn> TargetList
        {
            get
            {
                // if (_targetList == null)
                //     _targetList = AllItems.Where(i => i.IsAssigned).ToList();
                return AllItems.Where(i => i.IsAssigned).ToList();
            }
        }

        protected ListCollectionView TargetView
        {
            get
            {
                return System.Windows.Data.CollectionViewSource.GetDefaultView(lstTarget.ItemsSource) as ListCollectionView;//(_vm.SiteDoorsData)
            }
        }

        #endregion

        #region Contructor

        public ColumnsPickerControl()
        {
            InitializeComponent();

            this.DataContext = this; //init VM
        }

        public ColumnsPickerControl(IEnumerable<InfoColumn> allItems)
            : this()
        {
            if (allItems != null)
                AllItems = allItems.ToList();
        }

        #endregion

        #region Handlers

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var assignItems = lstSource.SelectedItems;
            if (assignItems != null)
            {
                foreach (InfoColumn data in assignItems)
                    data.IsAssigned = true;

                //Refresh lists
                OnPropertyChanged("SourceList");
                OnPropertyChanged("TargetList");
            }
        }

        private void btnAddAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (InfoColumn item in AllItems)
                item.IsAssigned = true;

            OnPropertyChanged("TargetList");
            OnPropertyChanged("SourceList");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var items = lstTarget.SelectedItems;
            if (items != null)
            {
                foreach (InfoColumn data in items)
                    data.IsAssigned = false;
                //Refresh lists
                OnPropertyChanged("SourceList");
                OnPropertyChanged("TargetList");
            }
        }

        private void btnRemoveAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (InfoColumn item in AllItems)
                item.IsAssigned = false;
            OnPropertyChanged("TargetList");
            OnPropertyChanged("SourceList");
        }

        private void btnMoveTop_Click(object sender, RoutedEventArgs e)
        {
            var item = lstTarget.SelectedItem as InfoColumn;
            if (item != null)
            {
                var index = lstTarget.SelectedIndex;
                if (index > 0)
                {
                    var prevItem = lstTarget.Items[index - 1] as InfoColumn;

                    //Find selected and previous items in the original List and swap them
                    var originalIndex = AllItems.IndexOf(item);
                    var previousIndex = AllItems.IndexOf(prevItem);
                    AllItems.RemoveAt(originalIndex);
                    AllItems.Insert(previousIndex, item); //swap 

                    OnPropertyChanged("TargetList");
                }
            }
        }

        private void btnMoveDown_Click(object sender, RoutedEventArgs e)
        {
            var item = lstTarget.SelectedItem as InfoColumn;
            var index = lstTarget.SelectedIndex;
            if (item != null)
            {
                //int oldIndex = AllItems.IndexOf(item);
                if (index < lstTarget.Items.Count - 1)
                {
                    var nextItem = lstTarget.Items[index + 1] as InfoColumn;

                    //Find selected and previous items in the original List and swap them
                    var originalIndex = AllItems.IndexOf(item);
                    var nextIndex = AllItems.IndexOf(nextItem);
                    AllItems.RemoveAt(nextIndex);
                    AllItems.Insert(originalIndex, nextItem); //swap 

                    OnPropertyChanged("TargetList");
                }
            }
        }

        #endregion
    }
}
