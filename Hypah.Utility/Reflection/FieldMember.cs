using System.Reflection;

namespace Hypah.Utility.Reflection
{
    internal class FieldMember : IReflectionMember
    {
        public string Name => Field.Name;
        public FieldMember(FieldInfo field)
        {
            Field = field;
        }

        public Type ReflectedType => Field.FieldType;

        public FieldInfo Field { get; }

        public bool CanRead() => true;

        public bool CanWrite() => true;

        public T? GetAttribute<T>() where T : Attribute => Field.GetCustomAttribute<T>();

        public List<T> GetAttributes<T>() where T : Attribute
        {
            return new List<T>(Field.GetCustomAttributes<T>());
        }

        public object? GetValue(object target) => Field.GetValue(target);
           
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
            Field.SetValue(target, value);
        }
    }
}