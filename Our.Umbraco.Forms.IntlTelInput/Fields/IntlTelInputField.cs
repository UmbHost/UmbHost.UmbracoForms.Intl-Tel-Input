using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Our.Umbraco.Forms.IntlTelInput.Configuration;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace Our.Umbraco.Forms.IntlTelInput.Fields
{
    public class IntlTelInputField : FieldType
    {
        private readonly IntlTelInputSettings _config;
        public IntlTelInputField(IOptionsMonitor<IntlTelInputSettings> config)
        {
            _config = config.CurrentValue;
            Id = new Guid("a7dc31b0-651f-4f29-ada3-0244bde2a7bd");
            Name = "Intl-Tel-Input";
            Description = "Entering and validating international telephone numbers";
            Icon = "icon-phone";
            DataType = FieldDataType.String;
            SortOrder = 10;
            SupportsRegex = false;
        }
        [Setting("Validation message", Description = "The message shown when an incorrect telephone number is entered", View = "textfield")]
        public string ValidationMessage { get; set; }

        [Setting("Auto Placeholder", Description = "Set the input's placeholder to an example number for the selected country, and update it if the country changes N.B. If enabled this will replace the default placeholder set below", View = "checkbox")]
        public string AutoPlaceholder { get; set; }

        [Setting("Auto Placeholder Type", Description = "Set the input's placeholder the number type to use for the placeholder", View = "dropdownlist", PreValues = "MOBILE,FIXED_LINE,FIXED_LINE_OR_MOBILE,TOLL_FREE,PREMIUM_RATE,SHARED_COST,VOIP,PERSONAL_NUMBER,PAGER,UAN,VOICEMAIL")]
        public string AutoPlaceholderType { get; set; }

        [Setting("Default Placeholder", Description = "Set the inputs default placeholder.", View = "textfield")]
        public string Placeholder { get; set; }

        [Setting("Country based on IP address", Description = "Selects the user's country based on their IP address", View = "checkbox")]
        public string IPBasedCountry { get; set; }

        [Setting("Initial country", Description = "The default country selected in ISO2 format (e.g. GB)", View = "textfield")]
        public string InitialCountry { get; set; }

        [Setting("Preferred countries", Description = "Specify the countries to appear at the top of the list in ISO2 format (Comma separated)", View = "textfield")]
        public string PreferredCountries { get; set; }

        [Setting("Only countries", Description = "In the dropdown, display only the countries you specify in ISO2 format (Comma separated)", View = "textfield")]
        public string OnlyCountries { get; set; }

        public override string GetDesignView()
        {
            return $"{IntlTelInputConsts.PluginViewRoot}/FieldTypes/intltelinput.html";
        }

        public override string RequiredJavascriptInitialization(Field field)
        {
            var ipBasedCountry = false;
            string ipInfoKey = null;
            string initialCountry = null;
            if (field.Settings.ContainsKey("InitialCountry"))
            {
                initialCountry = field.Settings["InitialCountry"];
            }
            if (field.Settings.ContainsKey("IPBasedCountry") && !string.IsNullOrEmpty(field.Settings["IPBasedCountry"]))
            {
                ipBasedCountry = Convert.ToBoolean(field.Settings["IPBasedCountry"]);
                if (ipBasedCountry)
                {
                    initialCountry = "auto";
                }
                ipInfoKey = _config.IPinfoKey;
            }

            var autoPlaceholder = false;
            if (field.Settings.ContainsKey("AutoPlaceholder") && !string.IsNullOrEmpty(field.Settings["AutoPlaceholder"]))
            {
                autoPlaceholder = Convert.ToBoolean(field.Settings["AutoPlaceholder"]);
            }

            string placeholderType = null;
            if (field.Settings.ContainsKey("AutoPlaceholderType"))
            {
                placeholderType = field.Settings["AutoPlaceholderType"];
            }

            string preferredCountries ="null";
            if (field.Settings.ContainsKey("PreferredCountries"))
            {
                var pfSettingValue = field.Settings["PreferredCountries"];
                if (!string.IsNullOrWhiteSpace(pfSettingValue) && !string.IsNullOrEmpty(pfSettingValue))
                {
                    var pfc = field.Settings["PreferredCountries"].Split(',');
                    if (pfc.Any())
                    {
                        preferredCountries = JsonConvert.SerializeObject(pfc);
                    }
                }
            }

            string onlyCountries = "null";
            if (field.Settings.ContainsKey("OnlyCountries"))
            {
                var ocSettingValue = field.Settings["OnlyCountries"];
                if (!string.IsNullOrWhiteSpace(ocSettingValue) && !string.IsNullOrEmpty(ocSettingValue))
                {
                    var oc = field.Settings["OnlyCountries"].Split(',');
                    if (oc.Any())
                    {
                        onlyCountries = JsonConvert.SerializeObject(oc);
                    }
                }
            }
            return $"ourUmbracoFormsIntlTelInput('t{field.Id}'," +
                   $"{ipBasedCountry.ToString().ToLower()}," +
                   $"'{initialCountry.ToUpper()}'," +
                   $"{autoPlaceholder.ToString().ToLower()}," +
                   $"'{ipInfoKey}'," +
                   $"'{placeholderType}'," +
                   $"{preferredCountries}," +
                   $"{onlyCountries});";
        }

        public override IEnumerable<string> RequiredCssFiles(Field field)

        {
            var cssFiles = base.RequiredCssFiles(field).ToList();

            cssFiles.Add($"{IntlTelInputConsts.PluginCssRoot}/intlTelInput.min.css");
            cssFiles.Add($"{IntlTelInputConsts.PluginCssRoot}/our.umbraco.forms.intl-tel-input.css");

            return cssFiles;
        }

        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();

            javascriptFiles.Add($"{IntlTelInputConsts.PluginScriptRoot}/intlTelInput.min.js");
            javascriptFiles.Add($"{IntlTelInputConsts.PluginScriptRoot}/our.umbraco.forms.intl-tel-input.js");

            if (field.Settings.ContainsKey("IPBasedCountry") && field.Settings["IPBasedCountry"] == "True"
                || field.Settings.ContainsKey("AutoPlaceholder") && field.Settings["AutoPlaceholder"] == "True")
            {
                javascriptFiles.Add($"{IntlTelInputConsts.PluginScriptRoot}/utils.js");
            }

            javascriptFiles.Add($"{IntlTelInputConsts.PluginScriptRoot}/validate.phonenumber.js");

            return javascriptFiles;
        }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContext context,
            IPlaceholderParsingService placeholderParsingService, List<string> errors)
        {
            var invalidFields = new List<string>();
            var formFields = context.Request.Form;
            if (formFields.ContainsKey("phone_intl_t" + field.Id))
            {
                try
                {
                    var containsNumber = formFields.TryGetValue("phone_intl_t" + field.Id, out var submittedNumber);
                    if (containsNumber)
                    {
                        var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                        var phoneNumber = phoneNumberUtil.Parse(submittedNumber, null);
                        var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                        if (!isValid)
                        {
                            invalidFields.Add("The number entered is not valid");
                            return invalidFields;
                        }
                    }
                    else
                    {
                        invalidFields.Add("The number entered is not valid");
                        return invalidFields;
                    }
                }
                catch (Exception e)
                {
                    invalidFields.Add(e.Message);
                    return invalidFields;
                }
            }

            return base.ValidateField(form, field, postedValues, context, placeholderParsingService, errors);
        }

        //public override IEnumerable<object> ProcessSubmittedValue(Field field, IEnumerable<object> postedValues, HttpContext context)
        //{
        //    var formFields = context.Request.Form;
        //    var submittedNumber = formFields["phone_intl_t" + field.Id];
        //    if (!string.IsNullOrEmpty(submittedNumber) && !string.IsNullOrWhiteSpace(submittedNumber))
        //    {
        //        var pv = postedValues.ToList();
        //        pv.Clear();
        //        pv.Add(submittedNumber);

        //        return base.ProcessSubmittedValue(field, pv, context);
        //    }

        //    return base.ProcessSubmittedValue(field, postedValues, context);
        //}
    }
}