namespace BLL
{
    using System;
    using System.Collections.Generic;
    using System.Security.Permissions;
    using System.Text;
    using log4net;
    using log4net.Config;

    /// <summary>
    /// Looger instance class.
    /// </summary>
    public static class Logger
    {
        private static ILog log = LogManager.GetLogger("LOGGER");

        /// <summary>
        /// Gets Logger.
        /// </summary>
        public static ILog Log
        {
            get { return log; }
        }

        /// <summary>
        /// Logger intialize.
        /// </summary>
        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }

}
