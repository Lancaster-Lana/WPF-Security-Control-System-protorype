
using IDenticard.Common.Wpf;

namespace WPFSecurityControlSystem.DTO
{
    /// <summary>
    /// The wrapper class to transfer(manipulate with) common information of any object (but mainly - Card Formats)
    /// </summary>
    public class InfoColumn : NotifyPropertyChangedBase
    {
        #region Properties

        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        bool _isAssigned;
        public bool IsAssigned
        {
            get { return _isAssigned; }
            set
            {
                _isAssigned = value;
                OnPropertyChanged("IsAssigned");
            }
        }

        #endregion

        public override bool Equals(object obj)
        {
            if (obj is InfoColumn)
            {
                return this.ID == ((InfoColumn)obj).ID;
            }

            return false;
        }
    }
}
