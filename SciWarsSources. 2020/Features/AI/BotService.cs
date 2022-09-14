using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using RandomNameGen;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services {

    public class BotDataGroup : IEntityDataGroup
    {
        public DriverData DriverData;
        public IdData IdData;
        public OwnerData OwnerData;
        public ViewMonoData ViewData;
        public PhotonViewData PhotonViewData;
    }

    public class CreateBotRequestSignal : ISignal {

    }
    public class BotService : ITickable, ISignalListener<CreateBotRequestSignal> {

        [Zenject.Inject]
        private SettingsService _settingsService;
        [Zenject.Inject]
        private GameContextDataService _contextDataService;
        [Zenject.Inject]
        private SpawnerView[] _spawnerViews;
        [Zenject.Inject]
        private Zenject.SignalBus _signalBus;

        private int _i = 0;

        private List<SpawnerView> _tempSpawnerViews = new List<SpawnerView>();
        private bool _botCreateRequested;

        void ISignalListener<CreateBotRequestSignal>.OnSignal(CreateBotRequestSignal signal) {
            if (!PhotonNetwork.InRoom)
                return;
            Debug.Log($"Create bot request received");
            var shipEntity = new Entity();

            shipEntity.Add(new OwnerData(PhotonNetwork.LocalPlayer.UserId));
            shipEntity.Add(new SpawnableData());
            shipEntity.Add(new DriverData(DriverData.DriverType.AI, 100));
            shipEntity.Add(new CurrentDriverData());
            shipEntity.Add(new CurrentTargetData());
            shipEntity.Add(new MovementInputData(new float[] { 0, 0 }));
            shipEntity.Add(new ShootData(false));
            shipEntity.Add(new CurrentHeatData(0));
            shipEntity.Add(new PhotonViewData());
            shipEntity.Add(new DeathData(false) { LastAliveTime = StaticNetworkTime.NetworkTimeCached().Ticks });
            shipEntity.Add(new KilledByData(new Dictionary<IdData, int>()));
            shipEntity.Add(new PointsData());
            shipEntity.Add(new KillsData(new Dictionary<IdData, int>()));
            var profileData = new ProfileData() {
                guid = Guid.NewGuid().ToString(),
                nickName = RndName.RandomNickName
            };
            shipEntity.Add(profileData);

            var s = _settingsService.GetSettings<ShipItemDataObject>();

            if (_i == s.Length) {
                _i = 0;
            }

            var settings = s[_i];
            _i++;

            shipEntity.Add(new MaxHeatData(settings.statsData.heatCapacity.value));
            shipEntity.Add(new HealthData(settings.statsData.heatCapacity.value));
            shipEntity.datas.Add(new ResourceData(settings.name));
            shipEntity.datas.AddRange(settings.GetType().GetFields().Select(_ => _.GetValue(settings) as IEntityData).ToArray());

            // apply spawn data
            var spawner = _spawnerViews.Where(_ => !_tempSpawnerViews.Contains(_)).OrderBy(_ => Guid.NewGuid()).First();
            _tempSpawnerViews.Add(spawner);
            var rot = Quaternion.LookRotation(Vector3.up);
            var wantedRot = rot.eulerAngles;
            wantedRot.y += 180;
            rot = Quaternion.Euler(wantedRot);
            shipEntity.Add(new SpawnData(spawner.transform.position.ToArray(), rot.ToArray()));

            var idDataGroup = _contextDataService.GetDataGroups<IdDataGroup>().First(_ => _.actorData.actor == PhotonNetwork.LocalPlayer.ActorNumber);

            shipEntity.Id = idDataGroup.nextAllocatedIdData.id++;
            shipEntity.Add(new IdData(shipEntity.Id));

            var deltaEntity = new Entity(idDataGroup.nextAllocatedIdData) {
                Id = idDataGroup.idData.Id
            };
            var deltaProperties = deltaEntity.ConvertToRoomProperties();
            var roomProperties = shipEntity.ConvertToRoomProperties();

            deltaProperties.Merge(roomProperties);
            PhotonNetwork.CurrentRoom.SetCustomProperties(deltaProperties);
        }

        void ITickable.Tick() {
            if (Input.GetKeyDown(KeyCode.B)) {
                _signalBus.Fire(new CreateBotRequestSignal());
            }

            var bots = _contextDataService.GetDataGroups<BotDataGroup>();
            CheckThatBotOwnerIsConnected(bots);
            if(bots.Any(_=>_.DriverData.driverType==DriverData.DriverType.AI))
                return;
            if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.CurrentRoom !=null&& PhotonNetwork.CurrentRoom.PlayerCount == 1 && !_botCreateRequested) {
                if(_contextDataService.GetDataGroups<IdDataGroup>().FirstOrDefault(_ => _.actorData.actor == PhotonNetwork.LocalPlayer.ActorNumber)==null)
                    return;
                _botCreateRequested = true;
                _signalBus.Fire(new CreateBotRequestSignal());
            }
        }

        private void CheckThatBotOwnerIsConnected(BotDataGroup[] bots)
        {
            if(!PhotonNetwork.LocalPlayer.IsMasterClient)
                return;
            var playersList = PhotonNetwork.PlayerList;
            foreach (var bot in bots)
            {
                if(bot.DriverData.driverType!=DriverData.DriverType.AI)
                    continue;
                if (playersList.Any(_ => _.UserId == bot.OwnerData.ownerId))
                {
                    continue;
                }
                else
                {
                    bot.OwnerData.ownerId = PhotonNetwork.MasterClient.UserId;
                    bot.PhotonViewData.PhotonView.RequestOwnership();
                    var deltaEntity = new Entity(bot.OwnerData)
                    {
                        Id = bot.IdData.Id
                    };
                    var deltaProperties = deltaEntity.ConvertToRoomProperties();
                    PhotonNetwork.CurrentRoom.SetCustomProperties(deltaProperties);
                }
            }
        }
    }
}