using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class ResourceData : IEntityData {
    [Key(0)]
    public string resourceName;

    public ResourceData(string resourceName) {
        this.resourceName = resourceName;
    }

    public ResourceData() {
    }
}