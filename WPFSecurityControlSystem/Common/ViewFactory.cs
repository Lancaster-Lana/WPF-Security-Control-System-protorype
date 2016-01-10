using System;
using System.Windows.Controls;
using System.ComponentModel.Composition.Hosting;

namespace WPFSecurityControlSystem
{
    public enum ViewType { SetDefaults, Site, Controller, IOBoard, Door, Reader }

    /// <summary>
    /// Controller
    /// </summary>
    public class ViewFactory
    {
        #region Static

        //static Hashtable _openedViews;

        //static ViewFactory()
        //{
        //    _openedViews = new Hashtable();
        //}

        //public static UserControl GreateViewFromType(ViewType viewType)
        //{           
        //    UserControl viewCtrl = null;//new BasePropertiesControl();            

        //    if (!_openedViews.ContainsKey(viewType))
        //    {
        //        switch (viewType)
        //        {
        //            case ViewType.SetDefaults:
        //                viewCtrl = new SetDefaultPropertiesControl();
        //                break;
        //            case ViewType.Site:
        //                viewCtrl = new SitePropertiesControl();
        //                break;
        //            case ViewType.Controller:
        //                viewCtrl = new SCPPropertiesControl();
        //                break;
        //            case ViewType.IOBoard:
        //                viewCtrl = new SIOPropertiesControl();
        //                break;
        //        }

        //        _openedViews.Add(viewType, viewCtrl);
        //    }
        //    else
        //        viewCtrl = (UserControl)_openedViews[viewType];

        //    return viewCtrl;
        //}

        //public static BasePropertiesControl<T> GreateViewFromType<T>(T entity) where T : class, new()
        //{
        //    BasePropertiesControl<T> viewCtrl = new BasePropertiesControl<T>(entity);                        
        //    return viewCtrl;
        //}

        #endregion

        #region Advanced

        private readonly CompositionContainer container;

        public ViewFactory(CompositionContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Returns the view that matches the supplied view name.
        /// </summary>
        /// <seealso cref="ExportViewAttribute"/>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public UserControl GetView(string viewName)
        {
            var view = this.container.GetExportedValueOrDefault<object>(viewName);
            if (view == null)
                throw new InvalidOperationException(string.Format("Unable to locate view with name {0}.", viewName));

            return view as UserControl;
        }

        #endregion

    }
}
