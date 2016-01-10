namespace WPFSecurityControlSystem.Common
{
    public static class Constants
    {
        public const char DOT_SEPARATOR = '.';

        #region Tree Folders Names

        public const string SitesFolder = "SITE(S)";
        public const string ControllersFolder = "CONTROLLER(S)";
        public const string IOBoardsFolder = "I/O Board(s)";
        public const string MonitorPointsFolder = "Monitor Point(s)";
        public const string ControlPointsFolder = "Control Point(s)";
        public const string DoorsFolder = "Door(s)";
        public const string ElevatorsFolder = "Elevator(s)";

        #endregion

        #region Default Settings for Harware Configuration

        public const int DEFAULT_CommType = 0;//(int)IDenticard.Access.Common.CommType.NetworkOut;
        public const int DEFAULT_BaudRate = (int)IDenticard.Access.Common.BaudRates.Baud384;
        public const int DEFAULT_RTSMode = (int)IDenticard.Access.Common.RTSMode.RTS_ON;
        public const int DEFAULT_IPPort = 6005;
        public const int DEFAULT_IOTimeout = 90;
        public const int DEFAULT_GMTRegion = 12;//Eastern Time (US & Canada) (int)IDenticard.Access.Common.GMTOffset.GMT_Plus_5;
        public const int DEFAULT_CardDatabase = 0;
        public const int DEFAULT_ScpType = (int)IDenticard.Access.Common.ScpType.EP1502; //Two Reader ctrl;
        public const int DEFAULT_Protocol = (int)IDenticard.Access.Common.CardFormats.Wiegand; //CardDataFormatFlags, WiegandControlFlags
        public const int DEFAULT_ReaderMode = (int)IDenticard.Access.Common.AccessReaderModes.CardOnly;
        public const int DEFAULT_ReaderConfiguration = (int)IDenticard.Access.Common.AccessReaderConfigModes.Single;

        public const int DEFAULT_PoolDelay = 1000;
        public const int DEFAULT_IPRetryConnect = 10000;
        public const int DEFAULT_RetryCount = 5;
        public const int DEFAULT_ReplyTimeoutSerial = 200;
        public const int DEFAULT_ReplyTimeoutNetwork = 2500;

        public const int DEFAULT_TRANS_BUFFER = 4000;
        public const int DEFAULT_CardFormat = 0; // Standard 26 Bit

        public const short USER_DEFINED_HOLIDAY = 1;

        #endregion

    }
}
