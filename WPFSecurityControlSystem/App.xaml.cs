using System;
using System.Windows;
using System.Threading;
using System.Security.Principal;
using IDenticard;
using IDenticard.Common.Security;
using IDenticard.Premisys;
using IDenticard.Types;

namespace WPFSecurityControlSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            try
            {
                //TODO: start application - init context and user secure identity
                if (!AppContext.One.IsInitialized)
                    AppContext.One.Initialize();

                //var identicardLicense = new IDenticard.Licensing.IDenticardLicenseStub();              

                //if (AppContext.One.Settings.CheckLicensing)
                //{
                //    var client = new IDenticard.Licensing.Services.LicensingClient();
                //    client.Connect(); 
                //}

                //TODO: Initialize current identity as in PremiSys and lunch application with admin role (to call PremiSys methods)
                SysUser systemUser = SysUser.Read("Admin");
                var curIdentity = new IDIdentity(systemUser.Name,
                                systemUser.User_ID, true, AuthenticationType.Application.ToString("f"),
                                systemUser.Expires.GetValueOrDefault(DateTime.MinValue),
                                systemUser.Activation.GetValueOrDefault(DateTime.MinValue),
                                systemUser.ScreenDesign_ID.GetValueOrDefault(Int32.MinValue));

                Thread.CurrentPrincipal = new GenericPrincipal(curIdentity, new string[] { RoleName.SecurityAdministrator });


            }
            catch
            {
                //TODO:
            }
        }

     }
}
