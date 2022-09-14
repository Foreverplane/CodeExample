using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class KeyData : IEntityData
{
    [Key(0)]
    public int entityId;
    [Key(1)]
    public byte componentId;

    public KeyData(int entityId, byte componentId)
    {
        this.entityId = entityId;
        this.componentId = componentId;
    }

    public KeyData()
    {
    }
}