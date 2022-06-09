using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Abstractions.Interfaces
{
    public interface IConfigurationManagementService
    {
        SystemConfiguration GetConfigurations();
        string GetConfiguration(string key);
        string GetConfigurationFromList(string key);
        Boolean UpdateConfiguration(string configuration, string value);
        Boolean CreateConfiguration(string configuration, string value);
        void ResetConfiguration();
    }
}
