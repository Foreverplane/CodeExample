using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class WeaponData : IEntityData {
    [Key(0)]
    public NameData nameData;
}
