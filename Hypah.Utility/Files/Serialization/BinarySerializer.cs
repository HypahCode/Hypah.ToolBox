using Hypah.Utility.Reflection;

namespace Hypah.Utility.Files.Serialization
{
    public sealed class BinarySerializer
    {
        public byte[] Serialize(object obj)
        {
            var buffer = new BinaryBuffer();
            SerializeType(buffer, obj);
            return buffer.ToByteArray();
        }

        public T Deserialize<T>(byte[] bytes) where T : new () => (T)Deserialize(bytes, typeof(T));

        public object Deserialize(byte[] data, Type type)
        {
            var buffer = new BinaryBuffer(data);
            return DeserializeType(buffer, type);
        }

        private void SerializeType(BinaryBuffer buffer, object obj)
        {
            var type = obj.GetType();
            if (obj is string str)
            {
                buffer.Write(str);
            }
            else if (type.IsArray)
            {
                SerializeArray(buffer, (Array)obj);
            }
            else if (IsEnumerableType(type))
            {
                SerializeEnumerable(buffer, obj);
            }
            else if (IsDictionary(type))
            {
                SerializeDictionary(buffer, obj);
            }
            else if (type.IsClass)
            {
                SerializeClass(buffer, obj);
            }
            else
            {
                buffer.Write(obj);
            }
        }

        private void SerializeClass(BinaryBuffer bytes, object obj)
        {
            var type = new ReflectionType(obj.GetType());
            foreach (var member in type.GetMembers(MemberAccess.OnlyPublic))
            {
                var value = member.GetValue(obj)!;
                SerializeType(bytes, value);
            }
        }

        private void SerializeArray(BinaryBuffer buffer, Array array)
        {
            var len = array.Length;
            buffer.Write(len);

            var elementType = array.GetType().GetElementType()!;
            Array.CreateInstance(elementType, len);

            for (int i = 0; i < len; i++)
            {
                var value = array.GetValue(i);
                SerializeType(buffer, value!);
            }
        }
        
        private void SerializeEnumerable(BinaryBuffer buffer, object obj)
        {
            var elementType = obj.GetType().GetGenericArguments()[0];
            var countMethodInfo = typeof(Enumerable).GetMethods().Single(method => method.Name == "Count" && method.IsStatic && method.GetParameters().Length == 1);
            var localCountMethodInfo = countMethodInfo.MakeGenericMethod(elementType);

            var enumerableType = typeof(IEnumerable<>).MakeGenericType(elementType);
            var getEnumeratorMethodInfo = enumerableType.GetMethods().Single(method => method.Name == "GetEnumerator" && method.GetParameters().Length == 0);

            var iterator = getEnumeratorMethodInfo.Invoke(obj, [ ])!;
            var moveNextMethodInfo = iterator.GetType().GetMethods().Single(method => method.Name == "MoveNext" && method.GetParameters().Length == 0);
            var currentPropertyInfo = iterator.GetType().GetProperties().Single(method => method.Name == "Current");

            var count = (int)localCountMethodInfo.Invoke(null, [obj])!;
            buffer.Write(count);
            
            while ((bool)moveNextMethodInfo.Invoke(iterator, [])!)
            {
                var element = currentPropertyInfo.GetValue(iterator);
                SerializeType(buffer, element);
            }
        }

        private void SerializeDictionary(BinaryBuffer buffer, object obj)
        {
            var keyType = obj.GetType().GetGenericArguments()[0];
            var valueType = obj.GetType().GetGenericArguments()[1];
            var countMethodInfo = obj.GetType().GetProperty("Count");

            var getEnumeratorMethodInfo = obj.GetType().GetMethods().Single(method => method.Name == "GetEnumerator" && method.GetParameters().Length == 0);

            var iterator = getEnumeratorMethodInfo.Invoke(obj, [])!;
            var moveNextMethodInfo = iterator.GetType().GetMethod("MoveNext");
            var currentPropertyInfo = iterator.GetType().GetProperty("Current");

            var count = (int)countMethodInfo!.GetValue(obj)!;
            buffer.Write(count);

            while ((bool)moveNextMethodInfo.Invoke(iterator, [])!)
            {
                var element = currentPropertyInfo!.GetValue(iterator)!;
                var key = element.GetType().GetProperty("Key")!.GetValue(element)!;
                var value = element.GetType().GetProperty("Value")!.GetValue(element)!;
                SerializeType(buffer, key);
                SerializeType(buffer, value);
            }

            buffer.Write(0);
        }

        private object DeserializeType(BinaryBuffer buffer, Type type)
        {
            if (type == typeof(string))
            {
                return buffer.ReadString();
            }
            if (type.IsArray)
            {
                return DeserializeArray(buffer, type);
            }
            if (IsEnumerableType(type))
            { 
                return DeserializeEnumerable(buffer, type);
            }
            if (IsDictionary(type))
            {
                return DeserializeDictionary(buffer, type);
            }
            if (type.IsClass)
            {
                return DeserializeClass(buffer, type);
            }
            return buffer.ReadStruct(type);
        }

        private object DeserializeClass(BinaryBuffer buffer, Type classType)
        {
            var instance = Activator.CreateInstance(classType);
            if (instance == null) throw new Exception($"Could not create instance of {classType}. Make sure it has a parameterless constructor");
            var type = new ReflectionType(classType);
            foreach (var member in type.GetMembers(MemberAccess.OnlyPublic))
            {
                var value = DeserializeType(buffer, member.ReflectedType);
                member.SetValue(instance, value);
            }
            return instance;
        }

        private Array DeserializeArray(BinaryBuffer buffer, Type type)
        {
            var elementCount = buffer.ReadInt32();
            var elementType = type.GetElementType()!;
            var array = Array.CreateInstance(elementType, elementCount);
            for (int i = 0; i < elementCount; i++)
            {
                var value = DeserializeType(buffer, elementType);
                array.SetValue(value, i);
            }
            return array;
        }

        private Array DeserializeEnumerable(BinaryBuffer buffer, Type type)
        {
            var elementType = type.GetGenericArguments()[0];
            var count = buffer.ReadInt32();

            var array = Array.CreateInstance(elementType, count);
            for (int i = 0; i < count; i++)
            {
                var value = DeserializeType(buffer, elementType);
                array.SetValue(value, i);
            }

            return array;
        }

        private object DeserializeDictionary(BinaryBuffer buffer, Type type)
        {
            var dictionary = Activator.CreateInstance(type);

            var keyType = type.GenericTypeArguments[0];
            var valueType = type.GenericTypeArguments[1];
            var count = buffer.ReadInt32();

            var addMethodInfo = type.GetMethod("Add", [ keyType, valueType ]);
            for (int i = 0; i < count; i++)
            {
                var key = DeserializeType(buffer, keyType);
                var value = DeserializeType(buffer, valueType);

                addMethodInfo.Invoke(dictionary, [ key, value ]);
            }

            return dictionary;
        }

        private bool IsEnumerableType(Type type)
        {
            var a = type.GetGenericArguments();
            if (a.Length == 1)
            {
                var enumerableType = typeof(IEnumerable<>).MakeGenericType(a);
                var isEnumerableType = enumerableType.IsAssignableFrom(type);
                return isEnumerableType;
            }
            return false;
        }

        private bool IsDictionary(Type type)
        {
            var a = type.GetGenericArguments();
            if (a.Length == 2)
            {
                var enumerableType = typeof(IDictionary<,>).MakeGenericType(a);
                var isDictionaryType = enumerableType.IsAssignableFrom(type);
                return isDictionaryType;
            }
            return false;
        }
    }
}
