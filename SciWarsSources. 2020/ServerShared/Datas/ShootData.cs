using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class ShootData : IEntityData {
    [Key(0)]
    public bool IsShoot;

    public ShootData(bool isShoot)
    {
        IsShoot = isShoot;
    }
}