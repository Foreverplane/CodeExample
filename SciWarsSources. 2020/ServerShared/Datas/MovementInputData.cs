using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class MovementInputData : IEntityData {
    [Key(0)]
    public float[] moveDirection;

    public MovementInputData(float[] moveDirection) {
        this.moveDirection = moveDirection;
    }
    [IgnoreMember]
    public bool IsMove => Math.Abs(moveDirection[0] + moveDirection[1]) > 0.001f;
    [IgnoreMember]
    public bool IsMoveVertical => Math.Abs(moveDirection[1]) > 0.001f;
}