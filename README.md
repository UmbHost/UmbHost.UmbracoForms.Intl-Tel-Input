# Our.Umbraco.Forms.Intl-Tel-Input

Add the [Intl-Tel-Input](https://intl-tel-input.com/) text field to your Umbraco Forms

## Getting started

This package is supported on Umbraco 9.5.0+ and Umbraco Forms 9.4.1+.

### Installation

Our.Umbraco.Forms.Intl-Tel-Input is available from [NuGet](https://www.nuget.org/packages/Our.Umbraco.Forms.Intl-Tel-Input) or as a manual download directly from GitHub.

    dotnet add package Our.Umbraco.Forms.Intl-Tel-Input

## Getting Started

If you wish to auto detect the users country, you will need to get an API key from [ipinfo.io](https://ipinfo.io/).

You will need to add the following setting to your `appsettings.json`

      "IntlTelInput": {
        "IPinfoKey": "YOUR API KEY"
      }

The following settings are available on the field type, and are set on each form within the Umbraco Forms Backoffice.

* Validation message
* Placeholder
* Initial country
* Enable / Disable auto country detection
* Restrict drop down list options to certain countries
* Specify which countries should be boosted and appear at the top of the drop down list

### Contribution guidelines

To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the package.

## License

Copyright &copy; 2022 [UmbHost Limited](https://umbhost.net/).

Licensed under the MIT License.
