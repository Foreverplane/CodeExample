using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class WeaponController : DataController<WeaponDataGroup>, IActionReceiver<UpdateAction> {
        private readonly WeaponService.Signals _signals;

        public WeaponController(IEntityDataGroup dataGroup, WeaponService.Signals signals) : base(dataGroup) {
            _signals = signals;
        }

        void IActionReceiver<UpdateAction>.Action() {

            if (_data.deathData.IsDead)
                return;
            if (_data.currentDriverData.Driver == null)
                return;

            var isFire = _data.shootData.IsShoot;
            if (!isFire)
                return;

            var rocketRoot = _data.viewMonoData.GetView<ShipView>().GetComponentInChildren<RocketRootView>();
            var rocketPos = rocketRoot.transform.position;



            //var dot = Vector3.Dot(rocketRoot.transform.forward, _data.currentTargetData.Course.normalized);

            //if (_data.currentTargetData.IsReachable)
            //    _data.currentDriverData.Driver.ProvideReward(dot * 2f);
            //else
            //{
            //    _data.currentDriverData.Driver.ProvideReward(-2f);
            //}
            //Debug.Log($"Fire");
            var signal = new FireRocketSignal(_data.idData.Id, rocketPos.ToArray(), rocketRoot.transform.rotation.ToArray());
            _signals.FireRocket(signal);
        }


    }
}