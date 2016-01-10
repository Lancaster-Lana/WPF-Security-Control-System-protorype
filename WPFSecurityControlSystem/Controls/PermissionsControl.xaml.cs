using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IDenticard.Access.Common;
using IDenticard.AccessUI.Wpf.Helper;
using IDenticard.Common.Security;
using IDenticard;
using IDenticard.AccessUI;
using IDenticard.Premisys;
using IDenticard.Access.Common;

namespace WPFSecurityControlSystem.Controls
{
    /// <summary>
    /// Interaction logic for PermissionsControl.xaml
    /// </summary>
    public partial class PermissionsControl : UserControl
    {
        #region Fields

        private AccessBO _accessObject;
        private bool _securityNeedsSaved = false;
        private bool _ignoringChanges = false;
        private List<GroupListItem> _groups = new List<GroupListItem>();
        private Dictionary<int, List<HardwarePermission>> _mapIdToPermissions = new Dictionary<int, List<HardwarePermission>>();
        private ListSortDirection _lastGroupSortDirection;
        private ListSortDirection _lastPermissionSortDirection;
        private GridViewColumnHeader _lastGroupSortHeader;
        private GridViewColumnHeader _lastPermissionSortHeader;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        public PermissionsControl()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Event handles loading the permissions window.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        public void LoadSecurity(AccessBO accessObject)
        {
            //Clear previous settings
            _groups.Clear();
            _mapIdToPermissions.Clear();

            //Load securoty settings
            _accessObject = accessObject;

            if (_accessObject.Link.Children.Count == 0)
                _checkBoxIsBase.Visibility = Visibility.Hidden;

            // Load the rest of the groups not already configured (new group or never setup yet).
            using (var context = new PremisysDataContext(AppContext.One.Settings.SystemDBConnStr))
            {
                var allGroups = context.ID_GetAllGroups();
                foreach (var group in allGroups)
                {
                    if (!_accessObject.MapGroupIdToSecurity.ContainsKey(group.group_id))
                    {
                        _accessObject.MapGroupIdToSecurity.Add(group.group_id, new GroupHardwareSecurity()
                        {
                            GroupId = group.group_id,
                            Name = group.display_name,
                            NameSpaceId = _accessObject.NameSpaceID,
                            DatabaseId = _accessObject.Node_ID,
                            SaveRequired = false,
                        });
                    }
                }
            }

            foreach (var groupSecurityPair in _accessObject.MapGroupIdToSecurity)
            {
                var groupSecurity = groupSecurityPair.Value;

                var view = new HardwarePermission(AccessPermissionType.View, groupSecurity.GroupId,
                    groupSecurity.ViewAllow, groupSecurity.ViewDeny, groupSecurity.IsInherited);
                var add = new HardwarePermission(AccessPermissionType.Add, groupSecurity.GroupId,
                    groupSecurity.AddAllow, groupSecurity.AddDeny, groupSecurity.IsInherited);
                var delete = new HardwarePermission(AccessPermissionType.Delete, groupSecurity.GroupId,
                    groupSecurity.DeleteAllow, groupSecurity.DeleteDeny, groupSecurity.IsInherited);
                var viewActions = new HardwarePermission(AccessPermissionType.ViewActions, groupSecurity.GroupId,
                    groupSecurity.ViewActionsAllow, groupSecurity.ViewActionsDeny, groupSecurity.IsInherited);

                var hardwarePermissions = new List<HardwarePermission>();
                hardwarePermissions.Add(add);
                hardwarePermissions.Add(delete);
                hardwarePermissions.Add(viewActions);
                hardwarePermissions.Add(view);

                _mapIdToPermissions.Add(groupSecurity.GroupId, hardwarePermissions);
                _groups.Add(new GroupListItem(groupSecurity.GroupId, groupSecurity.Name, groupSecurity.IsInherited,
                    groupSecurity.IsBase, groupSecurity.SaveRequired));
            }

            // Bind the groups.
            _listViewGroups.ItemsSource = _groups;
            _listViewGroups.SelectedIndex = 0;
            _listViewPermissions.Items.Refresh();
        }

        /// <summary>
        /// Save the security settings.
        /// </summary>
        public void SaveSecurity()
        {
            // Only attempt to save security if the user is part of the security administrators.
            if (_securityNeedsSaved && (Thread.CurrentPrincipal.IsInRole(RoleName.SecurityAdministrator) ||
                Thread.CurrentPrincipal.IsInRole(RoleName.SiteSecurityAdministrator)))
            {
                // Save this updated/new group permissions for this object and any child objects inheriting.
                foreach (var group in _groups)
                {
                    if (group.SaveRequired)
                    {
                        GroupHardwareSecurity groupSecurity = null;
                        foreach (var pairing in _accessObject.MapGroupIdToSecurity)
                        {
                            if (pairing.Key == group.Id)
                            {
                                groupSecurity = pairing.Value;
                                break;
                            }
                        }

                        if (groupSecurity == null)
                        {
                            // Create a new one.
                            groupSecurity = new GroupHardwareSecurity();
                            groupSecurity.Name = group.Name;
                            groupSecurity.GroupId = group.Id;
                            groupSecurity.NameSpaceId = _accessObject.NameSpaceID;
                            groupSecurity.DatabaseId = _accessObject.Node_ID;

                            _accessObject.MapGroupIdToSecurity.Add(groupSecurity.GroupId, groupSecurity);
                        }

                        groupSecurity.IsInherited = group.IsInheriting;
                        groupSecurity.IsBase = group.IsBase;

                        // Get the permissions for this group.
                        var groupPermissions = _mapIdToPermissions[group.Id];

                        // Update permissions for this group now.
                        foreach (var permission in groupPermissions)
                        {
                            switch (permission.Type)
                            {
                                case AccessPermissionType.View:
                                    {
                                        if (!((groupSecurity.ViewAllow == permission.Allow) &&
                                            (groupSecurity.ViewDeny == permission.Deny)))
                                        {
                                            IsViewPermissionModified = true;
                                        }
                                        groupSecurity.ViewAllow = permission.Allow;
                                        groupSecurity.ViewDeny = permission.Deny;
                                        break;
                                    }
                                case AccessPermissionType.Add:
                                    {
                                        groupSecurity.AddAllow = permission.Allow;
                                        groupSecurity.AddDeny = permission.Deny;
                                        break;
                                    }
                                case AccessPermissionType.ViewActions:
                                    {
                                        groupSecurity.ViewActionsAllow = permission.Allow;
                                        groupSecurity.ViewActionsDeny = permission.Deny;
                                        break;
                                    }
                                case AccessPermissionType.Delete:
                                    {
                                        groupSecurity.DeleteAllow = permission.Allow;
                                        groupSecurity.DeleteDeny = permission.Deny;
                                        break;
                                    }
                                default:
                                    {
                                        break;
                                    }
                            }
                        }

                        var modifyType = GroupHardwareSecurityModifyType.None;

                        if (groupSecurity.IsBase && groupSecurity.IsInherited)
                        {
                            modifyType = GroupHardwareSecurityModifyType.InheritAndPushToBase;
                        }
                        else if (groupSecurity.IsBase && !groupSecurity.IsInherited)
                        {
                            modifyType = GroupHardwareSecurityModifyType.PushToBaseOnly;
                        }
                        else if (!groupSecurity.IsBase && groupSecurity.IsInherited)
                        {
                            modifyType = GroupHardwareSecurityModifyType.InheritOnly;
                        }
                        else if (!groupSecurity.IsBase && !groupSecurity.IsInherited)
                        {
                            modifyType = GroupHardwareSecurityModifyType.SaveOnly;
                        }

                        _accessObject.Link.UpdateSecurity(groupSecurity, modifyType);

                    }   // end save required.
                }   // end loop
            }   // end save required.
        }

        #endregion

        public bool IsAnySecurityGroupViewDenyAndIsBase
        {
            get
            {
                foreach (var group in _groups)
                {
                    if (group.SaveRequired)
                    {
                        var groupPermissions = _mapIdToPermissions[group.Id];
                        var viewPermission = groupPermissions.Find(permission => permission.Type == AccessPermissionType.View);
                        if (viewPermission != null)
                        {
                            if (group.IsBase && viewPermission.Deny)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        private bool _isViewPermissionModified = false;
        public bool IsViewPermissionModified
        {
            get
            {
                return _isViewPermissionModified;
            }
            private set
            {
                _isViewPermissionModified = value;
            }
        }

        #region Events

        /// <summary>
        /// Event handles sorting the groups in the list view.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        private void GroupGridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                var sortDirection = ListSortDirection.Ascending;
                var sortColumn = (GridViewColumnHeader)e.OriginalSource;
                if (sortColumn == _lastGroupSortHeader)
                {
                    if (_lastGroupSortDirection == ListSortDirection.Ascending)
                        sortDirection = ListSortDirection.Descending;
                    else
                        sortDirection = ListSortDirection.Ascending;
                }

                // Determine the sort property.
                // GTL - would be nice to dynamically look through the data template to get to the property name
                //       instead of hard-coding the information here.  If this can be figured out, change it!
                //       Otherwise, changing the XAML could break this.
                var sortPropertyName = String.Empty;
                if (sortColumn.Content.ToString() == "Group(s)")
                    sortPropertyName = "Name";

                if (sortPropertyName != String.Empty)
                {
                    // Sort the listview.
                    _listViewGroups.Items.SortDescriptions.Clear();
                    _listViewGroups.Items.SortDescriptions.Add(new SortDescription(sortPropertyName, sortDirection));
                    _listViewGroups.Items.Refresh();

                    // Apply arrow resource to current sorted header.
                    if (sortDirection == ListSortDirection.Ascending)
                        sortColumn.Column.HeaderTemplate = _listViewGroups.Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    else
                        sortColumn.Column.HeaderTemplate = _listViewGroups.Resources["HeaderTemplateArrowDown"] as DataTemplate;

                    // Remove arrow resource from previously sorted header
                    if ((_lastGroupSortHeader != null) && (_lastGroupSortHeader != sortColumn))
                        _lastGroupSortHeader.Column.HeaderTemplate = null;

                    _lastGroupSortHeader = sortColumn;
                    _lastGroupSortDirection = sortDirection;
                }
            }
        }

        /// <summary>
        /// Event handles sorting the permission in the list view.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The event args.</param>
        private void PermissionGridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is GridViewColumnHeader)
            {
                var sortDirection = ListSortDirection.Ascending;
                var sortColumn = (GridViewColumnHeader)e.OriginalSource;
                if (sortColumn == _lastPermissionSortHeader)
                {
                    if (_lastPermissionSortDirection == ListSortDirection.Ascending)
                        sortDirection = ListSortDirection.Descending;
                    else
                        sortDirection = ListSortDirection.Ascending;
                }

                // Determine the sort property.
                // GTL - would be nice to dynamically look through the data template to get to the property name
                //       instead of hard-coding the information here.  If this can be figured out, change it!
                //       Otherwise, changing the XAML could break this.
                var sortPropertyName = String.Empty;
                if (sortColumn.Content.ToString() == "Permission")
                    sortPropertyName = "Name";

                if (sortPropertyName != String.Empty)
                {
                    // Sort the listview.
                    _listViewPermissions.Items.SortDescriptions.Clear();
                    _listViewPermissions.Items.SortDescriptions.Add(new SortDescription(sortPropertyName, sortDirection));
                    _listViewPermissions.Items.Refresh();

                    // Apply arrow resource to current sorted header.
                    if (sortDirection == ListSortDirection.Ascending)
                        sortColumn.Column.HeaderTemplate = _listViewPermissions.Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    else
                        sortColumn.Column.HeaderTemplate = _listViewPermissions.Resources["HeaderTemplateArrowDown"] as DataTemplate;

                    // Remove arrow resource from previously sorted header
                    if ((_lastPermissionSortHeader != null) && (_lastPermissionSortHeader != sortColumn))
                        _lastPermissionSortHeader.Column.HeaderTemplate = null;

                    _lastPermissionSortHeader = sortColumn;
                    _lastPermissionSortDirection = sortDirection;
                }
            }
        }

        /// <summary>
        /// Event handles the permissions being updated for this object.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void AllowDenyCheckBox_CheckedChange(object sender, RoutedEventArgs e)
        {
            // Assumed a group is selected because permissions grid would not be populated otherwise.
            if (_listViewGroups.SelectedItem != null && !_ignoringChanges)
            {
                // Flag security as having changed
                _securityNeedsSaved = true;

                var group = (GroupListItem)_listViewGroups.SelectedItem;
                group.SaveRequired = true;

                var checkBox = (CheckBox)e.Source;
                var permission = (HardwarePermission)checkBox.DataContext;

                if (checkBox.Name == "_checkBoxAllow")
                {
                    if (!permission.Allow && !permission.Deny)
                        permission.Allow = true;
                    else if (permission.Allow)
                    {
                        permission.Allow = true;
                        permission.Deny = false;
                    }
                }
                else
                {
                    if (!permission.Deny && !permission.Allow)
                        permission.Deny = true;
                    else if (permission.Deny)
                    {
                        permission.Deny = true;
                        permission.Allow = false;
                    }
                }

                _listViewPermissions.Items.Refresh();
            }
        }

        /// <summary>
        /// Event handles deselecting a group based on nothing being selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _listViewGroups_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var result = VisualTreeHelper.HitTest(_listViewGroups, e.GetPosition(_listViewGroups));

            var item = UIElementHelper.TryFindParent<ListViewItem>(result.VisualHit);
            if (item == null)
                _listViewGroups.SelectedItem = null;
        }

        /// <summary>
        /// Event handles updating the permissions based on what security group was selected.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _listViewGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Don't allow user to deselect all items. if possible
            if (_listViewGroups.SelectedItems.Count == 0 && _listViewGroups.Items.Count > 0 && e.RemovedItems.Count > 0)
            {
                _listViewGroups.SelectedItem = e.RemovedItems[0];
                return;
            }
            _checkBoxIsBase.Checked -= new RoutedEventHandler(_checkBoxIsBase_CheckedChanged);
            _checkBoxIsBase.Unchecked -= new RoutedEventHandler(_checkBoxIsBase_CheckedChanged);
            _checkBoxIsInheriting.Checked -= new RoutedEventHandler(_checkBoxIsInheriting_CheckedChanged);
            _checkBoxIsInheriting.Unchecked -= new RoutedEventHandler(_checkBoxIsInheriting_CheckedChanged);
            _ignoringChanges = true;

            if (e.AddedItems.Count > 0)
            {
                var selectedGroup = (GroupListItem)e.AddedItems[0];

                _listViewPermissions.ItemsSource = _mapIdToPermissions[selectedGroup.Id];
                _checkBoxIsInheriting.DataContext = selectedGroup;
                _checkBoxIsBase.DataContext = selectedGroup;
            }
            else
            {
                _listViewPermissions.ItemsSource = null;
                _checkBoxIsInheriting.DataContext = null;
                _checkBoxIsBase.DataContext = null;
            }

            _ignoringChanges = false;
            _checkBoxIsBase.Checked += new RoutedEventHandler(_checkBoxIsBase_CheckedChanged);
            _checkBoxIsBase.Unchecked += new RoutedEventHandler(_checkBoxIsBase_CheckedChanged);
            _checkBoxIsInheriting.Checked += new RoutedEventHandler(_checkBoxIsInheriting_CheckedChanged);
            _checkBoxIsInheriting.Unchecked += new RoutedEventHandler(_checkBoxIsInheriting_CheckedChanged);
        }

        /// <summary>
        /// Event handles checked changed event by notifying that the security needs saved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _checkBoxIsInheriting_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_listViewGroups.SelectedItem != null)
            {
                // Flag security as having changed
                _securityNeedsSaved = true;

                var group = (GroupListItem)_listViewGroups.SelectedItem;
                group.SaveRequired = true;

                if (_checkBoxIsInheriting.IsChecked.Value)
                {
                    // Get information to read the parent security.
                    var groupId = group.Id;

                    // Parent is ALWAYS an access bo collection object.
                    var parentLinkNode = _accessObject.Link.Parent;
                    var parentSecurity = GroupHardwareSecurity.CreateGroupHardwareSecurity(parentLinkNode.NamespaceId, parentLinkNode.Id, groupId);

                    foreach (HardwarePermission hardwarePermission in _listViewPermissions.Items)
                    {
                        switch (hardwarePermission.Type)
                        {
                            case AccessPermissionType.Add:
                                {
                                    hardwarePermission.Allow = parentSecurity.AddAllow;
                                    hardwarePermission.Deny = parentSecurity.AddDeny;
                                    break;
                                }
                            case AccessPermissionType.Delete:
                                {
                                    hardwarePermission.Allow = parentSecurity.DeleteAllow;
                                    hardwarePermission.Deny = parentSecurity.DeleteDeny;
                                    break;
                                }
                            case AccessPermissionType.ViewActions:
                                {
                                    hardwarePermission.Allow = parentSecurity.ViewActionsAllow;
                                    hardwarePermission.Deny = parentSecurity.ViewActionsDeny;
                                    break;
                                }
                            case AccessPermissionType.View:
                                {
                                    hardwarePermission.Allow = parentSecurity.ViewAllow;
                                    hardwarePermission.Deny = parentSecurity.ViewDeny;
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                    }

                    // Refresh permissions.
                    _listViewPermissions.Items.Refresh();
                }
            }
        }

        /// <summary>
        /// Event handles checked changed event by notifying that the security needs saved.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void _checkBoxIsBase_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (_listViewGroups.SelectedItem != null)
            {
                var group = (GroupListItem)_listViewGroups.SelectedItem;
                group.SaveRequired = true;

                // Flag security as having changed
                _securityNeedsSaved = true;
            }
        }

        #endregion
    }
}
