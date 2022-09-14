using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class VelocityData : IEntityData
{
    [Key(0)]
    public float value;

    public VelocityData(float value)
    {
        this.value = value;
    }

    public VelocityData()
    {
    }
}