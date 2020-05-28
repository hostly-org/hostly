using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using Xamarin.Essentials;

namespace Hostly.Extensions
{
    public static class XamarinHostBuilderExtensions
    {
        public static IXamarinHostBuilder UseAppSettings(this IXamarinHostBuilder builder, Assembly assembly) 
        {
            return builder.ConfigureHostConfiguration(c =>
             {
                 c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                 c.AddEmbeddedJsonFile(assembly, "appsettings.json");
                 c.AddEmbeddedJsonFile(assembly, $"appsettings.{c.Build().GetValue<string>("environment")}.json");
             });
        }

        public static IXamarinHostBuilder UseAppSettings<TAssemblyClass>(this IXamarinHostBuilder builder)
        {
            return builder.UseAppSettings(typeof(TAssemblyClass).Assembly);
        }

        public static IXamarinHostBuilder UseApplication<TApp>(this IXamarinHostBuilder builder) where TApp : class, IXamarinApplication
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinApplication, TApp>();
            });
        }

        public static IXamarinHostBuilder UseApplication<TApp>(this IXamarinHostBuilder builder, TApp app) where TApp : IXamarinApplication
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinApplication>(app);
            });
        }

        public static IXamarinHostBuilder UsePlatform<TPlatform>(this IXamarinHostBuilder builder) where TPlatform : class, IXamarinHostingPlatform
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinHostingPlatform, TPlatform>();
            });
        }

        public static IXamarinHostBuilder UsePlatform<TPlatform>(this IXamarinHostBuilder builder, TPlatform platform) where TPlatform : IXamarinHostingPlatform
        {
            return builder.ConfigureServices((context, services) =>
            {
                services.AddSingleton<IXamarinHostingPlatform>(platform);
            });
        }

        public static IXamarinHostBuilder UseStartup(this IXamarinHostBuilder hostBuilder, Type startupType)
        {
            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    if (typeof(IXamarinStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton(typeof(IXamarinStartup), startupType);
                    }
                    else
                    {
                        services.AddSingleton(typeof(IXamarinStartup), sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IHostEnvironment>();
                            return new XamarinStartup(StartupLoader.LoadMethods(sp, startupType, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        public static IXamarinHostBuilder UseStartup<TStartup>(this IXamarinHostBuilder hostBuilder, TStartup startup) where TStartup : class
        {
            var startupType = typeof(TStartup);
            return hostBuilder
                .ConfigureServices((context, services) =>
                {
                    if (typeof(IXamarinStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton((IXamarinStartup)startup);
                    }
                    else
                    {
                        services.AddSingleton<IXamarinStartup>(sp =>
                        {
                            var hostingEnvironment = sp.GetRequiredService<IHostEnvironment>();
                            return new XamarinStartup(StartupLoader.LoadMethods(startup, hostingEnvironment.EnvironmentName));
                        });
                    }
                });
        }

        public static IXamarinHostBuilder UseStartup<TStartup>(this IXamarinHostBuilder hostBuilder) where TStartup : class, new()
        {
            return hostBuilder.UseStartup(new TStartup());
        }
    }
}
