using System;
using Assets.Scripts.Core.Services;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class CurrentDriverData : IEntityData {
    [IgnoreMember]
    public Driver Driver;
}