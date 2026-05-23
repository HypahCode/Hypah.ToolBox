using System.Reflection;

namespace Hypah.Utility.Reflection
{
    public sealed class ReflectionTypeList : IEnumerable<ReflectionType>
    {
        public List<ReflectionType> Types { get; } = new List<ReflectionType>();

        public ReflectionTypeList() { }

        public ReflectionTypeList(IEnumerable<ReflectionType> types)
        {
            Types.AddRange(types);
        }

        /// <summary>
        /// Entry point to get all types from all assemblies
        /// </summary>
        /// <returns>List of types</returns>
        public static ReflectionTypeList GetAllTypes()
        {
            var list = new ReflectionTypeList();
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                list.AddRange(GetTypes(asm));
            }
            return list;
        }

        /// <summary>
        /// Entry point to get all types from a specified assembly
        /// </summary>
        /// <returns>List of types</returns>
        public static ReflectionTypeList GetTypes(Assembly asm)
        {
            return new ReflectionTypeList(asm.GetTypes().Select(x => new ReflectionType(x)));
        }

        public void Add(ReflectionType type) => Types.Add(type);

        private void AddRange(IEnumerable<ReflectionType> types) => Types.AddRange(types);

        public ReflectionTypeList Implements<T>() => Implements(typeof(T));

        public ReflectionTypeList Implements(Type interfaceType)
        {
            return new ReflectionTypeList(Types.Where(x => interfaceType.IsAssignableFrom(x.Type)));
        }

        public ReflectionTypeList WithAttributte<T>() where T : Attribute
        {
            return new ReflectionTypeList(Types.Where(x => x.Type.GetCustomAttributes<T>().Any()));
        }

        public ReflectionTypeList WithConstructor(params Type[] constructorArguments)
        {
            return new ReflectionTypeList(Types.Where(x => x.GetConstructor(constructorArguments) != null));
        }

        public IEnumerator<ReflectionType> GetEnumerator() => Types.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
