using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class KillsData : IEntityData
{
    [Key(0)]
    public Dictionary<IdData, int> Kills;

    public KillsData(Dictionary<IdData, int> kills) {
        this.Kills = kills;
    }

    public override int GetHashCode()
    {
        return Kills.Values.Sum();
    }
}