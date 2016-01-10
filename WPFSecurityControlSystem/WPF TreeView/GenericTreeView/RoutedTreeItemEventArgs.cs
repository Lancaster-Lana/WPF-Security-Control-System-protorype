using System.Windows;

namespace Controls.WpfUI.GenericTreeView
{
  public delegate void RoutedTreeItemEventHandler<T>(object sender, RoutedTreeItemEventArgs<T> e) where T : class;

  /// <summary>
  /// Event arguments for the <see cref="TreeViewBase{T}.SelectedItemChangedEvent"/>
  /// routed event.
  /// </summary>
  /// <typeparam name="T">The type of the tree's items.</typeparam>
  public class RoutedTreeItemEventArgs<T> : RoutedEventArgs where T : class
  {
    private readonly T newItem;
    private readonly T oldItem;

    /// <summary>
    /// The currently selected item that caused the event. If
    /// the tree's <see cref="TreeViewBase{T}.SelectedItem"/>
    /// property is null, so is this parameter.
    /// </summary>
    public T NewItem
    {
      get { return newItem; }
    }


    /// <summary>
    /// The previously selected item, if any. Might be null
    /// if no item was selected before.
    /// </summary>
    public T OldItem
    {
      get { return oldItem; }
    }


    /// <summary>
    /// Creates the event args.
    /// </summary>
    /// <param name="newItem">The selected item, if any.</param>
    /// <param name="oldItem">The previously selected item, if any.</param>
    public RoutedTreeItemEventArgs(T newItem, T oldItem)
      : base(TreeViewBase<T>.SelectedItemChangedEvent)
    {
      this.newItem = newItem;
      this.oldItem = oldItem;
    }
  }
}
