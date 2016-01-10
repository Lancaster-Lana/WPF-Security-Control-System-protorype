using System.Collections.Generic;
using Controls.WpfUI.GenericTreeView;
using IDenticard.Access.Common;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration.Controls
{
    /// <summary>
    /// HW tree structure class
    /// </summary>
    public class HWConfigurationTree : TreeViewBase<LinkNode>//<LinkNode>, INotifyPropertyChanged, HierarchicalData
    {
        public override string GetItemKey(LinkNode item)
        {
            return item.UiId + item.Id;
        }

        public override ObservableCollection<LinkNode> GetChildItems(LinkNode parent)
        {
            return new ObservableCollection<LinkNode>(parent.Children); //TODO:          
        }

        public override LinkNode GetParentItem(LinkNode item)
        {
            return item.Parent; //TODO:
        }

        /// <summary>
        /// Force to refresh TreeNode, if T (its object type) is not obseverable type !
        /// </summary>
        /// <param name="navigationItem"></param>
        public void RefreshNode(LinkNode navigationItem)
        {
            TreeViewItem refreshNode = TryFindNode(navigationItem);
            refreshNode.Header = navigationItem.AccessObjectLink;
            //refreshNode.Tag = navigationItem;

            //refreshNode.Items.Refresh();
            //((TreeViewItem)Tree.SelectedItem).Header = navigationNode.AccessObjectLink;
        }

        public void Navigate(LinkNode navigationItem)
        {
            //TreeViewItem refreshNode = TryFindNode(navigationItem);
            //if (refreshNode != null)
            //   refreshNode.IsSelected = true;
            this.SelectedItem = navigationItem;
        }


        #region debugging properties

        public HWConfigurationTree()
        {
            //Monitor.ChildCollections
            //Monitor.MonitoredCollectionChanged += delegate { CountNodesAndCollections(); };
        }

        public override void Refresh(TreeLayout layout)
        {
            base.Refresh(layout);
            //CountNodesAndCollections();
        }

        protected override void OnNodeCollapsed(TreeViewItem treeNode)
        {
            base.OnNodeCollapsed(treeNode);
            //CountNodesAndCollections();
        }

        protected override void OnNodeExpanded(TreeViewItem treeNode)
        {
            base.OnNodeExpanded(treeNode);
            //CountNodesAndCollections();
        }

        protected override void OnItemsPropertyChanged(IEnumerable<LinkNode> oldItems,
                                                       IEnumerable<LinkNode> newItems)
        {
            base.OnItemsPropertyChanged(oldItems, newItems);
            // CountNodesAndCollections();
        }

        /*
        #region TreeNodeCount dependency property

        /// <summary>
        /// Updates the debugging dependency properties.
        /// </summary>
        internal void CountNodesAndCollections()
        {
            int count = 0;
            foreach (TreeViewItem item in RecursiveNodeList)
            {
                count++;
            }
            TreeNodeCount = count;

            ObservedCollectionCount = Monitor.ChildCollections.Count;
        }


        /// <summary>
        /// Counts the currently rendered tree nodes.
        /// </summary>
        public static readonly DependencyProperty TreeNodeCountProperty 
            = DependencyProperty.Register("TreeNodeCount", typeof(int), typeof(LinkNode), new FrameworkPropertyMetadata(-1));

        /// <summary>
        /// A property wrapper for the <see cref="TreeNodeCountProperty"/>
        /// dependency property:<br/>
        /// Counts the currently rendered tree nodes.
        /// </summary>
        public int TreeNodeCount
        {
            get { return (int)GetValue(TreeNodeCountProperty); }
            set { SetValue(TreeNodeCountProperty, value); }
        }

        #endregion

        #region ObservedCollectionCount dependency property
        /// <summary>
        /// Reflects the number of collections
        /// </summary>
        public static readonly DependencyProperty ObservedCollectionCountProperty
            = DependencyProperty.Register("ObservedCollectionCount", typeof(int), typeof(LinkNode), new FrameworkPropertyMetadata(0));


        /// <summary>
        /// A property wrapper for the <see cref="ObservedCollectionCountProperty"/>
        /// dependency property:<br/>
        /// Reflects the number of collections
        /// </summary>
        public int ObservedCollectionCount
        {
            get { return (int)GetValue(ObservedCollectionCountProperty); }
            set { SetValue(ObservedCollectionCountProperty, value); }
        }

        #endregion
       
        */
        #endregion

    }
}
