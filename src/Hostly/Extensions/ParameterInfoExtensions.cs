using System;
using System.Reflection;

namespace Hostly.Extensions
{
    internal static class ParameterInfoExtensions
    {
        private static readonly Type _nullable = typeof(Nullable<>);

        public static bool TryGetDefaultValue(this ParameterInfo @param, out object defaultValue)
        {
            defaultValue = null;

            try
            {
                if(@param.HasDefaultValue)
                {
                    defaultValue = @param.DefaultValue;
                    return true;
                }                    
            }
            catch (FormatException) when (@param.ParameterType == typeof(DateTime)) { }

            if(@param.ParameterType.IsValueType)
            {
                defaultValue = Activator.CreateInstance(@param.ParameterType);
                return true;
            } 

            if(@param.ParameterType.IsGenericType && @param.ParameterType.GetGenericTypeDefinition() == _nullable)
            {
                var underlyingType = Nullable.GetUnderlyingType(@param.ParameterType);
                if (underlyingType?.IsEnum ?? false)
                {
                    defaultValue = Enum.ToObject(underlyingType, defaultValue);

                    return true;
                }
            }

            return false;
        }
    }
}
