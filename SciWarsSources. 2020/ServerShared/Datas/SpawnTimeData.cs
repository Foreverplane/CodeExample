using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class SpawnTimeData : IEntityData
{
    [Key(0)]
    public long spawnTime;

}