using System.Collections.Generic;

namespace Assets.Scripts.Core.Services
{
    public class OnEntitiesCreatedSignal : ISignal {
        public readonly List<Entity> createdEntities;

        public OnEntitiesCreatedSignal(List<Entity> createdEntities) {
            this.createdEntities = createdEntities;
        }
    }
}