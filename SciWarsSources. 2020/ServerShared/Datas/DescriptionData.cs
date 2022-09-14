using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class DescriptionData : IEntityData
{
    [Key(0)]
    public string Description;

    public DescriptionData(string description)
    {
        Description = description;
    }
}
