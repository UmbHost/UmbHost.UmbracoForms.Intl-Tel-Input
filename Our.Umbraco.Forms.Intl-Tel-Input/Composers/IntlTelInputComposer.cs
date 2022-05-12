using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.Intl_Tel_Input.Configuration;
using Our.Umbraco.Forms.Intl_Tel_Input.Extensions;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Forms.Intl_Tel_Input.Composers
{
	public class IntlTelInputComposer : IComposer
	{
		public void Compose(IUmbracoBuilder builder)
		{
			builder.Services.AddSingleton<IntlTelInputSettings>();
            builder.AddIntlTelInput();
        }
	}
}
