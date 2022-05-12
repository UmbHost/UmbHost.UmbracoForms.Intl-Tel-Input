using System;
using System.Configuration;

namespace ActiveIS.UmbracoForms.Intl_Tel_Input
{
    public static class AppSettingsManager
    {
        public static string GetIPinfoKey()
        {
            if (ConfigurationManager.AppSettings["IPinfoKey"] != null)
                return ConfigurationManager.AppSettings["IPinfoKey"];

            throw new Exception("\"IPinfoKey\" is missing in AppSettings.");
        }
    }
}