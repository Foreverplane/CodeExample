using System;
using System.Collections.Generic;
using Providers;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class TargetService : DataGroupService, ITickable, ISignalListener<OnSpawnedSignal>, ISignalListener<FindTargetSignal> {

        void ITickable.Tick() {
            base.objectProviderService.Action<UpdateAction>();
        }
        [Zenject.Inject]
        private IUnityEventProvider eventProvider;

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {
            Debug.Log($"Create target controllers for <b>{signal.idData.Id}</b>");
            base.CreateControllers<TargetDataGroup>(signal.idData);
        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            yield return new TargetInputController(datagroup, new TargetInputSignals(signalBus));
            yield return new TargetController(datagroup, ContextDataService);
            yield return new TargetRewardController(datagroup,eventProvider);
            yield return new TargetDebugController(datagroup);
        }

        protected override IEnumerable<ObjectProvider> GetProviders() {
            yield return ObjectProvider.Create<FindTargetSignal, TargetController>(_ => _.OnSignal);
        }

        void ISignalListener<FindTargetSignal>.OnSignal(FindTargetSignal signal) {
            //Debug.Log($"Receive FindTargetSignal for <b>{signal.id}</b>");
            base.objectProviderService.ProcessObject(signal.id, signal);
        }

        public class TargetInputSignals : SignalsFilter {
            public Action<FindTargetSignal> FireFindTargetSignal => signalBus.Fire;
            public TargetInputSignals(SignalBus signalBus) : base(signalBus) {
            }

        }
    }
}