//===================================================================================
// Microsoft patterns & practices
// Composite Application Guidance for Windows Presentation Foundation and Silverlight
//===================================================================================
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Windows.Controls;
using UIPrototype;

namespace UIPrototype.Base
{
    /// <summary>
    /// A base class for UI views manipulation
    /// </summary>
    /// <typeparam name="TMainView"><see cref="IMainView"/> type</typeparam>
    /// <seealso cref="IViewUIService"/>
    /// <seealso cref="IState{T}"/>
    /// <seealso cref="ICurrentState{T}"/>
    public abstract class ViewUIService<TMainView> : IViewUIService
        where TMainView : UserControl, IMainView, new()
    {
        private readonly TMainView mainWindow;

        [Import(typeof(ViewFactory))]
        public ViewFactory ViewFactory { get; set; }

        protected ViewUIService()
        {
            this.mainWindow = new TMainView();
        }

       // [Import(typeof(StateHandler))]
       // public StateHandler StateHandler { get; set; }

        /// <summary>
        /// Gets the MainWindow which hosts the current view;
        /// </summary>
        public IMainView MainWindow
        {
            get
            {
                return this.mainWindow;
            }
        }

        /// <summary>
        /// Shows the view identified by <see cref="viewName"/> as the current view.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        public void ShowView(string viewName)
        {
            var view = this.ViewFactory.GetView(viewName);
            this.MainWindow.CurrentView = view;
        }

        /// <summary>
        /// Shows the view identified by <see cref="viewName"/> as the current view, making <paramref name="context"/>
        /// available to views and view models.
        /// </summary>
        /// <remarks>
        /// <paramref name="context"/> is a plain object. It must be imported by the view associated to 
        /// <paramref name="viewName"/> or its view model. See <see cref="IState{T}"/> for details.
        /// </remarks>
        /// <typeparam name="T">The type for the <paramref name="context"/>.</typeparam>
        /// <param name="viewName">The view name.</param>
        /// <param name="context">Context information required by the target view.</param>
        public void ShowView<T>(string viewName, T context)
        {
            //var previousValue = StateHandler.SetState(context);

            try
            {
                this.ShowView(viewName);
            }
            finally
            {
                //StateHandler.SetState(previousValue);
            }
        }
    }
}
