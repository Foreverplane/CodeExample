using System;
using MessagePack;

[Serializable]
public abstract class Stat<TValue>
{
    [Key(0)]
    public TValue value;
    [Key(1)]
    public bool isVisible;
}
