using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Microsoft.CSharp;


public interface IBus {
    void Fire<TSignal>(TSignal signal) where TSignal : ISignal;
    void Subscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener;
    void UnSubscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener;
}

public class ListBus : IBus {

    private List<ISignalListener> _Data = new List<ISignalListener>();

    public void Fire<TSignal>(TSignal signal) where TSignal : ISignal {
        var listeners = _Data.FindAll(_ => _ is ISignalListener<TSignal>);
        var isReceived = false;
        foreach (var listener in listeners) {
            var properListener = listener as ISignalListener<TSignal>;
            properListener.OnSignal(signal);
            isReceived = true;
        }
        if (!isReceived)
            Debug.LogWarning($"Signal of type <b>{signal.GetType().Name}</b> is not received anyone!");
    }

    public void Subscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener {
        _Data.Add(signalListener);

    }

    public void UnSubscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener {
        _Data.Remove(signalListener);
    }
}

public static class StaticSignalBus {

    private static Dictionary<Type, ListBus> _Data = new Dictionary<Type, ListBus>();

    public static void Fire<TSignal>(TSignal signal) where TSignal : ISignal {
       // Debug.Log($"<color=blue>Fire signal <b>{signal.GetType().Name}</b></color>");
        var t = signal.GetType();

        if (_Data.ContainsKey(t))
            _Data[t].Fire(signal);
        else {
            Debug.LogWarning($"There is no signal listeners for <b>{t.Name}</b>");
        }
    }

    public static void Subscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener {
        var genTypes = GetInterfaceGenericTypes(signalListener);


        try {
            foreach (var type in genTypes) {
                ListBus bus;
                if (_Data.ContainsKey(type)) {
                    bus = _Data[type];
                }
                else {
                    bus = new ListBus();
                    _Data.Add(type, bus);

                }
                bus.Subscribe(signalListener);
              //  Debug.LogWarning($"<color=green><b>Register</b> <b>{signalListener.GetType().Name}</b> successfully for signal of type <b>{type.Name}</b>!</color>");
            }
        }
        catch (Exception e) {
            Debug.LogWarning($"Cant get type for register for {signalListener.GetType().Name}");
            return;
        }



    }

    private static List<Type> GetInterfaceGenericTypes<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener {
        var genTypes = new List<Type>();
        foreach (Type intType in signalListener.GetType().GetInterfaces()) {
            if (intType.IsGenericType && intType.GetGenericTypeDefinition()
                == typeof(ISignalListener<>)) {
                genTypes.Add(intType.GetGenericArguments()[0]);
            }
        }

        return genTypes;
    }

    public static void UnSubscribe<TSignalListener>(TSignalListener signalListener) where TSignalListener : ISignalListener {
        var genTypes = GetInterfaceGenericTypes(signalListener);


        try {
            foreach (var type in genTypes) {
                ListBus bus;
                if (_Data.ContainsKey(type)) {
                    bus = _Data[type];
                }
                else {
                    return;
                }
                bus.UnSubscribe(signalListener);
                Debug.LogWarning($"<color=green><b>Unregister</b> <b>{signalListener.GetType().Name}</b> successfully for signal of type <b>{type.Name}</b>!</color>");
            }
        }
        catch (Exception e) {
            Debug.LogWarning($"Cant get type for register for {signalListener.GetType().Name}");
            return;
        }
    }
}

public interface ISignalListener { }

public interface ISignalListener<TSignal> : ISignalListener where TSignal : ISignal {
    void OnSignal(TSignal signal);
}


