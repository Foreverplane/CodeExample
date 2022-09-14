using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services {
    public class CoolingController : DataController<HeatGroup>, IActionReceiver<UpdateAction> {
        private readonly ContextDataService _contextData;
        private readonly HeatService.HeatSignals _signals;
        private readonly float _сoolingTime = 0.1f;
        private float? _currentHeat;
        private float _tempTime;
        public CoolingController(IEntityDataGroup data, ContextDataService contextData, HeatService.HeatSignals signals) : base(data) {
            _contextData = contextData;
            _signals = signals;
            if (_data.OwnerData.ownerId != PhotonNetwork.LocalPlayer.UserId)
                return;
            var e = new Entity();
            e.Add(new HeatSourceData(_data.IdData.Id));
            e.Add(new HeatReceiverData(_data.IdData.Id));
            e.Add(new HeatCountData(10){lastHeatTime = StaticNetworkTime.NetworkTimeCached().Ticks });

            var code = e.GetHashCode();
            e.Add(new IdData(code));
            e.Id = code;

            PhotonNetwork.CurrentRoom.SetCustomProperties(e.ConvertToRoomProperties());

        }
        private HeatDeltaGroup HeatDeltaGroup => _contextData.GetDataGroups<HeatDeltaGroup>().First(_ => _.heatSourceData.sourceId == _data.IdData.Id);

        public void Action() {
            if (_data.DeathData.IsDead) {
                return;
            }

            _currentHeat = GetCurrentHeat(out var heatDealer); // cache request
            if (!_currentHeat.HasValue)
                return;

            if (_currentHeat >= _data.MaxHeatData.maxHeat && !_data.DeathData.IsDead) {
                Debug.Log($"Death signal fired for {_data.IdData.Id} as true dead source is {heatDealer?.Id}");
                _signals.FireDeathSignal(new DeathSignal(heatDealer, _data.IdData, true));
                HeatDeltaGroup.heatCountData.heatCount -= _currentHeat.Value;
                UpdateHeat();
                return;
            }

            Process();
            ProcessCooling();
        }


        private HeatDeltaGroup _LastHeatDeltaGroup;
        private float? GetCurrentHeat(out IdData lastHeatDealer) {
            lastHeatDealer = default;
            var deltas = _contextData.GetDataGroups<HeatDeltaGroup>();
            if (deltas.Length == 0) {
                Debug.LogError($"There is no healthDataGroup <b>{_data.IdData.Id}</b>");
                return null;
            }

            float currentHeat = 0;
            _LastHeatDeltaGroup = default;
            for (var index = 0; index < deltas.Length; index++) {
                var currentHeatDelta = deltas[index];
                //  Debug.Log($"HeatSourcePrint: Source{currentHeatDelta.heatSourceData.sourceId} Receiver {currentHeatDelta.heatReceiverData.receiverId} order {currentHeatDelta.heatCountData.lastHeatTime}");
                if (currentHeatDelta.heatReceiverData.receiverId == _data.IdData.Id) {
                    currentHeat += currentHeatDelta.heatCountData.heatCount;

                    //if (_.heatReceiverData.receiverId != _.heatSourceData.sourceId) {
                        if (_LastHeatDeltaGroup == null) {
                            _LastHeatDeltaGroup = currentHeatDelta;
                        }
                        if (_LastHeatDeltaGroup.heatCountData.lastHeatTime < currentHeatDelta.heatCountData.lastHeatTime) {
                            _LastHeatDeltaGroup = currentHeatDelta;
                        }
                   // }
                }
            }


            lastHeatDealer = new IdData(_LastHeatDeltaGroup?.heatSourceData.sourceId ?? _data.IdData.Id);



            return currentHeat;
        }
        private void Process() {
            _data.CurrentHeatData.NormalizedValue = _currentHeat.Value / _data.MaxHeatData.maxHeat;
            _data.CurrentHeatData.CurrentValue = _currentHeat.Value;
            _data.ViewMonoData.GetView<HeatView>().Value = _data.CurrentHeatData.NormalizedValue;
        }
        private void ProcessCooling() {
            if (_data.OwnerData.ownerId != PhotonNetwork.LocalPlayer.UserId)
                return;

            _tempTime += Time.deltaTime;
            if (_tempTime < _сoolingTime) {
                return;
            }
            _tempTime = 0;
            if (_currentHeat <= 0)
                return;

            HeatDeltaGroup.heatCountData.heatCount -= 0.8f;
            UpdateHeat();
        }

        private void UpdateHeat() {
            var e = new Entity {
                Id = HeatDeltaGroup.idata.Id
            };
            e.Add(HeatDeltaGroup.heatCountData);

            var roomProperties = e.ConvertToRoomProperties();

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
        }
    }
}