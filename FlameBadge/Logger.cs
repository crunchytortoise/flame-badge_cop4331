/*
 * Logger.cs - Flame Badge
 *      -- Logs events and data to specified logfile.
 *      -- Please always try to make sure we're logging as much as possible.
 *      
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{

    // Available log levels:
    // DEBUG    =   insignificant events that are useful for debugging and viewing realtime code execution
    // INFO     =   significant, successful operations
    // WARNING  =   problem that is not immediately significant, but could cause future issues
    // ERROR    =   significant problems that should be addressed, will likely interrupt execution

    class Logger
    {
        private static String log_file = String.Format("{0}\\{1}.log", Config.project_path, Config.project_name);

        /// <summary>
        /// Logs event messages to a predefined log file.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="type">Log Level
        /// <list type="bullet"><listheader>Available Options:
        /// </listheader>
        /// <item>
        /// <description>DEBUG</description>
        /// </item>
        /// <item>
        /// <description>INFO</description>
        /// </item>
        /// <item>
        /// <description>WARNING</description>
        /// </item>
        /// <item>
        /// <description>ERROR</description>
        /// </item>
        /// </list></param>
        /// <returns>true on success, false if we run into a StreamWriter exception</returns>
        public static Boolean log(String message, String type = "info") 
        {
            String message_type = _getMessageType(type);

            if (message_type == "DEBUG" && !Config.DEBUG)
            {
                return true;
            }

            try
            {
                using (StreamWriter w = File.AppendText(log_file))
                {
                    String date = DateTime.Now.ToString("MMM dd HH:mm:ss");
                    String msg = String.Format(date + @"  [{0}] : {1}", message_type, message);
                    w.WriteLine(msg);
                }
            }
            catch (Exception e)
            {
                Logger.log(e.ToString(), "warning");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a valid log level for use in log.
        /// </summary>
        /// <param name="type">Suggested log level to be converted.</param>
        /// <returns>A valid converted log level for the <c>Logger.log</c> method.</returns>
        private static String _getMessageType(String type)
        {
            switch (type.ToUpper())
            {
                case "DEBUG":
                    return "DEBUG";

                case "INFO":
                    return "INFO";

                case "WARNING":
                    return "WARNING";

                case "ERROR":
                    return "ERROR";

                default:
                    return "INFO";
            }
        }

        /// <summary>
        /// Debug method used to dump the logfile to the stdout console.
        /// </summary>
        public static void dumpLog() 
        {
            using(StreamReader r = File.OpenText(log_file))
            {
                String line;
                while((line = r.ReadLine()) != null) 
                {
                }
            }
        }
    }
}
