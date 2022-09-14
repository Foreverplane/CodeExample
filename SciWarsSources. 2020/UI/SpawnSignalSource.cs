using Assets.Scripts.Core.Services;
using UnityEngine.EventSystems;

public class SpawnSignalSource : SignalSource, IPointerDownHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        signalBus.Fire(new SpawnClickSignal());
    }
}
