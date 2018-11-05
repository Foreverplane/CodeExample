    public abstract class Context
    {
        // TODO: приватизировать все поля.
        // сущность это просто лист с компонентами.
        public readonly List<Entity> Entities;
        // вся логика в системах.
        public readonly List<Systems.System> Systems;
        // вьюшка (наследних монобеха) просто реф на компоненты юнитевских префабов без какой либо логики.
        public readonly List<View> Views;
        public readonly List<Object> Resources;
        public readonly GameController GameController;
        public Action OnContextUpdated;

        private GameState _gameState;
        public GameState CurrentGameState
        {
            get { return _gameState; }
            set
            {
                Debug.Log($"Try switch state from <b> {_gameState?.GetType()?.Name} </b> to <b> {value?.GetType()?.Name} </b>");
                _gameState = value;
                for (var index = 0; index < Systems.Count; index++)
                {
                    var system = Systems[index];
                    var systemListener = system as IOnGameStateSwitchListener;
                    systemListener?.OnGameStateSwitch(value);
                }

                OnContextUpdated?.Invoke();
            }
        }
        public Context(GameController controller)
        {
            Entities = new List<Entity>();
            Systems = new List<Systems.System>();
            Views = new List<View>();
            Resources = new List<Object>();
            GameController = controller;
        }

        public abstract void Initialize();

        public GameObject GetResource(string name)
        {
            Object first = null;
            for (var index = 0; index < Resources.Count; index++)
            {
                var unknown = Resources[index];
                if (unknown.name == name)
                {
                    first = unknown;
                    break;
                }
            }

            return first as GameObject;
        }

        public void FireEntityEvent<T>(T firedEvent)
        {
            for (var index = 0; index < Systems.Count; index++)
            {
                var system = Systems[index];
                var s = system as IOnEventFiredListenerSystem<T>;
                s?.OnEventFired(firedEvent);
            }
        }

        [Obsolete("Лучше юзать ивенты а не вот это вот всё.")]
        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
            Systems.ForEach(Notify(entity));
            OnContextUpdated?.Invoke();
        }

        private static Action<Systems.System> Notify(Entity entity)
        {
            return system =>
            {
                var systemListener = system as IOnEntityAddListenerSystem;
                systemListener?.OnEntityAdded(entity);
            };
        }

        public T GetView<T>() where T : class
        {
            for (int i = 0; i < Views.Count; i++)
            {
                var v = Views[i];
                if (v != null)
                {
                    var vv = v as T;
                    if (vv != null)
                        return vv;
                }
            }

            return null;
        }

        public List<T> GetViews<T>() where T : class
        {
            return Views.OfType<T>().ToList();
        }

        public T FindComponent<T>() where T : class, IComponent
        {
            return Entities.Find(_ => _.GetComponent<T>() != null)?.GetComponent<T>();
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
            for (var index = 0; index < Systems.Count; index++)
            {
                var system = Systems[index];
                var systemListener = system as IOnEntityRemoveListenerSystem;
                systemListener?.OnEntityRemoved(entity);
            }

            for (var index = 0; index < Views.Count; index++)
            {
                var system = Views[index];
                var systemListener = system as IOnEntityRemoveListenerSystem;
                systemListener?.OnEntityRemoved(entity);
            }

            OnContextUpdated?.Invoke();
        }

        public void AddSystem(Systems.System system)
        {
            Systems.Add(system);
            OnContextUpdated?.Invoke();
        }

        public void RemoveSystem(Systems.System system)
        {
            Systems.Remove(system);
            OnContextUpdated?.Invoke();
        }

        public void AddView(View view)
        {
            Views.Add(view);
            for (var index = 0; index < Systems.Count; index++)
            {
                var system = Systems[index];
                var systemListener = system as IOnViewAddedListenerSystem;
                systemListener?.OnViewAdded(view);
            }

            OnContextUpdated?.Invoke();
        }
        public void RemoveView(View view)
        {
            Views.Remove(view);
            OnContextUpdated?.Invoke();
        }

        public void CreateEntity(params IComponent[] components)
        {
            var entity = new Entity();
            entity.components.AddRange(components.ToList());
            AddEntity(entity);
        }
    }

