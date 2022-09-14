using System;
using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    internal class MovementController : DataController<MovementService.MovementDataGroup>, IActionReceiver<UpdateAction>
    {
        private Rigidbody _rigidBody;



        public MovementController(IEntityDataGroup dataGroup) : base(dataGroup) {
            _rigidBody = base._data.viewMonoData.GetView<ShipView>().GetComponent<Rigidbody>();
        }
        void IActionReceiver<UpdateAction>.Action() {
            if (base._data.ownerData.ownerId != PhotonNetwork.LocalPlayer.UserId) {
                return;
            }
            if (base._data.deathData.IsDead)
                return;
            var moveDirection = base._data.movementInputData.moveDirection[1];

            var resultForce = _rigidBody.transform.forward * moveDirection * base._data.statsData.speed.value;
            var currentVelocity = _rigidBody.velocity;
            if (Mathf.Sign(resultForce.x) == Mathf.Sign(currentVelocity.x))
                resultForce.x *= 1 - Math.Abs(currentVelocity.x) / base._data.statsData.speed.value;
            if (Mathf.Sign(resultForce.y) == Mathf.Sign(currentVelocity.y))
                resultForce.y *= 1 - Math.Abs(currentVelocity.y) / base._data.statsData.speed.value;

            _rigidBody.AddForce(resultForce, ForceMode.Acceleration);


        }
    }
}