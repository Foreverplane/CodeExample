using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using Providers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Core.Services {
    public class WeaponService :
        DataGroupService,
        ISignalListener<OnSpawnedSignal>,
        ISignalListener<FireRocketSignal>,
        ISignalListener<OnEntitiesCreatedSignal>,
        ISignalListener<OnRocketHitSignal>,
        ISignalListener<OnEntitiesChangedSignals>,
        ITickable {

        [Zenject.Inject]
        private ResourceService _resourceService;

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {

            base.CreateControllers<WeaponDataGroup>(signal.idData);
        }

        protected override IEnumerable<ObjectProvider> GetProviders() {
            return base.GetProviders();
        }



        public class Signals : SignalsFilter {
            public Action<FireRocketSignal> FireRocket => signalBus.Fire;

            public Signals(SignalBus signalBus) : base(signalBus) {
            }
        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            yield return new ShootInputController(datagroup);
            yield return new WeaponController(datagroup, new Signals(signalBus));
        }

        void ITickable.Tick() {
            base.objectProviderService.Action<UpdateAction>();
        }

        void ISignalListener<FireRocketSignal>.OnSignal(FireRocketSignal signal) {

            //Debug.Log($"Fire rocket <b>{signal.id}</b>");

            var idDataGroup = ContextDataService.GetDataGroups<IdDataGroup>().First(_ => _.actorData.actor == PhotonNetwork.LocalPlayer.ActorNumber);
            //  Debug.Log($"Found IdDataGroup {idDataGroup.idData.Id}");
            var allocatedId = idDataGroup.nextAllocatedIdData.id++;
            // Debug.Log($"Allocated id is: {allocatedId}");
            var deltaEntity = new Entity(idDataGroup.nextAllocatedIdData) {
                Id = idDataGroup.idData.Id
            };
            var deltaProperties = deltaEntity.ConvertToRoomProperties();


            var rocketEntity = new Entity();
            rocketEntity.Add(new SpawnData(signal.position, signal.rotation));
            rocketEntity.Add(new OwnerData(PhotonNetwork.LocalPlayer.UserId));
            rocketEntity.Add(new RocketData());
            rocketEntity.Add(new DamageData(50));
            rocketEntity.Add(new VelocityData(50));
            rocketEntity.Add(new AuthorData(signal.id));
            rocketEntity.Id = allocatedId;
            rocketEntity.Add(new IdData(allocatedId));


            var rocketPropertiews = rocketEntity.ConvertToRoomProperties();
            deltaProperties.Merge(rocketPropertiews);


            var heatDeltaGroup = ContextDataService.GetDataGroups<HeatDeltaGroup>()
                .First(_ => _.heatSourceData.sourceId == signal.id);

            Debug.Log($"Add heat from to <b>{heatDeltaGroup.heatSourceData.sourceId}</b> to <b>{heatDeltaGroup.heatReceiverData.receiverId}</b>");
            heatDeltaGroup.heatCountData.heatCount += 20;
            heatDeltaGroup.heatCountData.lastHeatTime = StaticNetworkTime.NetworkTimeCached().Ticks;
            var deltaGroup = new Entity {
                Id = heatDeltaGroup.idata.Id
            };
            deltaGroup.Add(heatDeltaGroup.heatCountData);
            var deltaHeat = deltaGroup.ConvertToRoomProperties();

            deltaProperties.Merge(deltaHeat);

            PhotonNetwork.CurrentRoom.SetCustomProperties(deltaProperties);
        }

        void ISignalListener<OnEntitiesCreatedSignal>.OnSignal(OnEntitiesCreatedSignal signal) {
            var rocketEntities = signal.createdEntities.Where(_ => _.GetData<RocketData>() != null).ToArray();
            if (rocketEntities.Length == 0)
                return;
            //Debug.Log($"Create <b>{rocketEntities.Length}</b> rocket entities!");
            foreach (var rocketEntity in rocketEntities) {

                var resource = _resourceService.GetData<RocketView>();
                if (resource == null) {
                    Debug.LogError("Cant get rocket resource!");
                    return;
                }

                var transformData = rocketEntity.GetData<SpawnData>();
                var rocketEntityId = rocketEntity.Id;
                var ownerData = rocketEntity.GetData<OwnerData>();
                var isLocal = ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId;

                var inst = Object.Instantiate(resource, transformData.position.ToVector3(), transformData.rotation.ToQuaternion());
                var rb = inst.gameObject.AddComponent<Rigidbody>();
                //var collider = rb.GetComponent<CapsuleCollider>();
                //collider.enabled = false;
                //DOVirtual.DelayedCall(0.1f, () => { collider.enabled = true; });
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                var photonView = inst.gameObject.AddComponent<PhotonView>();
                photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
                photonView.OwnershipTransfer = OwnershipOption.Takeover;
                photonView.ViewID = rocketEntityId;
                var photonSync = inst.gameObject.AddComponent<PhotonRigidbodyView>();
                photonView.ObservedComponents = new List<Component> { photonSync };

                photonSync.m_SynchronizeAngularVelocity = false;
                photonSync.m_SynchronizeVelocity = true;


                var collider = inst.GetComponent<Collider>();
                collider.enabled = false;
                DOVirtual.DelayedCall(0.1f, () => { collider.enabled = true; });
                inst.idData = rocketEntity.GetData<IdData>();
                ContextDataService.AddDatasById(rocketEntityId, new ViewMonoData(inst));

                if (isLocal) {
                    rb.AddForce(inst.transform.forward * rocketEntity.GetData<VelocityData>().value, ForceMode.VelocityChange);
                    var rocketView = inst.GetComponentInChildren<RocketView>();
                    rocketView.OnCollision += OnContact(rocketView, rocketEntity);
                    photonView.RequestOwnership();
                }
            }
        }

        private Action<ContactPoint> OnContact(RocketView rocketView, Entity rocketEntity) {
            return contact => {
                rocketView.ClearSubscribers();

                var otherView = contact.otherCollider.GetComponent<View>();
                var surface = otherView.GetComponent<SurfaceView>();
                var surfaceType = surface.surfaceType;
                var position = contact.point;
                var normal = contact.normal;

                var damagable = contact.otherCollider.GetComponentInParent<IdDataWrapper>();
                int? damagableId = damagable?.Data.Id;
                //Debug.Log($"Get contact with <b>{contact.otherCollider.name}</b> and damagable id <b>{damagableId.GetValueOrDefault()}</b>.");


                signalBus.Fire(new OnRocketHitSignal(rocketEntity.Id, damagableId, surfaceType, position.ToArray(), normal.ToArray()));
            };
        }

        void ISignalListener<OnRocketHitSignal>.OnSignal(OnRocketHitSignal signal) {
            var rocketDataGroup = ContextDataService.GetDataGroupById<RocketDataGroup>(signal.rocketEntityId);
            if (rocketDataGroup == null) {
                Debug.LogError($"Can't get rocket data group for {signal.rocketEntityId}.");
                return;
            }

            //Debug.Log($"Get rocket hit signal to <b>{signal.damagableId.GetValueOrDefault()}</b>.");



            var destroyData = new DestroyData();
            var entity = new Entity {
                Id = rocketDataGroup.Idata.Id
            };
            entity.Add(destroyData);

            var properties = entity.ConvertToRoomProperties();
            if (signal.damagableId.HasValue) {
                // Debug.Log($"Try get get damagable id {signal.damagableId.Value}.");
                var receiver = ContextDataService.GetDataGroupById<HeatGroup>(signal.damagableId.Value);
                if (receiver == null) {
                    Debug.LogError($"There is no damage receiver for <b>{signal.damagableId.Value}</b>");
                    return;
                }
                var author = ContextDataService.GetDataGroupById<HeatGroup>(rocketDataGroup.AuthorData.authorId);
                if (author == null) {
                    Debug.LogError($"There is no author for <b>{rocketDataGroup.OwnerData.ownerId}</b>");
                    return;
                }


                var heatDeltaGroup = ContextDataService.GetDataGroups<HeatDeltaGroup>()
                    .FirstOrDefault(_ => _.heatSourceData.sourceId == author.IdData.Id && _.heatReceiverData.receiverId == receiver.IdData.Id);

                //if (author.IdData.Id != receiver.IdData.Id)
                //    author.CurrentDriverData.Driver?.ProvideReward();
                if (heatDeltaGroup == null) {
                    Debug.LogError($"Cant get health delta group for author {author.IdData.Id} and receiver {receiver.IdData.Id}");
                    var e = new Entity();
                    e.Add(new HeatSourceData(author.IdData.Id));
                    e.Add(new HeatReceiverData(receiver.IdData.Id));
                    e.Add(new HeatCountData(rocketDataGroup.DamageData.value){lastHeatTime = StaticNetworkTime.NetworkTimeCached().Ticks });
                    var code = e.GetHashCode();
                    e.Add(new IdData(code));
                    e.Id = code;

                    var property = e.ConvertToRoomProperties();
                    properties.Merge(property);


                }
                else {
                    heatDeltaGroup.heatCountData.heatCount += rocketDataGroup.DamageData.value;
                    heatDeltaGroup.heatCountData.lastHeatTime= StaticNetworkTime.NetworkTimeCached().Ticks;
                    // Debug.Log($"Get health delta with id: {heatDeltaGroup.idata} with delta {heatDeltaGroup.heatCountData.heatCount}");

                    var deltaGroup = new Entity {
                        Id = heatDeltaGroup.idata.Id
                    };
                    deltaGroup.Add(heatDeltaGroup.heatCountData);
                    var deltaHeat = deltaGroup.ConvertToRoomProperties();
                    properties.Merge(deltaHeat);
                }

            }
            else {
                //  Debug.Log("There is no damage receiver!");
            }

            //Debug.Log("Try set new room custom properties with destroy data!");
            PhotonNetwork.CurrentRoom.SetCustomProperties(properties);
        }

        void ISignalListener<OnEntitiesChangedSignals>.OnSignal(OnEntitiesChangedSignals signal) {
            var destroyRockets = signal.changedEntities.Where(_ => _.GetData<DestroyData>() != null && _.GetData<RocketData>() != null).ToArray();
            // Debug.Log($"Found {destroyRockets.Length} rocket entities for destroy.");
            foreach (var rocketEntity in destroyRockets) {
                var view = rocketEntity.GetData<ViewMonoData>().GetView<RocketView>();
                var rb = view.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                var col = view.GetComponent<Collider>();
                col.enabled = false;
                var ps = view.GetComponentInChildren<ParticleSystem>();
                ps.transform.parent = null;
                UnityEngine.Object.Destroy(view.gameObject);
                UnityEngine.Object.Destroy(ps.gameObject, 5f);
                signalBus.Fire(new PlayVfxSignal("ExplosionVFX", view.transform.position.ToArray(), new float[] { 0, 1, 0 }));
                //Debug.Log($"Request destroy entity with id {rocketEntity.Id}");
                signalBus.Fire(new DestroyEntitySignal(rocketEntity.Id));
            }

        }


    }
}