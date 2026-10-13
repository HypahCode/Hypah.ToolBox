namespace Hypah.Ai.Provider.Interfaces
{

    public class Parameter
    {
        public string Name { get; }
        public string Value { get; }
        public Parameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
