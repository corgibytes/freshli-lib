using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Corgibytes.Xunit.Extensions;
using Xunit;
using Xunit.Sdk;

namespace Corgibytes.Xunit.Extensions
{
    // TODO: Add tests for the behavior that's captured here
    // TODO: Extract this assembly into it's own nuget package
    // TODO: A lot of the code here is an _almost_ direct copy from https://github.com/xunit/xunit/blob/v2/src/xunit.core/MemberDataAttributeBase.cs
    //       I'd like to see us create a pull request that creates space for the differences to exist
    // TODO: Replace logger with something better
    [DataDiscoverer("Corgibytes.Xunit.Extensions.InstanceMemberDataDiscoverer", "Corgibytes.Xunit.Extensions")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InstanceMemberDataAttribute : MemberDataAttributeBase
    {
        private static Logger _logger = new Logger();

        public InstanceMemberDataAttribute(string memberName) : base(memberName, null)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // TODO: Update `DebugLog` so that it captures the caller name using `[System.Runtime.CompilerServices.CallerMemberName]`
            DebugLog($"GetDate({testMethod})");

            _ = testMethod ?? throw new ArgumentNullException(nameof(testMethod));

            // TODO: Update or write a different method that uses `[System.Runtime.CompilerServices.CallerMemberName]` to get the "name" of the thing that needs to be logged
            DebugLog($"testMethod.DeclaringType: {testMethod.DeclaringType}");
            // TODO: Update or write a different method that uses `[System.Runtime.CompilerServices.CallerMemberName]` to get the "name" of the thing that needs to be logged
            DebugLog($"this.MemberType: {this.MemberType}");
            var type = MemberType ?? testMethod.DeclaringType;
            var accessor = GetPropertyAccessor(type) ?? GetFieldAccessor(type) ?? GetMethodAccessor(type);
            if (accessor == null)
            {
                var parameterText = Parameters?.Length > 0 ? $" with parameter types: {string.Join(", ", Parameters.Select(p => p?.GetType().FullName ?? "(null)"))}" : "";
                throw new ArgumentException($"Could not find public non-static member (property, field, or method) named '{MemberName}' on {type.FullName}{parameterText}");
            }

            var obj = accessor();
            if (obj == null)
                return null;

            var dataItems = obj as IEnumerable;
            if (dataItems == null)
                throw new ArgumentException($"Property {MemberName} on {type.FullName} did not return IEnumerable");

            return dataItems.Cast<object>().Select(item => ConvertDataItem(testMethod, item));

        }

        object GetInstance(Type type)
        {
            DebugLog($"GetInstance({type})");

            Type target = type;

            // TODO: Add a unit test that covers the scenario where this expression is false
            if (type.IsAbstract && type.GenericTypeArguments.Length > 0)
            {
                target = type.GenericTypeArguments[0];
                DebugLog($"Switching activation target to: {target})");
            }

            return Activator.CreateInstance(target);
        }

        Func<object> GetFieldAccessor(Type type)
        {
            DebugLog($"GetFieldAccessor({type})");
            FieldInfo fieldInfo = null;
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
            {
                fieldInfo = reflectionType.GetRuntimeField(MemberName);
                if (fieldInfo != null)
                    break;
            }

            if (fieldInfo == null || fieldInfo.IsStatic)
                return null;

            object instance = GetInstance(type);
            return () => fieldInfo.GetValue(instance);
        }

        Func<object> GetMethodAccessor(Type type)
        {
            DebugLog($"GetMethodAccessor({type})");
            MethodInfo methodInfo = null;
            var parameterTypes = Parameters == null ? new Type[0] : Parameters.Select(p => p?.GetType()).ToArray();
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
            {
                methodInfo = reflectionType.GetRuntimeMethods()
                                           .FirstOrDefault(m => m.Name == MemberName && ParameterTypesCompatible(m.GetParameters(), parameterTypes));
                if (methodInfo != null)
                    break;
            }

            if (methodInfo == null || methodInfo.IsStatic)
                return null;

            object instance = GetInstance(type);
            return () => methodInfo.Invoke(instance, Parameters);
        }

        Func<object> GetPropertyAccessor(Type type)
        {
            DebugLog($"GetPropertyAccessor({type})");
            PropertyInfo propInfo = null;
            for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
            {
                propInfo = reflectionType.GetRuntimeProperty(MemberName);
                if (propInfo != null)
                    break;
            }

            if (propInfo == null || propInfo.GetMethod == null || propInfo.GetMethod.IsStatic)
                return null;

            object instance = GetInstance(type);
            return () => propInfo.GetValue(instance, null);
        }

        static bool ParameterTypesCompatible(ParameterInfo[] parameters, Type[] parameterTypes)
        {
            DebugLog($"ParameterTypesCompatible({parameters}, {parameterTypes})");
            if (parameters?.Length != parameterTypes.Length)
                return false;

            for (int idx = 0; idx < parameters.Length; ++idx)
                if (parameterTypes[idx] != null && !parameters[idx].ParameterType.GetTypeInfo().IsAssignableFrom(parameterTypes[idx].GetTypeInfo()))
                    return false;

            return true;
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            DebugLog($"ConvertDataItem({testMethod}, {item})");
            if (item == null)
                return null;

            var array = item as object[];
            if (array == null)
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not an object[]");

            return array;
        }

        static void DebugLog(string message)
        {
            _logger.Debug(message);
        }

        internal class Logger
        {
            bool IsDebugLogEnabled { get; }

            internal Logger()
            {
                IsDebugLogEnabled = GetDebugLogSettingFromEnvironment();
            }

            private bool GetDebugLogSettingFromEnvironment()
            {
                bool result = false;
                var environmentValue = Environment.GetEnvironmentVariable("DEBUG_CORGIBYTES_XUNIT_EXTENSIONS") ?? "False";
                Boolean.TryParse(environmentValue, out result);
                return result;
            }

            internal void Debug(string message)
            {
                if (IsDebugLogEnabled)
                {
                    System.Console.WriteLine(message);
                }
            }
        }
    }
}
