using System.Runtime.InteropServices;
using System.Text;

namespace Hypah.Utility.Files.Serialization
{
    internal sealed class BinaryBuffer
    {
        private List<byte> Data { get; } = new List<byte>();
        public int Position { get; set; }
        public int Length => Data.Count;

        public BinaryBuffer(byte[] buffer)
        {
            Data = new List<byte>(buffer);
            Position = 0;
        }

        public BinaryBuffer()
        {
            Position = 0;
        }

        public void Write(byte[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (Position >= Data.Count)
                {
                    Data.Add(data[i]);
                }
                else
                {
                    Data[i] = data[i];
                }
                Position++;
            }
        }


        public void Write(string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            Write(bytes.Length);
            Write(bytes);
        }
        public void Write(object structure) => Write(ToByteArray(structure));
        public void Write(int num) => Write(BitConverter.GetBytes(num));


        public byte[] ReadBytes(int count)
        {
            var bytes = new byte[count];
            for (int i = 0; i < count; i++)
            {
                bytes[i] = Data[Position++];
            }
            return bytes;
        }


        public string ReadString()
        {
            var size = ReadInt32();
            var bytes = ReadBytes(size);
            return Encoding.ASCII.GetString(bytes);
        }

        public int ReadInt32() => BitConverter.ToInt32(ReadBytes(4));
        public object ReadStruct(Type type)
        {
            var size = Marshal.SizeOf(type);
            var bytes = ReadBytes(size)!;
            return FromByteArray(bytes, type)!;
        }

        public byte[] ToByteArray() => Data.ToArray();

        private static byte[] ToByteArray(object structure)
        {
            var type = structure.GetType();
            var size = Marshal.SizeOf(type);
            var array = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structure, ptr, true);
            Marshal.Copy(ptr, array, 0, size);
            Marshal.FreeHGlobal(ptr);
            return array;
        }

        private static object? FromByteArray(byte[] array, Type type)
        {
            var size = Marshal.SizeOf(type);
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.Copy(array, 0, ptr, size);
            var structure = Marshal.PtrToStructure(ptr, type);
            Marshal.FreeHGlobal(ptr);
            return structure;
        }
    }
}
