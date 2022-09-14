

public class LockClickSignal : ISignal {

    public readonly byte selectableId;

    public LockClickSignal(byte selectableId) {
        this.selectableId = selectableId;
    }
}

public class LockWidgetView : ViewsHolder {

    public byte selectableId;

    void OnMouseDown() {
        StaticSignalBus.Fire(new LockClickSignal(selectableId));
    }
}

