using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MessagePack;
using UnityEngine;

public static class DataGroupsFieldsCache {
    private static readonly Dictionary<Type, FieldInfo[]> _fieldInfoses = new Dictionary<Type, FieldInfo[]>();

    public static FieldInfo[] GetFieldsCached(this Type type) {
        _fieldInfoses.TryGetValue(type, out var fields);
        if (fields == null) {
            fields = type.GetFields();
            _fieldInfoses[type] = fields;
        }

        return fields;
    }



}

public class CachedGroup {
    public bool IsRequested;
    public bool IsFilled;
    public object Group;
}

[MessagePackObject()]
public class Entity {
    [Key(1)]
    public int Id;
    [Key(0)]
    public List<IEntityData> datas = new List<IEntityData>();

    public TData GetData<TData>() where TData : IEntityData {
        return datas.OfType<TData>().FirstOrDefault();
    }
    [IgnoreMember]
    private readonly Dictionary<Type, CachedGroup> _dictionaryDataGroups = new Dictionary<Type, CachedGroup>();
 
    public event Action<Entity> OnEntityChanged = (e) => {};

    public TGroup GetDataGroup<TGroup>() where TGroup : class, IEntityDataGroup, new() {

        var type = typeof(TGroup);
        _dictionaryDataGroups.TryGetValue(type, out var cachedDataGroup);
        if (cachedDataGroup == null) {

            cachedDataGroup = new CachedGroup();
            _dictionaryDataGroups[type] = cachedDataGroup;
            var groupInstance = GroupCache.CreateInstance<TGroup>();
            cachedDataGroup.Group = groupInstance;
        }

        if (!cachedDataGroup.IsRequested) {
            cachedDataGroup.IsRequested = true;
            var groupFields = cachedDataGroup.Group.GetType().GetFieldsCached();
            for (var i = 0; i < groupFields.Length; i++)
            {
                var field = groupFields[i];
                IEntityData data = null;
                for (var index = 0; index < datas.Count; index++)
                {
                    var unknown = datas[index];
                    if (unknown.GetType() == field.FieldType)
                    {
                        data = unknown;
                        break;
                    }
                }

                if (data == null)
                    return null;
                field.SetValue(cachedDataGroup.Group, data);
            }

            cachedDataGroup.IsFilled = true;

        }
        return cachedDataGroup.IsFilled ? cachedDataGroup.Group as TGroup : null;

    }


    public Entity Add(params IEntityData[] data) {
        datas.AddRange(data);
        foreach (var cachedGroup in _dictionaryDataGroups.Values) {
            cachedGroup.IsRequested = false;
        }
        OnEntityChanged.Invoke(this);
        
        return this;
    }

    public Entity(params IEntityData[] data) {
        datas.AddRange(data);
    }

    public Entity() {
    }

    public override string ToString() {
        var types = datas.Select(_ => $"{_.GetType().Name}");
        return string.Join("/", types);
    }
}

[MessagePackObject()]
public class ClientEntity : Entity {
    public ClientEntity(params IEntityData[] data) : base(data) {
    }

    public ClientEntity() {
    }
}
[MessagePackObject()]
public class DeltaEntity : Entity { }


