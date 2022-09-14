using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class StatsData : IEntityData {
    [Key(0)]
    public Power power;
    [Key(1)]
    public Speed speed;
    [Key(2)]
    public Turn turn;
    [Key(3)]
    public Temp temp;
    [Key(4)]
    public Shield shield;
    [Key(5)]
    public Durability durability;
    [Key(6)]
    public AGEEffect ageEffect;
    [Key(7)]
    public HeatCapacity heatCapacity;
    [Key(8)]
    public EneryCapacity eneryCapacity;

}
