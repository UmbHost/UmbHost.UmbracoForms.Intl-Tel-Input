using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.IntlTelInput.Configuration;
using Our.Umbraco.Forms.IntlTelInput.Fields;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace Our.Umbraco.Forms.IntlTelInput.Extensions
{
	public static class UmbracoBuilderExtensions
	{
        public static IUmbracoBuilder AddIntlTelInput(this IUmbracoBuilder builder)
        {
            builder.WithCollectionBuilder<FieldCollectionBuilder>().Add<IntlTelInputField>();
            builder.Services.Configure<IntlTelInputSettings>((IConfiguration)builder.Config.GetSection(IntlTelInputConsts.IntlTelInput));
            return builder;
        }
    }
}
