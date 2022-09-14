using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class MaxHeatData : IEntityData
{
    [Key(0)]
    public float maxHeat;

    public MaxHeatData(float maxHeat)
    {
        this.maxHeat = maxHeat;
    }

    public MaxHeatData()
    {
    }
}