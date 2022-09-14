using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class ActorData : IEntityData {
    [Key(0)]
    public int actor;

    public ActorData(int actor) {
        this.actor = actor;
    }

    public ActorData() {
    }
}
[Serializable]
[MessagePackObject()]
public class PointsData : IEntityData
{
    [Key(0)]
    public int amount;

    public PointsData(int amount)
    {
        this.amount = amount;
    }

    public PointsData()
    {
    }


    public override int GetHashCode()
    {
        return amount;
    }
}