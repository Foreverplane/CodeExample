using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class GuidData : IEntityData {
    [Key(0)]
    public Guid guid;

    public GuidData(Guid guid) {
        this.guid = guid;
    }

    public GuidData() {
    }
}