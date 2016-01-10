using IDenticard.Common.Wpf;
using System.Xml.Serialization;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration
{
    /// <summary>
    /// A wrapper for filterable Grid in the main area of HWConfiguration view
    /// </summary>
    public class HWBusinessObject : NotifyPropertyChangedBase // INotifyPropertyChanged,  IDataErrorInfo
    {
        [XmlAttribute("Site ID")]
        public int SiteID { get; set; }

        [XmlAttribute("Site Name")]
        public string SiteName { get; set; }

        [XmlAttribute("Comm Type")]
        public /*IDenticard.Access.Common.CommType*/string CommType { get; set; }

        [XmlAttribute("Controller Type")]
        public /*IDenticard.Access.Common.ScpType*/string ControllerType { get; set; }

        [XmlAttribute("Controller ID")]
        public int ControllerID{ get; set; }

        [XmlAttribute("Controller Name")]
        public string ControllerName { get; set; }

        [XmlAttribute("Default Mode")]
        public /*IDenticard.Access.Common.AccessReaderModes*/string DefaultMode { get; set; }

        [XmlAttribute("Offline Mode")]
        public /*IDenticard.Access.Common.AccessReaderModes*/string OfflineMode { get; set; }

        [XmlAttribute("TimeZone")]
        public string TimeZone { get; set; }

        [XmlAttribute("Door ID")]
        public short? DoorID { get; set; }

        [XmlAttribute("Door Name")]
        public string DoorName { get; set; }

        [XmlAttribute("IOBoard ID")]
        public short? IOBoardID { get; set; }

        [XmlAttribute("IOBoard Name")]
        public string IOBoardName { get; set; }
        
        [XmlAttribute("Properties")]
        public object Properties { get; set; }
                
   
         //#region INotifyPropertyChanged Members

         //public event PropertyChangedEventHandler PropertyChanged;

         //public virtual void OnPropertyChanged(string propertyName)
         //{
         //    PropertyChangedEventHandler handler = this.PropertyChanged;
         //    if (handler != null)
         //        handler(this, new PropertyChangedEventArgs(propertyName));
         //}

         //#endregion
    }
}
