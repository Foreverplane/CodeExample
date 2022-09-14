using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class HeatCountData : IEntityData
{
    [Key(0)]
    public float heatCount;
    [Key(1)]
    public long lastHeatTime;

    public HeatCountData(float heatCount)
    {
        this.heatCount = heatCount;
    }

    public HeatCountData()
    {
    }
}