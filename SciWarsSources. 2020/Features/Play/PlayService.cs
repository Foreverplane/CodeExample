using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class PlayService :
        ISignalListener<ExitSignal>,
        ISignalListener<StartGameSignal>,
        ISignalListener<SpawnClickSignal>,
        ISignalListener<RoomPropertiesUpdatedSignal>, ITickable {

        [Inject]
        private GameContextDataService _contextDataService;
        [Inject]
        private ResourceService _resourceService;


        [Inject]
        private SpawnerView[] _spawnerViews;
        [Inject]
        private SignalBus _signalBus;

        private ClientEntity _clientEntity;
        [SerializeField]
        private Entity shipEntity;

        private Guid _clientId = Guid.NewGuid();
        [SerializeField]
        private List<string> _properties = new List<string>();

        [SerializeField]
        private string _localPlayerUserId;

        [SerializeField]
        private int _localActorNr;

        [SerializeField]
        [Inject]
        private GlobalContextDataService _gloablContext;


        void ISignalListener<ExitSignal>.OnSignal(ExitSignal signal) {
            _contextDataService.Clear();
            _signalBus.Fire(new SwitchSceneSignal("LoadingScene", () => {
                _signalBus.Fire(new SwitchSceneSignal("MainMenu", () => { }));

            }));
        }

        void ISignalListener<StartGameSignal>.OnSignal(StartGameSignal signal) {
            //Debug.unityLogger.logEnabled = false;
            // Debug.LogWarning($"<color=blue><b>Game started!</b></color>");
            shipEntity = signal.e;
            _signalBus.Fire(new ConnectClientSignal(null));
            //  Debug.LogError($"<b>Please start create bots here!</b>");
        }



        void ISignalListener<SpawnClickSignal>.OnSignal(SpawnClickSignal signal) {
            if (!PhotonNetwork.InRoom)
            {
                Debug.LogError($"Client is not in room!");
                return;
            }

            Debug.Log($"Spawn ship with id: <b>{PhotonNetwork.LocalPlayer.UserId}</b>");

         
            var localShip = _contextDataService.GetDataGroups<RessurectDataGroup>().FirstOrDefault(_ => _.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId && _.driverData.driverType == DriverData.DriverType.User);


            if (localShip != null && localShip.deathData.IsDead) {
                  Debug.Log("Ressurect this shit!");
                _signalBus.Fire(new DeathSignal(localShip.idData, localShip.idData, false));
                Debug.Log($"Death signal fired for {localShip.idData.Id} as false");
                return;
            }
             Debug.Log("<color=red>Create ship entity!</color>");
            var healthData = new HealthData(100);
            shipEntity.Add(healthData);
            shipEntity.Add(new OwnerData(PhotonNetwork.LocalPlayer.UserId));
            shipEntity.Add(new SpawnableData());
            shipEntity.Add(new DriverData(DriverData.DriverType.User, 100));
            shipEntity.Add(new CurrentDriverData());
            shipEntity.Add(new CurrentTargetData());
            shipEntity.Add(new MovementInputData(new float[] { 0, 0 }));
            shipEntity.Add(new ShootData(false));
            shipEntity.Add(new CurrentHeatData(0));
            shipEntity.Add(new MaxHeatData(100));
            shipEntity.Add(new PhotonViewData());
            shipEntity.Add(new DeathData(false){ LastAliveTime = StaticNetworkTime.NetworkTimeCached().Ticks});
            shipEntity.Add(new PointsData());
            shipEntity.Add(new KilledByData(new Dictionary<IdData, int>()));
            shipEntity.Add(new KillsData(new Dictionary<IdData, int>()));
            var profileData = _gloablContext.GetData<ProfileData>();
            if (profileData == null)
                throw new NullReferenceException("FUCK! Profile data is null!");
            shipEntity.Add(profileData);

            var spawner = _spawnerViews.OrderBy(_ => Guid.NewGuid()).First();

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
        private Entity heatEntity;


        void ITickable.Tick() {



            if (Input.GetKeyDown(KeyCode.R)) {
                var properties = PhotonNetwork.CurrentRoom.CustomProperties;
                _properties = properties.Select(_ => _.Key.ToString()).ToList();
            }

            _localPlayerUserId = PhotonNetwork.LocalPlayer.UserId;
            _localActorNr = PhotonNetwork.LocalPlayer.ActorNumber;
        }
        void ISignalListener<RoomPropertiesUpdatedSignal>.OnSignal(RoomPropertiesUpdatedSignal signal) {
            // Debug.Log($"Receive <b>{signal.roomProperties.Count}</b> roomProperties");

            var entities = _contextDataService.GetEntities();
              //Debug.Log($"Entities count <b>{entities.Count}</b>");
            var createdEntities = new List<Entity>();
            var changedEntities = new HashSet<Entity>();

            foreach (var property in signal.roomProperties) {
                var jsonByte = MessagePack.MessagePackSerializer.ConvertFromJson(property.Key as string);
                var keyData = MessagePack.MessagePackSerializer.Deserialize<KeyData>(jsonByte);
                var entity = entities.Find(_ => _.Id == keyData.entityId);
                if (entity == null) {
                  //  Debug.Log($"There is no entity for id <b>{keyData.entityId}</b>. Lets create it!");
                    entity = new Entity();
                    entities.Add(entity);
                    entity.Id = keyData.entityId;
                    createdEntities.Add(entity);
                }

                IEntityData data = null;
                try
                {
                    data = entity.datas.Find(_ => _.GetByteCode() == keyData.componentId);
                }
                catch (Exception e)
                {
                    throw new NullReferenceException("Some of data is null!");
                }
               
                if (data == null)
                {
                    var p = property.Value as IEntityData;
                  //  Debug.Log($"Add {property.Value.GetType().Name} as IEntityData to entity with ID: <b>{entity.Id}</b>");
                    entity.Add(p);

                    changedEntities.Add(entity);

                }
                else {
                    var dataFields = data.GetType().GetFields();
                    foreach (var field in dataFields) {
                        var propValue = property.Value;
                        if (propValue == null) {
                            entity.datas.RemoveAll(_ => _.GetType() == property.GetType());
                            continue;
                        }
                        var value = propValue.GetType().GetField(field.Name).GetValue(property.Value);
                        field.SetValue(data, value);
                    }
                }
            }

            Entity[] emptyEntities = null;

            emptyEntities = createdEntities.Where(_ => _.datas.Count == 0).ToArray();
            foreach (var e in emptyEntities) {
                //  Debug.LogError($"<b>Created Entity with id {e.Id} is now empty!</b>");
            }

            emptyEntities = changedEntities.Where(_ => _.datas.Count == 0).ToArray();
            foreach (var e in emptyEntities) {
                // Debug.LogError($"<b>Changed Entity with id {e.Id} is now empty!</b>");
            }


            SpawnIfNotSpawned();
            if (createdEntities.Count > 0)
                _signalBus.Fire(new OnEntitiesCreatedSignal(createdEntities));
            if (changedEntities.Count > 0)
                _signalBus.Fire(new OnEntitiesChangedSignals(changedEntities.ToList()));
        }

        private void SpawnIfNotSpawned() {
            var spawnables = _contextDataService.GetDataGroups<SpawnableDataGroup>();
            // Debug.Log($"Get {spawnables.Count()} spawnables");
            foreach (var spawnable in spawnables) {
                if (_contextDataService.GetDataGroupById<ViewDataGroup>(spawnable.idData.Id) != null)
                    continue;

                var isLocal = spawnable.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId;
                // Debug.Log($"Spawn ship with id: <b>{spawnable.ownerData.ownerId}</b> and it is local <b>{isLocal}</b>");
                var resource = _resourceService.GetDatas<ResourceView>()
                    .First(_ => _.name == spawnable.resourceData.resourceName);
                var instance = UnityEngine.Object.Instantiate(resource, spawnable.spawnData.position.ToVector3(), spawnable.spawnData.rotation.ToQuaternion());

                var photonView = instance.gameObject.AddComponent<PhotonView>();
                photonView.OwnershipTransfer = OwnershipOption.Takeover;
                photonView.ViewID = spawnable.idData.Id;
                spawnable.photonViewData.PhotonView = photonView;

                if (isLocal) {
                    DOVirtual.DelayedCall(1, () => {
                        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    });

                    _contextDataService.AddDatasById(spawnable.idData.Id, new CameraFollowData());

                }

                var prefics = isLocal ? "_local" : "_remote";
                instance.gameObject.name = instance.gameObject.name + prefics;
                var syncView = instance.gameObject.AddComponent<PhotonRigidbodyView>();
                syncView.m_SynchronizeAngularVelocity = true;
                syncView.m_SynchronizeVelocity = true;

                var syncAnimator = SetupAnimatorView(instance);

                photonView.ObservedComponents = new List<Component>();
                photonView.ObservedComponents.Add(syncView);
                photonView.ObservedComponents.Add(syncAnimator);
                photonView.Synchronization = ViewSynchronization.UnreliableOnChange;

                SetupRigibody(instance);

                instance.gameObject.transform.localScale = Vector3.one * 0.5f;
                var viewData = new ViewMonoData(instance.GetComponent<ShipView>());
                instance.gameObject.AddComponent<IdDataWrapper>().Data = spawnable.idData;
                instance.gameObject.AddComponent<DeathDataWrapper>().Data = spawnable.deathData;
                instance.gameObject.AddComponent<CurrentHeatDataWrapper>().data = spawnable.currentHeatData;
                instance.gameObject.AddComponent<OwnerDataWrapper>().Data = spawnable.ownerData;
                _contextDataService.AddDatasById(spawnable.idData.Id, viewData);

                _signalBus.Fire(new OnSpawnedSignal(spawnable.idData));
            }



        }

        private static void SetupRigibody(ResourceView instance) {
            var rb = instance.gameObject.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        }

        private static PhotonAnimatorView SetupAnimatorView(ResourceView instance) {
            var syncAnimator = instance.gameObject.AddComponent<PhotonAnimatorView>();
            syncAnimator.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Disabled);
            syncAnimator.SetParameterSynchronized(StaticAnimatorParameters.EngineRearActive, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
            syncAnimator.SetParameterSynchronized(StaticAnimatorParameters.EngineFrontActive, PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete);
            syncAnimator.SetParameterSynchronized(StaticAnimatorParameters.Blend, PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            return syncAnimator;
        }


    }
}

public static class StaticAnimatorParameters
{
    public const string EngineRearActive = "EngineRearActive";
    public const string EngineFrontActive = "EngineFrontActive";
    public const string Blend = "Blend";
}