namespace Hypah.Utility.Files
{
    public sealed class FilePath : IComparable<FilePath>, IComparable<string>
    {
        private readonly string _path = "";

        public FilePath(string p)
        {
            _path = p;
        }

        public FilePath(FilePath p)
        {
            _path = p.ToString();
        }

        public static implicit operator string(FilePath p) => p.ToString();
        public static implicit operator FilePath(string path) => new FilePath(path);

        public static FilePath operator +(FilePath filepath, string path)
        {
            return new FilePath(filepath.ToString() + path);
        }

        public static FilePath operator /(FilePath filepath, string path)
        {
            return Path.Combine(filepath, path);
        }

        public static FilePath operator /(FilePath p1, FilePath p2)
        {
            return new FilePath(Path.Combine(p1.ToString(), p2.ToString()));
        }

        public static bool operator ==(FilePath a, FilePath b)
        {
            if (a is null && b is null)
                return true;
            if (a is null || b is null)
                return false;
            return a.ToString() == b.ToString();
        }

        public static bool operator !=(FilePath a, FilePath b)
        {
            return !(a == b);
        }

        public FilePath Up()
        {
            var directory = Path.GetDirectoryName(_path);
            return new FilePath(directory ?? string.Empty);
        }

        public bool Exists => Path.Exists(_path);

        public override string ToString() => _path;

        public int CompareTo(FilePath? other)
        {
            if (other is null)
                return 1;
            return string.Compare(_path, other.ToString(), StringComparison.Ordinal);
        }

        public int CompareTo(string? other)
        {
            if (other == null)
                return 1;
            return string.Compare(_path, other, StringComparison.Ordinal);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (obj is FilePath other)
            {
                return string.Equals(_path, other._path, StringComparison.Ordinal);
            }

            return false;
        }

        public override int GetHashCode() => _path.GetHashCode();

        public static bool operator <(FilePath left, FilePath right)
        {
            return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
        }

        public static bool operator <=(FilePath left, FilePath right)
        {
            return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
        }

        public static bool operator >(FilePath left, FilePath right)
        {
            return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
        }

        public static bool operator >=(FilePath left, FilePath right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
        }
    }
}
