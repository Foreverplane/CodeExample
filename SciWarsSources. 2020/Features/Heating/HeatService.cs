using System;
using System.Collections.Generic;
using Providers;
using Zenject;
using SignalBus = Zenject.SignalBus;

namespace Assets.Scripts.Core.Services
{
    public class HeatService : DataGroupService, ISignalListener<OnSpawnedSignal>,ITickable {
        [Zenject.Inject]
        private readonly ResourceService _resourceService;

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {
            base.CreateControllers<HeatGroup>(signal.idData);
        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            yield return new HeatController(datagroup, _resourceService);
            yield return new CoolingController(datagroup, ContextDataService,new HeatSignals(signalBus));
        }

        public class HeatSignals : SignalsFilter {
            public Action<DeathSignal> FireDeathSignal => signalBus.Fire;
            public HeatSignals(SignalBus signalBus) : base(signalBus) {
            }
        }

        public void Tick()
        {
            base.objectProviderService.Action<UpdateAction>();
        }
    }
}