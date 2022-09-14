using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services {
    public class DeathInputController : DataController<DeathDataGroup>, IActionReceiver<UpdateAction> {
        private readonly DeadService.DeadSignals _signals;
        private bool _requested;

        private float _resCd = 2f;
        private float _tempCd;
        public DeathInputController(IEntityDataGroup data, DeadService.DeadSignals signals) : base(data) {
            _signals = signals;
        }

        void IActionReceiver<UpdateAction>.Action() {
            if (_data.currentDriverData.Driver == null)
                return;
            if (!_data.deathData.IsDead) {
                return;
            }

            _tempCd -= Time.deltaTime;
            if (_tempCd > 0) {
                return;
            }

            if (_requested) {
                _requested = false;
            }


            if (!_requested && _data.currentDriverData.Driver.IsResurrectRequested) {
                Debug.Log($"Death signal fired for {_data.idData.Id} as false by Input");
                _signals.FireDeathSignal(new DeathSignal(_data.idData, _data.idData, false));
                _tempCd = _resCd;
                _requested = true;
            }
        }
    }
}