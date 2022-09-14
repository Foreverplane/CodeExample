using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class SpawnData : IEntityData {
    [Key(0)]
    public float[] position;
    [Key(1)]
    public float[] rotation;

    public SpawnData(float[] position, float[] rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public SpawnData()
    {
    }
}
