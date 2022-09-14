using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    internal class MovementInputController : DataController<MovementService.MovementDataGroup>,IActionReceiver<UpdateAction> {

        public MovementInputController(IEntityDataGroup dataGroup) : base(dataGroup) {
            base._data.viewMonoData.GetView<ShipView>().gameObject.AddComponent<MovementInputDataWrapper>();

        }

        void IActionReceiver<UpdateAction>.Action() {
            if (base._data.ownerData.ownerId != PhotonNetwork.LocalPlayer.UserId)
            {
                return;
            }
            if (base._data.deathData.IsDead)
                return;
            if (base._data.currentDriverData.Driver == null)
                return;
            var movementInputDriver = base._data.currentDriverData.Driver.MovementInput;

            base._data.movementInputData.moveDirection[0] = movementInputDriver.x;
            base._data.movementInputData.moveDirection[1] = movementInputDriver.y;
        }
    }
}
