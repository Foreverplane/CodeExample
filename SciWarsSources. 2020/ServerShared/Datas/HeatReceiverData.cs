using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class HeatReceiverData : IEntityData
{
    [Key(0)]
    public int receiverId;

    public HeatReceiverData(int receiverId)
    {
        this.receiverId = receiverId;
    }

    public HeatReceiverData()
    {
    }
}