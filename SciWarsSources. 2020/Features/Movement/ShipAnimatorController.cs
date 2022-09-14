using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    internal class ShipAnimatorController : DataController<MovementService.MovementDataGroup>, IActionReceiver<UpdateAction>
    {
        private Animator _animator;
        private ShipModelView _modelView;
        private float _blend;
        private float _vel;

        public ShipAnimatorController(IEntityDataGroup dataGroup) : base(dataGroup)
        {
            _animator = base._data.viewMonoData.GetView<ShipView>().gameObject.GetComponent<Animator>();
            _modelView = base._data.viewMonoData.GetView<ShipView>().gameObject.GetComponentInChildren<ShipModelView>();
        }

        void IActionReceiver<UpdateAction>.Action() {
            if (base._data.ownerData.ownerId != PhotonNetwork.LocalPlayer.UserId) {
                return;
            }
            if (base._data.deathData.IsDead)
                return;
            var target = base._data.movementInputData.moveDirection[0];
            _blend = Mathf.SmoothDamp(_blend, target, ref _vel, 0.1f);
            _animator.SetFloat("Blend",_blend);
            _animator.SetBool("EngineRearActive",base._data.movementInputData.moveDirection[1]>=0.01f);
            _animator.SetBool("EngineFrontActive",base._data.movementInputData.moveDirection[1]<=-0.01f);
        }
    }
}