using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class CurrencyData : IEntityData {
    [Key(0)]
    public int value;
    [Key(1)]
    public CurrencyType type;

    public CurrencyData(int value, CurrencyType type) {
        this.value = value;
        this.type = type;
    }

    public CurrencyData()
    {
    }
}
