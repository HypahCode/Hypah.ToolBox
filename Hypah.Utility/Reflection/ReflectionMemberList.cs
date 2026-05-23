
namespace Hypah.Utility.Reflection
{
    public sealed class ReflectionMemberList : IEnumerable<IReflectionMember>
    {
        public List<IReflectionMember> Members { get; } = new List<IReflectionMember>();

        public ReflectionMemberList() { }
        public ReflectionMemberList(IEnumerable<IReflectionMember> members)
        {
            Members.AddRange(members);
        }

        public void Add(IReflectionMember members) => Members.Add(members);

        public void AddRange(IEnumerable<IReflectionMember> members) => Members.AddRange(members);

        public ReflectionMemberList WithAttributte<T>() where T : Attribute
        {
            var members = Members.Where(x => x.GetAttributes<T>().Any());
            return new ReflectionMemberList(members);
        }

        public IEnumerator<IReflectionMember> GetEnumerator() => Members.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
