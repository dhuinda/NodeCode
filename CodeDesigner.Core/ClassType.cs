namespace CodeDesigner.Core;

public class ClassType
{
    public string Name;
    public readonly List<ClassType> GenericTypes;

    public ClassType(string name, List<ClassType> genericTypes)
    {
        Name = name;
        GenericTypes = genericTypes;
    }

    public ClassType(string name)
    {
        Name = name;
        GenericTypes = new List<ClassType>();
    }
    
    public string GetGenericName()
    {
        var genericName = Name;
        if (GenericTypes.Count == 0)
        {
            return genericName;
        }

        genericName += "<";
        for (var i = 0; i < GenericTypes.Count; i++)
        {
            genericName += $"{GenericTypes[i].GetGenericName()}";
            if (i + 1 < GenericTypes.Count)
            {
                genericName += ",";
            }
        }

        return genericName + ">";
    }
}