using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Hostly
{
    internal static class XamarinApplicationBuilder
    {
        public static IXamarinApplication Build<TApplication>() where TApplication : class, new()
        {
            if (typeof(IXamarinApplication).IsAssignableFrom(typeof(TApplication)))
                return (IXamarinApplication)new TApplication();

            var @base = typeof(TApplication);
            var @interface = typeof(IXamarinApplication);
            var assemblyName = new AssemblyName($"{nameof(Hostly)}_{Guid.NewGuid()}");

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            var typeBuilder = assemblyBuilder.DefineDynamicModule("core")
                .DefineType($"{@interface.Name}_{Guid.NewGuid()}");

            typeBuilder.AddInterfaceImplementation(@interface);
            typeBuilder.SetParent(@base);

            return (IXamarinApplication)Activator.CreateInstance(typeBuilder.CreateTypeInfo());
        }
    }
}
