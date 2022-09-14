using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assets.Scripts.Core.Services
{
    public abstract class ContextDataService {


        private readonly List<Entity> _entities = new List<Entity>();

        public void Remove(Entity e)
        {
            _entities.Remove(e);
        }

        public void Add(Entity e) {
            // Debug.Log($"Add entity <b>{e.Id}</b> to context");
            e.OnEntityChanged += (en) => {
                // Debug.LogError($"Entity {en.Id} is changed");
                SetAllCacheDirty();
            };
            SetAllCacheDirty();
            _entities.Add(e);
        }


        private void SetAllCacheDirty() {
            // Debug.LogWarning($"Cache is dirty!");
            foreach (var cachedDataGroups in _dictionary.Values) {
                cachedDataGroups.IsRequested = false;
            }
        }

        private readonly Dictionary<Type, CachedDataGroups> _dictionary = new Dictionary<Type, CachedDataGroups>();


        public TData[] GetDataGroups<TData>() where TData : class, IEntityDataGroup, new() {
            TData[] d = null;
            Parallel.Invoke(() => {
                _dictionary.TryGetValue(typeof(TData), out var cachedDataGroups);
                if (cachedDataGroups == null) {
                    cachedDataGroups = new CachedDataGroups();
                    _dictionary[typeof(TData)] = cachedDataGroups;
                    // Debug.Log($"Cache for {typeof(TData).Name} is created");
                }

                if (!cachedDataGroups.IsRequested) {
                    cachedDataGroups.IsRequested = true;
                    var list = _entities.Select(_ => _.GetDataGroup<TData>()).Where(x => x != null).ToArray();
                    cachedDataGroups.DataGroups = list;
                    // Debug.Log($"Cached {list.Count} dataGroups");
                }

                d = cachedDataGroups.DataGroups as TData[];
            });

            return d;
        }


        public TData GetData<TData>() where TData : class, IEntityData {
            foreach (var e in _entities) {
                var data = e.GetData<TData>();
                if (data != null) {
                    return data;
                }
            }

            return null;
        }


        internal void Clear() {
            _entities.Clear();
        }

        public List<Entity> GetEntities() {
            SetAllCacheDirty();
            return _entities;
        }
        public TData GetDataGroupById<TData>(int id) where TData : class, IEntityDataGroup, new() =>
            _entities.Where(_ => _.Id == id).Select(_ => _.GetDataGroup<TData>()).FirstOrDefault(x => x != null);

        internal void AddDatasById(int id, params IEntityData[] entityData) {

            _entities.Find(_ => _.Id == id).Add(entityData);
        }
    }
}
