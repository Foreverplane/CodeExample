using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupView : View
{
    [SerializeField]
    private CanvasGroup _canvasGroup;
    private bool _targetActive;
    private bool IsActive => Math.Abs(_canvasGroup.alpha - 1) < Mathf.Epsilon;

    void OnValidate()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    void Awake()
    {
        _targetActive = IsActive;
    }

    public void SetActive(bool isActive)
    {
        if(_targetActive == isActive)
            return;
        _canvasGroup.DOFade(isActive ? 1 : 0, 0.2f);
        _targetActive = isActive;
    }
}