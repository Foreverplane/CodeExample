using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class ConnectionData : IEntityData
{
    public enum ConnectionState
    {
        None,
        IsConnected,
        IsDisconnected
    }
    [Key(0)]
    public ConnectionState connectionState;
}