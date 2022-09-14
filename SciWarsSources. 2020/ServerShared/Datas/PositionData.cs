using System;
using MessagePack;
using UnityEngine;

[Serializable]
[MessagePackObject()]
public class PositionData : IEntityData
{
    [Key(0)]
    public Vector3 position;
}