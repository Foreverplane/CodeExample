using UnityEngine.EventSystems;

public class ExitSignal : ISignal {

}

public class ExitSignalSource : SignalSource, IPointerDownHandler
{

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) {
        signalBus.Fire(new ExitSignal());
    }
}