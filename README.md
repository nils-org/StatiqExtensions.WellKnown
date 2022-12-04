# StatiqExtensions.WellKnown

[![standard-readme compliant][]][standard-readme]
[![Contributor Covenant][contrib-covenantimg]][contrib-covenant]
[![Build][buildimage]][build]
[![Codecov Report][codecovimage]][codecov]
[![NuGet package][nugetimage]][nuget]

A set of [Statiq](https://www.statiq.dev) helpers to create files in `.well-known/`

## Table of Contents

- [Install](#install)
- [Usage](#usage)
- [Maintainer](#maintainer)
- [License](#license)

## Install

```ps
dotnet add package StatiqExtensions.WellKnown
```

## Usage

### `.well-known/webfinger` for fediverse account

Create a [WebFinger](https://webfinger.net) file
that contains static content and "redirects" to a
real account in the fediverse.

```cs
await Bootstrapper
    .Factory
    .CreateWeb(args)
    .WithWellKnown(x => x
        .WebFingerAlias("nils_andresen@mastodon.social"))
    .RunAsync();
```

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

StatiqExtensions.WellKnown follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

This project follows the [all-contributors][] specification. Contributions of any kind welcome!


## License

[MIT License © Nils Andresen][license]

[build]: https://github.com/nils-org/StatiqExtensions.WellKnown/actions/workflows/build.yml
[buildimage]: https://github.com/nils-org/StatiqExtensions.WellKnown/actions/workflows/build.yml/badge.svg
[codecov]: https://codecov.io/gh/nils-org/StatiqExtensions.WellKnown
[codecovimage]: https://img.shields.io/codecov/c/github/nils-org/StatiqExtensions.WellKnown.svg?logo=codecov&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/2/0/code_of_conduct/
[contrib-covenantimg]: https://img.shields.io/badge/Contributor%20Covenant-v2.0%20adopted-ff69b4.svg
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/StatiqExtensions.WellKnown
[nugetimage]: https://img.shields.io/nuget/v/StatiqExtensions.WellKnown.svg?logo=nuget&style=flat-square
[license]: LICENSE.md
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
