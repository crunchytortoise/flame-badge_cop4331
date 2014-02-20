/*
 * Config.cs - Flame Badge
 *      -- Parses the config file and exposes static methods
 *      -- that other classes can use.
 *      
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    class Config
    {
        public static String project_name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
        public static String project_path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static String project_bin = String.Format(@"{0}\bin", project_path);
        private static String project_conf = String.Format(@"{0}\{1}.conf", project_path, project_name);

        public static Boolean DEBUG = false;
        public static Int16 NUM_PLAYER = 6;
        public static Int16 NUM_CPU = 6;

        private Boolean _parseConfig()
        {
            return true;
        }
    }
}
