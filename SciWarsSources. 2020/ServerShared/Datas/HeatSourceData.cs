using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class HeatSourceData : IEntityData
{
    [Key(0)]
    public int sourceId;

    public HeatSourceData(int sourceId)
    {
        this.sourceId = sourceId;
    }

    public HeatSourceData()
    {
    }
}