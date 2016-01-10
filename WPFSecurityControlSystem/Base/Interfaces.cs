
using System.Windows.Controls;
using IDenticard.Premisys;

namespace WPFSecurityControlSystem.Base
{
    public interface IPropertiesView
    {
        object Data { get; }
        void LoadProperties(dynamic entity);
        bool ValidateProperties();
        void SaveProperties();
    }

    public interface IView<T> where T : AccessUIBase
    {
        T Entity { get; set; }
        void Show();
        void Hide();
        void Load(T entity);
        void Save(T entity);
    }

    /// <summary>
    /// Interface for the main view which holds the current view.
    /// </summary>
    public interface IShellView
    {
        /// <summary>
        /// Gets or sets the current view.
        /// </summary>
        UserControl ContentsView { get; set; }
    }

    public interface INavigationView
    {
        /// <summary>
        /// Refresh\re-navigation item in the view.
        /// </summary>
        void Refresh(IDenticard.Access.Common.LinkNode oldNode, IDenticard.Access.Common.LinkNode newNode);
    }
}
