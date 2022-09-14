using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class DamageMultiplierData : IEntityData
{
    [Key(0)]
    public float multiplier;
}

[Serializable]
[MessagePackObject()]
public class IgnoreSelfHeatData : IEntityData
{

}
[Serializable]
[MessagePackObject()]
public class IgnoreOtherHeatData : IEntityData
{

}
[Serializable]
[MessagePackObject()]
public class RicochetData : IEntityData
{

}
[Serializable]
[MessagePackObject()]
public class RicoShieldData : IEntityData
{

}
[Serializable]
[MessagePackObject()]
public class RouletteData : IEntityData
{
    [Key(0)]
    public int count;
}
[Serializable]
[MessagePackObject()]
public class RocketAutoAimData : IEntityData
{

}
[Serializable]
[MessagePackObject()]
public class NeoData : IEntityData
{
    [Key(0)]
    public float radius;
}