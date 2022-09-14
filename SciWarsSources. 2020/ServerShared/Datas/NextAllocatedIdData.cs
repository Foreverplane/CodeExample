using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class NextAllocatedIdData : IEntityData {
    [Key(1)]
    public int id;

    public NextAllocatedIdData(int id) {
        this.id = id;
    }

    public NextAllocatedIdData() {
    }
}