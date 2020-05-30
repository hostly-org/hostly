![hostly](https://github.com/hostly-org/hostly/blob/master/src/Hostly/images/icon.png)
#  Hostly
A .Net Core host style builder for Xamarin apps.

![Build](https://github.com/hostly-org/hostly/workflows/Build/badge.svg)

# Quickstart
See the [Wiki](https://github.com/hostly-org/hostly/wiki) for full documentation.
## Install Packages
1. Install Hostly

```
Install-Package Hostly -Version 1.1.2
```
2. Install platform specific package

*For Android:*
```
Install-Package Hostly.Android -Version 1.1.2
```
*For iOS:*
```
Install-Package Hostly.iOS -Version 1.1.2
```
Run the Xamarin host builder in your activity (Android) Or app delegate (iOS):
```
new XamarinHostBuilder()
                .UseApplication<App>()
                .UseStartup<Startup>()
                .UsePlatform(this)
                .Build()
                .StartAsync().Wait();
```
