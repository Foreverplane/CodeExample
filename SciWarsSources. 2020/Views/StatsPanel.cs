

using System;
using Zenject;
public class StatsPanel : Panel {

    [Inject]
    private SignalBus _SignalBus;

    
    
    private void Start() {
        _SignalBus.Subscribe<StatsPanelChange>((s) => {
            gameObject.SetActive(s.IsActive);
        });
    }

}

public struct StatsPanelChange {
    public readonly bool IsActive;
    public StatsPanelChange(bool isActive) {
        IsActive = isActive;
    }
}