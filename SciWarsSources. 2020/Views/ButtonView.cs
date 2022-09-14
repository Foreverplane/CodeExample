using UnityEngine.UI;

public abstract class ButtonView<TSignal> : View
where TSignal : ISignal, new() {
    public Button Button;

    void OnValidate() {
        Button = GetComponent<Button>();
    }

    void Awake() {
        Button.onClick.AddListener(() => { StaticSignalBus.Fire(new TSignal()); });
    }
}