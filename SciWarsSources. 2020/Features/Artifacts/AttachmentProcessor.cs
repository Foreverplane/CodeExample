using System;
using UnityEngine;

public class AttachmentProcessor
{
    private readonly Transform _target;
    private readonly Transform _transform;
    private readonly Action _onDeattach;
    private Vector3 _vel;
    private bool _attached;

    public AttachmentProcessor(Transform target, Transform transform, Action onDeattach)
    {
        this._target = target;
        this._transform = transform;
        this._onDeattach = onDeattach;
        _attached = true;

    }

    public void Update()
    {
        if(!_attached)
            return;
        _transform.position = Vector3.SmoothDamp(_transform.position, _target.transform.position, ref _vel, 0.1f);
    }

    public void Deattach()
    {
        if (!_attached)       
            return;
        
        _attached = false;
        _onDeattach.Invoke();
    }
}