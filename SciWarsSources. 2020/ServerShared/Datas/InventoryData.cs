using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class InventoryData : IEntityData {
    [Key(0)]
    public WeaponData weaponData;
    [Key(1)]
    public AbilityData abilityData;
}
