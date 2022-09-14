using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class OwnerData : IEntityData
{
    [Key(0)]
    public string ownerId;

    public OwnerData(string ownerId)
    {
        this.ownerId = ownerId;
    }

    public OwnerData()
    {
    }
}