using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using NPoco.fastJSON;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;

namespace ActiveIS.UmbracoForms.Intl_Tel_Input.Fields
{
    public class IntlTelInput : FieldType
    {
        public IntlTelInput()
        {
            Id = new Guid("a7dc31b0-651f-4f29-ada3-0244bde2a7bd");
            Name = "Intl-Tel-Input";
            Description = "Entering and validating international telephone numbers";
            Icon = "icon-phone";
            DataType = FieldDataType.String;
            SortOrder = 10;
            SupportsRegex = false;
        }

        [Setting("Auto Placeholder", Description = "Set the input's placeholder to an example number for the selected country, and update it if the country changes", View = "checkbox")]
        public string AutoPlaceholder { get; set; }

        [Setting("Placeholder", Description = "Set the input's placeholder. N.B. This will override the Auto Placeholder setting above", View = "textfield")]
        public string Placeholder { get; set; }

        [Setting("Country based on IP address", Description = "Selects the user's country based on their IP address", View = "checkbox")]
        public string IPBasedCountry { get; set; }

        [Setting("Initial IP based country", Description = "The default country for the ip address look up", View = "textfield")]
        public string InitialIPCountry { get; set; }

        public override string GetDesignView()
        {
            return $"{Consts.PluginViewRoot}/FieldTypes/intltelinput.html";
        }

        public override string RequiredJavascriptInitialization(Field field)
        {
            //return $"window.intlTelInput(document.querySelector(\"#{field.Id}\"));";

            var ipBasedCountry = false;
            string ipInfoKey = null;
            if (field.Settings.ContainsKey("IPBasedCountry") && !string.IsNullOrEmpty(field.Settings["IPBasedCountry"]))
            {
                ipBasedCountry = Convert.ToBoolean(field.Settings["IPBasedCountry"]);
                ipInfoKey = AppSettingsManager.GetIPinfoKey();
            }

            var initialIPBasedCountry = "auto";
            if (field.Settings.ContainsKey("InitialIPCountry") && !string.IsNullOrEmpty(field.Settings["InitialIPCountry"]))
            {
                initialIPBasedCountry = field.Settings["IPBasedCountry"];
            }

            var autoPlaceholder = false;
            if (field.Settings.ContainsKey("AutoPlaceholder") && !string.IsNullOrEmpty(field.Settings["AutoPlaceholder"]))
            {
                autoPlaceholder = Convert.ToBoolean(field.Settings["AutoPlaceholder"]);
            }

            return $"activeisUmbracoFormsIntlTelInput(document.querySelector(\"#{field.Id}\")," +
                   $"{ipBasedCountry.ToString().ToLower()}," +
                   $"'{initialIPBasedCountry}'," +
                   $"{autoPlaceholder.ToString().ToLower()}," +
                   $"'{ipInfoKey}');";
        }

        public override IEnumerable<string> RequiredCssFiles(Field field)

        {
            var cssFiles = base.RequiredCssFiles(field).ToList();

            cssFiles.Add($"{Consts.PluginCssRoot}/intlTelInput.min.css");
            cssFiles.Add($"{Consts.PluginCssRoot}/activeis.umbracoforms.intl-tel-input.css");

            return cssFiles;
        }

        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();

            javascriptFiles.Add($"{Consts.PluginScriptRoot}/intlTelInput.min.js");
            javascriptFiles.Add($"{Consts.PluginScriptRoot}/activeis.umbracoforms.intl-tel-input.js");

            if (field.Settings.ContainsKey("IPBasedCountry") && field.Settings["IPBasedCountry"] == "True"
                || field.Settings.ContainsKey("AutoPlaceholder") && field.Settings["AutoPlaceholder"] == "True")
            {
                javascriptFiles.Add($"{Consts.PluginScriptRoot}/utils.js");
            }

            return javascriptFiles;
        }
    }
}