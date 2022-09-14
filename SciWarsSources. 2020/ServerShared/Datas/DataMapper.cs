using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataMapper
{

    private static Dictionary<Type, byte> _bytes = new Dictionary<Type, byte>();

    static DataMapper()
    {
        var type = typeof(IEntityData);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

        var startValue = byte.MinValue;
        foreach (var t in types)
        {
            _bytes[t] = startValue++;
        }
    }

    public static byte GetByteCode(this IEntityData data)
    {
        if (_bytes.TryGetValue(data.GetType(), out var b))
        {
            return b;
        }
        else
        {
            throw new NullReferenceException($"Cant get byte code for <b>{data.GetType().Name}</b>");
        }
        
    }

    public static Type[] GetEntityDataTypes()
    {
        return _bytes.Keys.ToArray();
    }

    public static Dictionary<Type, byte> GetDictionary()
    {
        return _bytes;
    }
}