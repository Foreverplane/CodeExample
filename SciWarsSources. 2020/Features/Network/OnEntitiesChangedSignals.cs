using System.Collections.Generic;

namespace Assets.Scripts.Core.Services
{
    public class OnEntitiesChangedSignals : ISignal {
        public readonly List<Entity> changedEntities;

        public OnEntitiesChangedSignals(List<Entity> changedEntities) {
            this.changedEntities = changedEntities;
        }
    }
}