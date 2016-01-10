using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using UIPrototype.Base;
using UIPrototype.Common;

namespace UIPrototype.MODULE.HWConfiguration
{
    [Export(typeof(IViewUIService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class HWConfigurationUIService : ViewUIService<HardwareConfigurationShellView>
    {
        public static ViewType GetItemTypeByParentFolder(string parentFolderName)
        {
            switch (parentFolderName.ToUpper())
            {
                case Constants.SitesFolder:
                    return ViewType.Site;
                case Constants.ControllersFolder:
                    return ViewType.Controller;
                case Constants.IOBoardsFolder:
                    return ViewType.IOBoard;
            }
            return ViewType.SetDefaults;
        }
    }

}
