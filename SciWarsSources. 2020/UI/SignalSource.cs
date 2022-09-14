using UnityEngine;
using UnityEngine.EventSystems;


public abstract class SignalSource : MonoBehaviour
{
    [Zenject.Inject]
    protected Zenject.SignalBus signalBus;

    [Zenject.Inject]
    public void Construct(Zenject.SignalBus s)
    {
        Debug.Log($"{this.GetType().Name} is constructed!");
        signalBus = s;
    }
}

public abstract class SignalSource<TSignal> : SignalSource, IPointerClickHandler
where TSignal : class,ISignal, new()
{
    [SerializeField]
    private TSignal signal;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
        signalBus.Fire(signal??new TSignal());
    }
}
