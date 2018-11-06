
    public class Entity
    {

        public readonly string id;
        public readonly List<IComponent> components;

        public Entity()
        {
            id = Guid.NewGuid().ToString();
            components = new List<IComponent>();
        }

        public Entity AddComponent(params IComponent[] component)
        {
            components.AddRange(component);
            return this;
        }

        public Entity AddComponent(IComponent component)
        {
            components.Add(component);
            return this;
        }

        public T GetComponent<T>() where T : IComponent
        {
            for (var index = 0; index < components.Count; index++)
            {
                var comp = components[index];
                if (comp is T)
                    return (T) comp;
            }

            return default(T);
        }

    }
