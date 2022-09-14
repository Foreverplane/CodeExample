
using UnityEngine;
using Zenject;
public class ShipSettingsClickSignal : ISignal {

    public readonly byte selectableId;

    public ShipSettingsClickSignal(byte selectableId) {
        this.selectableId = selectableId;
    }
}

public class ShipSettingsWidgetView : View
{
    public byte selectableId;
    
    private SignalBus _SignalBus;

    [Zenject.Inject]
    public void Construct(Zenject.SignalBus s)
    {
        Debug.Log($"{this.GetType().Name} is constructed!");
        _SignalBus = s;
    }
    void OnMouseDown() {
        
        _SignalBus.Fire(new ShipSettingsClickSignal(selectableId));
    }
}