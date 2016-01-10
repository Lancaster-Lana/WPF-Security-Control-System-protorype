using System;

namespace WPFSecurityControlSystem.DTO
{
    /// <summary>
    /// NOT business, but simple wrapper class
    /// </summary>
    public class Holiday
    {
        public string Holiday_ID;
        public short SCP_ID;
        public bool IsAssigned;

        public string Name;
        public string Description;
        public DateTime Date;
        public int Duration;
        public int Type;
    }
}
