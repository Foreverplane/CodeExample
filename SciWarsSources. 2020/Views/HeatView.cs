using UnityEngine;
using UnityEngine.UI;

public class HeatView : View
{
    [SerializeField]
    private Image _image;

    public float Value
    {
        set { _image.fillAmount = value; }
    }

    [field: SerializeField]
    public Vector3 Offset { get; } = new Vector3(-7,0,0);
}