using System;
using System.Collections;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using UIPrototype;
using UIPrototype.Base.Commands;

using IDenticard.Common.Wpf;
using IDenticard.Access.Common;

using System.ComponentModel;
using IDenticard.AccessUI;

namespace UIPrototype.Base
{
    /*
    /// <summary>
    /// Something, like IDenticard.Access.Common.LinkNode, but for WPFTree
    /// </summary>
    public class WPFTreeViewItem : NotifyPropertyChangedBase //ObservableObject<LinkNode> //  or LinkNode with AccessBO data
    {
        #region Properties

        public string ID { get; set; }

        /// <summary>
        /// The name that can be displayed or used as an
        /// ID to perform more complex styling.
        /// </summary>
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;               
                _name = value;
                OnPropertyChanged("Name");                
            }
        }

        public object Tag
        {
            get;
            set;
        }

        public WPFTreeViewItem Parent { get; set; }

        private string _imagePath;
        public string ImagePath
        {
            get
            {               
                 _imagePath = IsFolder 
                       ? ResourcesHelper.GetImagePathForHWItem(IsFolder)
                       : ResourcesHelper.GetImagePathForHWItem(Tag);                                 
                return _imagePath;
            }
        }
        public System.Drawing.Image Icon
        {
            get
            {
                System.Drawing.Image icon = System.Drawing.Image.FromFile(ImagePath);                
                return icon;
            }
        }

        bool _isFolder = false;
        public bool IsFolder
        {
            get
            {
                return _isFolder;
            }
            set
            {
                _isFolder = value;
            }
        }

        bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// The child items of the folder.
        /// </summary>
        private ObservableCollection<WPFTreeViewItem> _items;
        public ObservableCollection<WPFTreeViewItem> Items
        {
            get { return _items; }
            set
            {
                //ignore if values are equal
                if (value == _items) return;

                _items = value;
                OnPropertyChanged("Items");
            }
        }            

        #endregion

        #region Constructor

        public WPFTreeViewItem() 
        {
            
        }

        public WPFTreeViewItem(string name)
            : this()
        {           
            this.Name =  name;      
        }

        public WPFTreeViewItem(string name, bool isFolder)
            : this(name)
        {
            this.IsFolder = isFolder;
        }

        public WPFTreeViewItem(WPFTreeViewItem parentnode, string name)
            : this(name)
        {
            this.Parent = parentnode;
        }

        public WPFTreeViewItem(WPFTreeViewItem parentnode, string name, bool isFolder)
            : this(parentnode, name)
        {
            this.IsFolder = true;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        //public static implicit operator TreeViewItem(HWTreeViewItem a)
        //{
        //    TreeViewItem ti = new TreeViewItem()
        //    {  //Parent = a.Parent,
        //        Tag = a.Tag,
        //        Header = a.Name//,
        //        // Items = a.Items   
        //    };

        //   return ti;
        //}

        //public static explicit operator HWTreeViewItem(TreeViewItem a)
        //{
        //    HWTreeViewItem ti = new HWTreeViewItem()
        //    {  //Parent = a.Parent,
        //        Tag = a.Tag,
        //        Name = a.Header.ToString()//,
        //        // Items = a.Items   
        //    };

        //    return ti;
        //}

        #endregion
    }
    */
    
  /// <summary>
  /// Provides a virtual folder data structure for arbitrary
  /// child items.
  /// </summary>
  public class HWTreeViewItem : LinkNode, INotifyPropertyChanged//: TreeViewItem or AccessUIBase or AccessBO
  {
        #region Properties
     
        public short ID
        {
            get
            {
                return base.Id; 
            }
            set
            {
                base.Id = value;
                OnPropertyChanged("ID");
            }
        }

        public new string Name
        {
            get { return base.Name; }
            set
            {
                if (value == base.Name) return;

                base.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public new LinkNode Parent
        {
            get { return base.Parent; }
            set
            {
                base.Parent = value;
                OnPropertyChanged("Parent");
            }
        }

        public string NodeType { get; set; }

        public AccessBO Tag
        {
            get { return base.AccessObjectLink; }
            set
            {
                base.AccessObjectLink = value;
                OnPropertyChanged("Tag");
            }
        }

        ObservableCollection<HWTreeViewItem> _items;
        public ObservableCollection<HWTreeViewItem> Children
        {
            get { return _items; }
            set
            {
                //ignore if values are equal
                if (value == _items) return;

                _items = value;
                OnPropertyChanged("Items");
            }
        }

        //public AccessBOCollection Items
        //{
        //    get { return base.AccessCollectionLink; }
        //    set
        //    {
        //        //ignore if values are equal
        //        if (value == base.AccessCollectionLink) return;

        //        base.AccessCollectionLink = value;
        //        OnPropertyChanged("Items");
        //    }
        //}

        string _imagePath;
        public string ImagePath
        {                               
            get
            {
                if (IsFolder)
                    _imagePath = ResourcesHelper.GetImagePath("Folder.png");
                else if (Tag is Site)
                    _imagePath =  ResourcesHelper.GetImagePath("Site.png");
                else if (Tag is SCP)
                    _imagePath =  ResourcesHelper.GetImagePath("Controller.png");
                else if (Tag is SIO)
                    _imagePath =  ResourcesHelper.GetImagePath("IOBoard.png");

                return _imagePath; 
            }
        }

        public System.Drawing.Image Icon
        {
            get
            {
                return System.Drawing.Image.FromFile(ImagePath);
            }
        }
     
        public bool IsFolder
        {
            get
            {
                return base.IsCollection;
            }
            private set
            {
                base.IsCollection = value;
            }
        }

        //public new List<LinkNode> Children
        //{
        //    get { return base.Children; }
        //    set
        //    {
        //        //ignore if values are equal
        //        if (value == base.Children) return;

        //        base.Children = value;
        //        OnPropertyChanged("Items");
        //    }
        //}

        #endregion       

        #region Constructor     

        public HWTreeViewItem(LinkNode node)
        {
            this.Parent = node.Parent;
            this.IsFolder = node.IsCollection;
            this.Name = node.Name;
            this.Tag = node.AccessObjectLink;
            this.NodeType = node.UiId;
            this.Children = BuildSubTree(this, node.Children);
        }

        public static ObservableCollection<HWTreeViewItem> BuildSubTree(HWTreeViewItem wpfTreeItem, List<LinkNode> subNodes)
        {
            //var wpfTreeItem = new HWTreeViewItem(node);
            //return wpfTreeItem;
            var wpfTreeSubTree = new ObservableCollection<HWTreeViewItem>();
            if (subNodes != null)
            {               
                foreach (var subnode in subNodes)
                {
                    var subTreeItem = new HWTreeViewItem(subnode);
                    wpfTreeSubTree.Add(subTreeItem);
                }               
            }
            return wpfTreeSubTree;
        }
        #endregion
        
        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
  }

   
}