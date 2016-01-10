using IDenticard.Access.Common;
using IDenticard.AccessUI;

namespace WPFSecurityControlSystem.MODULE.HWConfiguration
{
    public class HWDoorsConfiguration
    {
        public DoorCollection DoorsCollection;

        //public Site Site;
        //public SCP Controller;

        /// <summary>
        /// Generating doors count
        /// </summary>
        public int Count;

        public SIOType SIOBoardType;

        //public HWDoorsConfiguration(Site site, SCP controller, __SIOType ioType, int doorsCount)
        //{ 
        //        this.Site = site;
        //        this.Controller = controller;

        //        this.DoorsCount = doorsCount;
        //        this.SIOBoardType = ioType;
        //}

        public HWDoorsConfiguration(AccessBOCollection doorsParentCollection, SIOType ioType, int doorsCount)
        {
            //this.Controller = ioBoardsParentCollection.Link.Parent.AccessObjectLink as SCP;
            this.DoorsCollection = doorsParentCollection as DoorCollection;
            this.Count = doorsCount;
            this.SIOBoardType = ioType;
        }

    }
}
