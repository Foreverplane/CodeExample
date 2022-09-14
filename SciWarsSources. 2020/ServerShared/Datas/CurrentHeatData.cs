using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class CurrentHeatData : IEntityData
{
    [Key(0)]
    public float NormalizedValue;
    [Key(1)]
    public float CurrentValue;

    public CurrentHeatData(float normalizedValue)
    {
        this.NormalizedValue = normalizedValue;
    }

    public CurrentHeatData()
    {
    }
}