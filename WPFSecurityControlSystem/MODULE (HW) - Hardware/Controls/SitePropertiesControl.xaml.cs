using IDenticard.AccessUI;
using WPFSecurityControlSystem.Base;
using System.ComponentModel.Composition;
using WPFSecurityControlSystem.Utils;

namespace WPFSecurityControlSystem.Controls
{
    [Export(typeof(Site))]
    [Export("Site")]
    public sealed partial class SitePropertiesControl : BasePropertiesControl<Site>
    {
        #region Constructor

        public SitePropertiesControl()
            : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override void RegisterVaidators()
        {
            base.RegisterVaidators();
            ErrorProvider.RegisterValidator(txtName);
        }

        public override void LoadProperties(Site entity)
        {
            base.LoadProperties(entity);

            if (Entity == null) return; //set default values

            //1. Load Security           
            Entity.ReadSecurity();
            ctrlPermissions.LoadSecurity(entity);

            //2. Fill SiteEntity data to controls
            txtName.Text = Entity.Name;
            txtDescription.Text = Entity.Description;
        }

        public override void SaveProperties()
        {
            //1.Save permissions
            ctrlPermissions.SaveSecurity();

            //2.Common Properties save
            Entity.Name = txtName.Text;
            Entity.Description = txtDescription.Text;

            base.SaveProperties(); //Entity.Save(); //to DB context                   
        }

        #endregion
    }
}
