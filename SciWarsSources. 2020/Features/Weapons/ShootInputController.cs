using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    internal class ShootInputController : DataController<WeaponDataGroup>, IActionReceiver<UpdateAction> {
        private float _tick;
        private float _minDelay = 0.2f;

        public ShootInputController(IEntityDataGroup dataGroup) : base(dataGroup) {
            _data.viewMonoData.GetView<ShipView>().gameObject.AddComponent<ShootDataWrapper>();
        }



        void IActionReceiver<UpdateAction>.Action() {
            if(_data.ownerData.ownerId!=PhotonNetwork.LocalPlayer.UserId)
                return;
            if (_data.deathData.IsDead)
                return;
            if (_data.currentDriverData.Driver == null)
                return;
            _data.shootData.IsShoot = false/* _data.currentDriverData.Driver.IsShoot*/;
            _tick += Time.deltaTime;
            if (_tick >= _minDelay) {
                _data.shootData.IsShoot = _data.currentDriverData.Driver.IsShoot;
                if (_data.shootData.IsShoot) {
                    _tick = 0;
                }
            }
        }
    }
}