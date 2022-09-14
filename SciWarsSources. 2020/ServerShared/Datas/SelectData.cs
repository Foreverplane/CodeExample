using System;
using MessagePack;

[Serializable]
[MessagePackObject()]
public class SelectData : IEntityData {
    [Key(0)]
    public bool IsSelected;

    public SelectData(bool isSelected) {
        IsSelected = isSelected;
    }

    public SelectData() {
    }
}

