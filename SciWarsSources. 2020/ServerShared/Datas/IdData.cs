using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class IdData : IEntityData {
    [Key(0)]
    public int Id;

    public IdData(int id) {
        this.Id = id;
    }

    public IdData()
    {
    }

    protected bool Equals(IdData other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((IdData) obj);
    }

    public override int GetHashCode()
    {
        return Id;
    }
}