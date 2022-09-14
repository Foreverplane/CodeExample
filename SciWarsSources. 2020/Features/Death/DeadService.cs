using System;
using System.Collections.Generic;
using Providers;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class DeadService : DataGroupService, ITickable, ISignalListener<DeathSignal>, ISignalListener<OnSpawnedSignal> {

        [Zenject.Inject]
        private SpawnerView[] _spawnerViews;
        void ISignalListener<DeathSignal>.OnSignal(DeathSignal signal) {
            Debug.Log($"Receive death signal for <b>{signal.deadReceiver}</b> with isDead <b>{signal.isDead}</b>");

            objectProviderService.ProcessObject(signal.deadReceiver, signal);
        }

        void ITickable.Tick() {
            if (Input.GetKeyDown(KeyCode.K)) {

                signalBus.Fire(new DeathSignal(new IdData(1000),new IdData(1000), true));
            }

            objectProviderService.Action<UpdateAction>();
        }

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {
            Debug.Log($"Create death controller for <b>{signal.idData.Id}</b>");

            CreateControllers<DeathDataGroup>(signal.idData);
        }

        protected override IEnumerable<ObjectProvider> GetProviders() {
            yield return ObjectProvider.Create<DeathSignal, DeathController>(_ => _.ApplyDeath);
        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            var signals = new DeadSignals(signalBus);
            yield return new DeathInputController(datagroup, signals);
            yield return new DeathController(ContextDataService,datagroup, signals, _spawnerViews);
        }

        public class DeadSignals : SignalsFilter {
            public Action<PlayVfxSignal> PlayVfxSignal => signalBus.Fire;
            public Action<DeathSignal> FireDeathSignal => signalBus.Fire;
            public DeadSignals(SignalBus signalBus) : base(signalBus) {
            }


        }
    }
}