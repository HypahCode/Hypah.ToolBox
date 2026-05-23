using System.Reflection;

namespace Hypah.Utility.Reflection
{
    public sealed class ReflectionType
    {
        public string FullName => Type.FullName ?? string.Empty;

        public ReflectionType(Type type)
        {
            Type = type;
        }

        public Type Type { get; }

        public T? Create<T>(params Type[] arguments) where T : class
        {
            if (Type.IsAssignableFrom(typeof(T)))
            {
                throw new InvalidCastException($"Cannot assign {Type} to {typeof(T)}");
            }
            return Activator.CreateInstance(Type, arguments) as T;
        }

        public ConstructorInfo? GetConstructor(params Type[] constructorArguments)
        {
            return Type.GetConstructor(constructorArguments);
        }

        public bool InheritsFrom(Type implementedType)
        {
            return implementedType.IsAssignableFrom(Type);
        }

        public bool InheritsFrom<T>() => InheritsFrom(typeof(T));

        public T? GetAttribute<T>() where T : Attribute
        {
            var attributes = Type.GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0)
            {
                return (T)attributes[0];
            }
            return null;
        }

        public bool GetAttribute<T>(out T? attribute) where T : Attribute
        {
            attribute = GetAttribute<T>();
            return attribute != null;
        }

        public ReflectionMemberList GetMembers(MemberAccess memberAccess = MemberAccess.All)
        {
            var list = new ReflectionMemberList();
            list.AddRange(GetProperties(memberAccess));
            list.AddRange(GetFields(memberAccess));
            return list;
        }

        public ReflectionMemberList GetProperties(MemberAccess memberAccess = MemberAccess.All)
        {
            var list = new ReflectionMemberList();
            var flags = GetAccessFlags(memberAccess);
            foreach (var prop in Type.GetProperties(flags))
            {
                list.Members.Add(new PropertyMember(prop));
            }
            return list;
        }

        public ReflectionMemberList GetFields(MemberAccess memberAccess = MemberAccess.All)
        {
            var list = new ReflectionMemberList();
            var flags = GetAccessFlags(memberAccess);
            foreach (var field in Type.GetFields(flags))
            {
                list.Members.Add(new FieldMember(field));
            }
            return list;
        }

        private BindingFlags GetAccessFlags(MemberAccess memberAccess)
        {
            switch (memberAccess)
            {
                case MemberAccess.All: return BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                case MemberAccess.OnlyPublic: return BindingFlags.Public | BindingFlags.Instance;
                case MemberAccess.OnlyNonePublic: return BindingFlags.NonPublic | BindingFlags.Instance;
                default: throw new ArgumentOutOfRangeException(nameof(memberAccess), memberAccess, null);
            }
        }

        public override string ToString() => FullName;
    }
}
