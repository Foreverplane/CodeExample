using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class DeathData : IEntityData
{
    [Key(0)]
    public bool IsDead;
    [Key(1)]
    public long LastAliveTime;

    public DeathData(bool isDead)
    {
        IsDead = isDead;
    }

    public DeathData()
    {
    }

    public override int GetHashCode()
    {
        return IsDead.GetHashCode();
    }
}