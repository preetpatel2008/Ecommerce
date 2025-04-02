using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{
    public  class ConfigurationModel
    {
        public int ConfigurationSettingsID { get; set; } = 0;
        public string ConfigurationSettingsKey { get; set; } = string.Empty;
        public string ConfigurationSettingsValue { get; set; } = string.Empty;
        public List<ConfigurationModel> lstConfigurationSettings { get; set; }
    }
}
