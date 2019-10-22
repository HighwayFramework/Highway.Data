using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Highway.Data.OData
{
    /// <summary>
    ///     Defines the RuntimeTypeProvider.
    /// </summary>
    public class RuntimeTypeProvider : IRuntimeTypeProvider
    {
        private const MethodAttributes GetSetAttr = MethodAttributes.Final | MethodAttributes.Public;
        private static readonly AssemblyName AssemblyName = new AssemblyName {Name = "HighwayDataODataTypes"};
        private static readonly ModuleBuilder ModuleBuilder;
        private static readonly ConcurrentDictionary<string, Type> BuiltTypes = new ConcurrentDictionary<string, Type>();

        private static readonly ConcurrentDictionary<Type, CustomAttributeBuilder[]> TypeAttributeBuilders =
            new ConcurrentDictionary<Type, CustomAttributeBuilder[]>();

        private static readonly ConcurrentDictionary<MemberInfo, CustomAttributeBuilder[]> PropertyAttributeBuilders =
            new ConcurrentDictionary<MemberInfo, CustomAttributeBuilder[]>();

        private readonly IMemberNameResolver _nameResolver;

        static RuntimeTypeProvider()
        {
            ModuleBuilder = Thread
                .GetDomain()
                .DefineDynamicAssembly(AssemblyName, AssemblyBuilderAccess.Run)
                .DefineDynamicModule(AssemblyName.Name);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="RuntimeTypeProvider" /> class.
        /// </summary>
        /// <param name="nameResolver"></param>
        public RuntimeTypeProvider(IMemberNameResolver nameResolver)
        {
            _nameResolver = nameResolver;
        }

        /// <summary>
        ///     Gets the <see cref="Type" /> matching the provided members.
        /// </summary>
        /// <param name="sourceType">The <see cref="Type" /> to generate the runtime type from.</param>
        /// <param name="properties">The <see cref="MemberInfo" /> to use to generate properties.</param>
        /// <returns>A <see cref="Type" /> mathing the provided properties.</returns>
        public Type Get(Type sourceType, IEnumerable<MemberInfo> properties)
        {
            properties = properties.ToArray();
            if (!properties.Any())
            {
                throw new ArgumentOutOfRangeException("properties",
                    "properties must have at least 1 property definition");
            }

            var dictionary = properties.ToDictionary(f => _nameResolver.ResolveName(f), memberInfo => memberInfo);

            var className = GetTypeKey(sourceType, dictionary);
            return BuiltTypes.GetOrAdd(
                className,
                s =>
                {
                    var typeBuilder = ModuleBuilder.DefineType(
                        className,
                        TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                    Contract.Assume(typeBuilder != null);

                    SetAttributes(typeBuilder, sourceType);

                    foreach (var field in dictionary)
                    {
                        CreateProperty(typeBuilder, field);
                    }

                    return typeBuilder.CreateType();
                });
        }

        private static void CreateProperty(TypeBuilder typeBuilder, KeyValuePair<string, MemberInfo> field)
        {
            var propertyType = field.Value.MemberType == MemberTypes.Property
                ? ((PropertyInfo) field.Value).PropertyType
                : ((FieldInfo) field.Value).FieldType;
            var fieldBuilder = typeBuilder.DefineField("_" + field.Key, propertyType, FieldAttributes.Private);

            var propertyBuilder = typeBuilder.DefineProperty(field.Key, PropertyAttributes.None, propertyType, null);

            SetAttributes(propertyBuilder, field.Value);

            var getAccessor = typeBuilder.DefineMethod(
                "get_" + field.Key,
                GetSetAttr,
                propertyType,
                Type.EmptyTypes);

            var getIl = getAccessor.GetILGenerator();
            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            var setAccessor = typeBuilder.DefineMethod(
                "set_" + field.Key,
                GetSetAttr,
                null,
                new[] {propertyType});

            var setIl = setAccessor.GetILGenerator();
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getAccessor);
            propertyBuilder.SetSetMethod(setAccessor);
        }

        private static void SetAttributes(TypeBuilder typeBuilder, Type type)
        {
            var attributeBuilders = TypeAttributeBuilders
                .GetOrAdd(
                    type,
                    t =>
                    {
                        var customAttributes = t.GetCustomAttributesData();
                        return CreateCustomAttributeBuilders(customAttributes).ToArray();
                    });

            Contract.Assume(attributeBuilders != null);

            foreach (var attributeBuilder in attributeBuilders)
            {
                typeBuilder.SetCustomAttribute(attributeBuilder);
            }
        }

        private static void SetAttributes(PropertyBuilder propertyBuilder, MemberInfo memberInfo)
        {
            var customAttributeBuilders = PropertyAttributeBuilders
                .GetOrAdd(
                    memberInfo,
                    p =>
                    {
                        var customAttributes = p.GetCustomAttributesData();
                        return CreateCustomAttributeBuilders(customAttributes).ToArray();
                    });

            Contract.Assume(customAttributeBuilders != null);

            foreach (var attribute in customAttributeBuilders)
            {
                propertyBuilder.SetCustomAttribute(attribute);
            }
        }

        private static IEnumerable<CustomAttributeBuilder> CreateCustomAttributeBuilders(
            IEnumerable<CustomAttributeData> customAttributes)
        {
            return customAttributes.Select(x =>
            {
                var namedArguments = x.NamedArguments;
                var properties = namedArguments.Select(a => a.MemberInfo).OfType<PropertyInfo>().ToArray();
                var values = namedArguments.Select(a => a.TypedValue.Value).ToArray();
                var constructorArgs = x.ConstructorArguments.Select(a => a.Value).ToArray();
                var constructor = x.Constructor;
                return new CustomAttributeBuilder(constructor, constructorArgs, properties, values);
            });
        }

        private static string GetTypeKey(Type sourceType, Dictionary<string, MemberInfo> fields)
        {
            return fields.Aggregate("HighwayDataOData<>" + sourceType.Name,
                (current, field) => current + (field.Key + field.Value.MemberType));
        }
    }
}