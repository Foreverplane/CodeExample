using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class RangeData : IEntityData {
    [Key(0)]
    public int range;

    public RangeData(int range) {
        this.range = range;
    }

    public RangeData() {
    }
}