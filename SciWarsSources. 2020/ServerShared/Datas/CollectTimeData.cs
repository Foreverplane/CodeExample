using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class CollectTimeData : IEntityData {

    [Key(0)]
    public long collectTime;
}
[Serializable]
[MessagePackObject()]
public class ActiveTimeData : IEntityData
{
    [Key(0)]
    public float activeTime;
}
[Serializable]
[MessagePackObject()]
public class AttachmentData : IEntityData
{
    [Key(0)]
    public IdData attachedTo;
}