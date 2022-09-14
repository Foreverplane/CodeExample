using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class NameData : IEntityData {
    [Key(0)]
    public string Name;

    public NameData(string name) {
        Name = name;
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}
