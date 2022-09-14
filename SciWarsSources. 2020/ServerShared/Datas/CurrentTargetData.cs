using System;
using Assets.Scripts.Core.Services;
using MessagePack;
using UnityEngine;

[Serializable]
[MessagePackObject()]
public class CurrentTargetData : IEntityData {
    [IgnoreMember]
    public FindTargetDataGroup Target;
    [IgnoreMember]
    public Vector3 Course;
    [IgnoreMember]
    public bool IsReachable;
    [IgnoreMember]
    public Vector3 CollisionPoint;
}