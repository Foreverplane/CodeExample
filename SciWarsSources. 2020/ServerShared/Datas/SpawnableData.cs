using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class SpawnableData : IEntityData
{
    [Key(0)]
    public bool isSpawned;

    public SpawnableData(bool isSpawned)
    {
        this.isSpawned = isSpawned;
    }

    public SpawnableData()
    {
    }
}