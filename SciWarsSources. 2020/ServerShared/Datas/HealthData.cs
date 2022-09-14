using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class HealthData : IEntityData {
    [Key(0)]
    public float value;

    public HealthData(float value) {
        this.value = value;
    }

    public HealthData()
    {
    }
}
