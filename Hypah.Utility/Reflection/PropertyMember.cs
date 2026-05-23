using System.Reflection;

namespace Hypah.Utility.Reflection
{
    internal class PropertyMember : IReflectionMember
    {
        public string Name => Prop.Name;
        public PropertyMember(PropertyInfo prop)
        {
            Prop = prop;
        }

        public Type ReflectedType => Prop.PropertyType;

        public PropertyInfo Prop { get; }

        public bool CanRead() => Prop.CanRead;

        public bool CanWrite() => Prop.CanWrite;

        public T? GetAttribute<T>() where T : Attribute => Prop.GetCustomAttribute<T>();

        public List<T> GetAttributes<T>() where T : Attribute
        {
            return new List<T>(Prop.GetCustomAttributes<T>());
        }

        public object? GetValue(object target) => Prop.GetValue(target);

        public T? GetValue<T>(object target)
        {
            var value = GetValue(target);
            if (value is T tValue)
            {
                return tValue;
            }
            throw new InvalidCastException("Invalid type conversion");
        }

        public void SetValue(object target, object value)
        {
            Prop.SetValue(target, value);
        }
    }
}