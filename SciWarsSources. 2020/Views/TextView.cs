using TMPro;
using UnityEngine;

public abstract class TextView : View
{
    public string Value {
        set { _text.text = value.ToString(); }
    }
    [SerializeField]
    protected TMP_Text _text;

    void OnValidate() {
        _text = GetComponent<TMP_Text>();
    }
}