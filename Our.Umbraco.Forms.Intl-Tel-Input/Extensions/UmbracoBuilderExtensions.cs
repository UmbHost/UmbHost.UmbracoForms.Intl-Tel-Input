using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.Intl_Tel_Input.Configuration;
using Our.Umbraco.Forms.Intl_Tel_Input.Fields;
using Our.Umbraco.Forums.Intl_Tel_Input;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace Our.Umbraco.Forms.Intl_Tel_Input.Extensions
{
	public static class UmbracoBuilderExtensions
	{
        public static IUmbracoBuilder AddIntlTelInput(this IUmbracoBuilder builder)
        {
            builder.WithCollectionBuilder<FieldCollectionBuilder>().Add<IntlTelInput>();
            builder.Services.Configure<IntlTelInputSettings>((IConfiguration)builder.Config.GetSection(IntlTelInputConsts.IntlTelInput));
            return builder;
        }
    }
}
