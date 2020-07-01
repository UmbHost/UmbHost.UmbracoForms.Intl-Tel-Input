using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;
using NPoco.fastJSON;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Data.Storage;
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
        [Required]
        public string InitialCountry { get; set; }

        public override string GetDesignView()
        {
            return $"{Consts.PluginViewRoot}/FieldTypes/intltelinput.html";
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
                ipInfoKey = AppSettingsManager.GetIPinfoKey();
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

            return $"activeisUmbracoFormsIntlTelInput('{field.Id}'," +
                   $"{ipBasedCountry.ToString().ToLower()}," +
                   $"'{initialCountry}'," +
                   $"{autoPlaceholder.ToString().ToLower()}," +
                   $"'{ipInfoKey}'," +
                   $"'{placeholderType}');";
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

            javascriptFiles.Add($"{Consts.PluginScriptRoot}/validate.phonenumber.js");

            return javascriptFiles;
        }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContextBase context,
            IFormStorage formStorage)
        {
            var invalidFields = new List<string>();
            var formFields = context.Request.Form;
            if (formFields["phone_intl_" + field.Id] != null)
            {
                try
                {
                    var submittedNumber = formFields.Get("phone_intl_" + field.Id);
                    var phoneNumberUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
                    var phoneNumber = phoneNumberUtil.Parse(submittedNumber, null);
                    var isValid = phoneNumberUtil.IsValidNumber(phoneNumber);

                    if (!isValid)
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

            return base.ValidateField(form, field, postedValues, context, formStorage);
        }

        public override IEnumerable<object> ProcessSubmittedValue(Field field, IEnumerable<object> postedValues, HttpContextBase context)
        {
            var formFields = context.Request.Form;
            var submittedNumber = formFields.Get("phone_intl_" + field.Id);
            if (!string.IsNullOrEmpty(submittedNumber) && !string.IsNullOrWhiteSpace(submittedNumber))
            {
                var pv = postedValues.ToList();
                pv.Clear();
                pv.Add(submittedNumber);

                return base.ProcessSubmittedValue(field, pv, context);
            }

            return base.ProcessSubmittedValue(field, postedValues, context);
        }
    }
}