
namespace Hypah.Utility.Reflection
{
    public interface IReflectionMember
    {
        string Name { get; }
        Type ReflectedType { get; }

        T? GetAttribute<T>() where T : Attribute;
        List<T> GetAttributes<T>() where T : Attribute;

        bool CanWrite();
        bool CanRead();

        void SetValue(object target, object value);
        object? GetValue(object target);
        T? GetValue<T>(object target);
    }
}