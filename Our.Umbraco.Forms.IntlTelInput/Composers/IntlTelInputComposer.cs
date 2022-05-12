using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.IntlTelInput.Configuration;
using Our.Umbraco.Forms.IntlTelInput.Extensions;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Forms.IntlTelInput.Composers
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
