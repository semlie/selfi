using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfiguratinService
{
    public class ConfigurationSettingManager
    {

        public static string GetConfigurtionSetting(string key)
        {
            var appSettings = ConfigurationManager.AppSettings;

            return appSettings[key];
        }
    }
}
