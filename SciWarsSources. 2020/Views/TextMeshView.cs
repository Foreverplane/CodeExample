using TMPro;
using UnityEngine;

public abstract class TextMeshView : View {
    public string Value {
        set { _text.text = value; }
    }
    [SerializeField]
    private TextMeshProUGUI _text;

    void OnValidate() {
        _text = GetComponent<TextMeshProUGUI>();
    }
}