using UnityEngine;

public abstract class Panel : ViewsHolder
{

    [SerializeField]
    private CanvasGroup _canvasGroup;

    protected override void OnValidate()
    {
        base.OnValidate();
        _canvasGroup = GetComponent<CanvasGroup>();
    }
}