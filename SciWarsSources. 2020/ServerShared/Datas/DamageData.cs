using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class DamageData : IEntityData
{
    [Key(0)]
    public float value;

    public DamageData(float value)
    {
        this.value = value;
    }

    public DamageData()
    {
    }
}