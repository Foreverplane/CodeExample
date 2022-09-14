using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Core.Services;
using UnityEngine;

public class ArrowSignalSource : SignalSource
{
    [SerializeField]
    private Vector3 _Direction;

    void OnMouseDown()
    {
        signalBus.Fire(new SelectSignal(_Direction));
    }

}
