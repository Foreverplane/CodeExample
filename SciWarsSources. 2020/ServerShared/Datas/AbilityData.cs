using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class AbilityData : IEntityData {
    [Key(0)]
    public NameData nameData;
}
