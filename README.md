# Hostly
A .Net Core host style builder for Xamarin apps.

![Build](https://github.com/hostly-org/hostly/workflows/Build/badge.svg)

# Getting started
## Example usage
```
new XamarinHostBuilder()
                .UseApplication<App>()
                .UseStartup<Startup>()
                .UseAppSettings<Startup>()
                .UsePlatform(this)
                .ConfigureHostConfiguration(c => c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" }))
                .Build()
                .StartAsync().Wait();
```

See the [Sample App](/samples/Hostly.Samples.Xamarin.Forms) for full exemple

## Packages
Please install all of the packages described below
### Hostly [NuGet link](https://www.nuget.org/packages/Hostly/)
This is the core package and is necessary on any Xamarin app.

#### Package manager
```
Install-Package Hostly -Version 1.1.2
```
#### .NET CLI
```
dotnet add package Hostly -Version 1.1.2
```
#### PackageReference
```
<PackageReference Include="Hostly" Version="1.1.2" />
```
#### Paket CLI
```
paket add Hostly --version 1.1.2
```
### Hostly.Android [NuGet link](https://www.nuget.org/packages/Hostly.Android/)
This is the Android platform specific package and is necessary for any Android apps

#### Package manager
```
Install-Package Hostly.Android -Version 1.1.2
```
#### .NET CLI
```
dotnet add package Hostly.Android -Version 1.1.2
```
#### PackageReference
```
<PackageReference Include="Hostly.Android" Version="1.1.2" />
```
#### Paket CLI
```
paket add Hostly.Android --version 1.1.2
```
### Hostly.iOS [NuGet link](https://www.nuget.org/packages/Hostly.iOS/)
This is the iOS platform specific package and is necessary for any iOS apps

#### Package manager
```
Install-Package Hostly.iOS -Version 1.1.2
```
#### .NET CLI
```
dotnet add package Hostly.iOS -Version 1.1.2
```
#### PackageReference
```
<PackageReference Include="Hostly.iOS" Version="1.1.2" />
```
#### Paket CLI
```
paket add Hostly.iOS --version 1.1.2
```
