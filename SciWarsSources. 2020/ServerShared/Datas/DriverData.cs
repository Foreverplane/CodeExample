using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class DriverData : IEntityData {
    public enum DriverType {
        None,
        User,
        AI
    }
    [Key(0)]
    public DriverType driverType;
    [Key(1)]
    public byte iq;

    public DriverData(DriverType driverType, byte iq) {
        this.driverType = driverType;
        this.iq = iq;
    }

    public DriverData() {
    }
}