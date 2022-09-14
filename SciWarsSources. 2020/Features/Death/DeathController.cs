using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services {


    public class DeathController : DataController<DeathDataGroup>, IActionReceiver<UpdateAction> {
        private readonly GameContextDataService _context;
        private readonly DeadService.DeadSignals _signals;
        private readonly SpawnerView[] _spawnerViews;
        private bool _isKilled = false;
        private bool _isLocal = false;
        //private Tween _tween;

        public DeathController(GameContextDataService context,IEntityDataGroup dataGroup, DeadService.DeadSignals signals, SpawnerView[] spawnerViews) : base(dataGroup) {
            _context = context;
            _signals = signals;
            _spawnerViews = spawnerViews;
            _isLocal = _data.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId &&
                       _data.driverData.driverType == DriverData.DriverType.User;
        }
        public void ApplyDeath(DeathSignal deathSignal) {
            //Debug.Log($"Apply death id:<b>{_data.idData.Id}</b> isDead {deathSignal.isDead}");

            ApplyPhoton(_data, deathSignal.isDead, deathSignal.deadSource);
        }

        private void ApplyDeathView(bool isDeath) {
            //Debug.Log($"Apply death view <b>{isDeath}</b> for {_data.idData.Id}");
            var view = _data.viewData.GetView<ShipView>();
            var rbView = view.GetComponent<Rigidbody>();
            var shipModel = view.GetComponentInChildren<ShipModelView>(true);
            var colView = shipModel.GetComponent<Collider>();
            colView.enabled = !isDeath;
            var animView = view.GetComponent<Animator>();
            animView.enabled = !isDeath;
            //var photonRbView = view.GetComponent<PhotonRigidbodyView>();
            //photonRbView.enabled = !isDeath;
            //var photonAnimView = view.GetComponent<PhotonAnimatorView>();
            //photonAnimView.enabled = !isDeath;
            var destructedView = view.GetView<DestructedView>();
            // view.GetView<ShipModelView>().gameObject.SetActive(!isDeath);
            destructedView.gameObject.SetActive(isDeath);
            //view.gameObject.SetActive(!isDeath);
            foreach (var activeView in view.Views.OfType<AliveActiveView>()) {
                activeView.gameObject.SetActive(!isDeath);
            }


            destructedView.gameObject.transform.parent = isDeath ? null : shipModel.transform;
            if (destructedView.gameObject.transform.parent != null) {
                destructedView.gameObject.transform.localPosition = Vector3.zero;
                destructedView.gameObject.transform.localRotation = Quaternion.identity;
                destructedView.gameObject.transform.localScale = Vector3.one;
            }

            var velocity = rbView.velocity;
            foreach (var chunk in destructedView.Views.OfType<DestructedChunkView>()) {
                chunk.gameObject.transform.localPosition = chunk.InitialLocalPosition;
                chunk.gameObject.transform.localRotation = chunk.InitialLocalRotation;
                chunk.gameObject.transform.localScale = chunk.InitialLocalScale;
                var rb = chunk.GetComponent<Rigidbody>();
                rb.isKinematic = !isDeath;
                if (isDeath) {
                    rb.velocity = Vector3.one;
                    rb.velocity += velocity;
                    var forceDir = destructedView.transform.position - chunk.transform.position;
                    forceDir.Scale(new Vector3(1, 1, 0));
                    rb.AddForce(forceDir.normalized * 50f, ForceMode.Impulse);
                }
                var collider = chunk.GetComponent<Collider>();
                collider.enabled = isDeath;
            }

            var randomSpawner = _spawnerViews.OrderBy(_ => Guid.NewGuid()).First();

            if (!isDeath) {
                //view.transform.position = _data.spawnData.position.ToVector3();
                //view.transform.rotation = _data.spawnData.rotation.ToQuaternion();
                view.transform.position = randomSpawner.transform.position;
                view.transform.rotation = _data.spawnData.rotation.ToQuaternion();
            }

            var vfxPos = view.transform.position.ToArray();
            var vfxNorm = Vector3.forward.ToArray();
            var vfxName = isDeath ? "BigExplosionVFX" : "EnergyExplosionVFX";

            _signals.PlayVfxSignal(new PlayVfxSignal(vfxName, vfxPos, vfxNorm));

            rbView.isKinematic = isDeath;

        }

        private void ApplyPhoton(DeathDataGroup killedDataGroup, bool isDeath, IdData killerId) {
            killedDataGroup.deathData.IsDead = isDeath;
            killedDataGroup.deathData.LastAliveTime = !isDeath ? StaticNetworkTime.NetworkTimeCached().Ticks : 0;
            var deadStatData = killedDataGroup.killedByData;
            Debug.Log($"Apply dead to photon for dataGroupId {killedDataGroup.idData.Id} and source {killerId.Id} is Dead is {isDeath}");

            Entity killerEntity = null;
            if (isDeath)
            {
                if (!killedDataGroup.idData.Equals(killerId))
                {
                    if (deadStatData.KilledBy.ContainsKey(killerId))
                    {
                        deadStatData.KilledBy[killerId]++;
                    }
                    else
                    {
                        deadStatData.KilledBy[killerId] = 1;
                    }
                    var killer = _context.GetDataGroupById<DeathDataGroup>(killerId.Id);

                    if (killer.killsData.Kills.ContainsKey(killerId)) {
                        killer.killsData.Kills[killerId]++;
                    }
                    else {
                        killer.killsData.Kills[killerId] = 1;
                    }

                    killerEntity = new Entity(killer.killsData) { Id = killer.idData.Id };
                }

               
            }

            var resurrectEntity = new Entity(new DeathData(isDeath), deadStatData) {
                Id = killedDataGroup.idData.Id
            };
            var resultProperties = resurrectEntity.ConvertToRoomProperties();
            if (killerEntity!=null)
            {
                var killerProperties = killerEntity.ConvertToRoomProperties();
                resultProperties.Merge(killerProperties);
            }
            
            PhotonNetwork.CurrentRoom.SetCustomProperties(resultProperties);
        }
        void IActionReceiver<UpdateAction>.Action() {
            if (_isLocal) {
                //SignalBus.Fire(_dataGroup.deathData.IsDead
                //    ? new SetUIStateSignal(typeof(UiState.DeathUiState))
                //    : new SetUIStateSignal(typeof(UiState.AliveUiState)));
            }

            if (_isKilled == _data.deathData.IsDead) {
                //Debug.Log($"<b>{_data.idData}</b> is already killed!");
                return;
            }

            ApplyDeathView(_data.deathData.IsDead);
            _isKilled = _data.deathData.IsDead;
        }
    }
}