using System;
using System.Collections.Generic;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class KilledByData : IEntityData
{
    [Key(0)]
    public Dictionary<IdData, int> KilledBy;

    public KilledByData(Dictionary<IdData, int> killedBy)
    {
        this.KilledBy = killedBy;
    }
}