using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    internal class RotationController : DataController<MovementService.MovementDataGroup>, IActionReceiver<UpdateAction>
    {
        private readonly Rigidbody _rigidBody;
        private float _vel;
        private float rotationZAxis;

        public RotationController(IEntityDataGroup dataGroup) : base(dataGroup) {
            _rigidBody = base._data.viewMonoData.GetView<ShipView>().GetComponent<Rigidbody>();
        }


        void IActionReceiver<UpdateAction>.Action() {
            if (base._data.ownerData.ownerId != PhotonNetwork.LocalPlayer.UserId) {
                return;
            }
            if (base._data.deathData.IsDead)
                return;



            var target = base._data.movementInputData.moveDirection[0];
            rotationZAxis = Mathf.SmoothDamp(rotationZAxis, target, ref _vel, 0.05f);

            _rigidBody.transform.Rotate(new Vector3(0, rotationZAxis * 5f, 0), Space.Self);
      
            // _rigidBody.AddTorque(new Vector3(0,0,rotationZAxis*500f),ForceMode.VelocityChange);
        }



    }
}