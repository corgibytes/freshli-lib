using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Corgibytes.Freshli.Lib;
using Elasticsearch.Net;
using Xunit;
using Xunit.Sdk;

namespace Corgibytes.Freshli.Lib.Test.Integration
{
    public abstract class RepositoryTestFixture<T>
    {
        public abstract IPackageRepository Repository { get; }

        public abstract TheoryData<IList<string>, IList<int>, string> DataForTestingVersionInfo { get; }

        [Theory]
        [InstanceMemberData(nameof(DataForTestingVersionInfo))]
        public void VersionInfo(
            string[] methodParams,
            int[] expectedDateParts,
            string expectedVersion
        )
        {
            var gemName = methodParams[0];
            var gemVersion = methodParams[1];
            var versionInfo = Repository.VersionInfo(gemName, gemVersion);
            var expectedDate =
                DateBuilder.BuildDateTimeOffsetFromParts(expectedDateParts);

            Assert.Equal(expectedVersion, versionInfo.Version);
            Assert.Equal(expectedDate, versionInfo.DatePublished);
        }

    }

    [DataDiscoverer("Corgibytes.Freshli.Lib.Test.Integration.InstanceMemberDataDiscoverer", "Corgibytes.Freshli.Lib.Test")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class InstanceMemberDataAttribute: MemberDataAttributeBase
    {
        public InstanceMemberDataAttribute(string memberName): base(memberName, null)
        {
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            System.Console.WriteLine($"GetDate({testMethod})");

            _ = testMethod ?? throw new ArgumentNullException(nameof(testMethod));

            System.Console.WriteLine($"testMethod.DeclaringType: {testMethod.DeclaringType}");
            System.Console.WriteLine($"this.MemberType: {this.MemberType}");
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
            System.Console.WriteLine($"GetInstance({type})");

            Type target = type;

            if (type.IsAbstract && type.GenericTypeArguments.Length > 0)
            {
                target = type.GenericTypeArguments[0];
                System.Console.WriteLine($"Switching activation target to: {target})");
            }

            return Activator.CreateInstance(target);
        }

        Func<object> GetFieldAccessor(Type type)
        {
            System.Console.WriteLine($"GetFieldAccessor({type})");
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
            System.Console.WriteLine($"GetMethodAccessor({type})");
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
            System.Console.WriteLine($"GetPropertyAccessor({type})");
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
            System.Console.WriteLine($"ParameterTypesCompatible({parameters}, {parameterTypes})");
            if (parameters?.Length != parameterTypes.Length)
                return false;

            for (int idx = 0; idx < parameters.Length; ++idx)
                if (parameterTypes[idx] != null && !parameters[idx].ParameterType.GetTypeInfo().IsAssignableFrom(parameterTypes[idx].GetTypeInfo()))
                    return false;

            return true;
        }

        protected override object[] ConvertDataItem(MethodInfo testMethod, object item)
        {
            System.Console.WriteLine($"ConvertDataItem({testMethod}, {item})");
            if (item == null)
                return null;

            var array = item as object[];
            if (array == null)
                throw new ArgumentException($"Property {MemberName} on {MemberType ?? testMethod.DeclaringType} yielded an item that is not an object[]");

            return array;
        }
    }

    public class InstanceMemberDataDiscoverer: DataDiscoverer
    {
    }
}
