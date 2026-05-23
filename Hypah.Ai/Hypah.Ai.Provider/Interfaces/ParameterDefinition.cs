namespace Hypah.Ai.Provider.Interfaces
{
    public enum ParameterType
    {
        String,
        Int,
        Float,
    }

    public class ParameterDefinition
    {
        public string Name { get; }
        public string DefaultValue { get; }
        public ParameterType ParameterType { get; }
        public string Description { get; }

        public ParameterDefinition(string name, string defaultValue, ParameterType parameterType, string description)
        {
            Name = name;
            DefaultValue = defaultValue;
            ParameterType = parameterType;
            Description = description;
        }

        public Parameter CreateDefaultParameter()
        {
            return new Parameter(Name, DefaultValue);
        }

        public Parameter CreateParameter(object value)
        {
            switch (ParameterType)
            {
                case ParameterType.String:
                    return new Parameter(Name, value.ToString());
                case ParameterType.Int:
                    if (int.TryParse(value.ToString(), out int intValue))
                    {
                        return new Parameter(Name, intValue.ToString());
                    }
                    throw new ArgumentException($"Value for parameter '{Name}' must be an integer.");
                case ParameterType.Float:
                    if (float.TryParse(value.ToString(), out float floatValue))
                    {
                        return new Parameter(Name, floatValue.ToString());
                    }
                    throw new ArgumentException($"Value for parameter '{Name}' must be a float.");
                default:
                    throw new InvalidOperationException($"Unsupported parameter type: {ParameterType}");
            }
        }

        public override string ToString()
        {
            return $"{Name}={DefaultValue} ({ParameterType}) - {Description}";
        }
    }
}
