using System;
using UnityEngine;

public class ArrowGameSignalSource : SignalSource<InputSignal>
{

}
[Serializable]
public class InputSignal : ISignal
{
    public Vector2 ArrowInputData;
}

