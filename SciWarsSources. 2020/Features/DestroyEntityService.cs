using Photon.Pun;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class DestroyEntitySignal : ISignal {
        public readonly int id;

        public DestroyEntitySignal(int id) {
            this.id = id;
        }
    }

    public class DestroyEntityService : ISignalListener<DestroyEntitySignal> {
        [Zenject.Inject]
        private GameContextDataService _contextDataService;

        void ISignalListener<DestroyEntitySignal>.OnSignal(DestroyEntitySignal signal) {
            var e = _contextDataService.GetEntities().Find(_ => _.Id == signal.id);
            if (e == null) {
                Debug.LogError($"Cant destroy entity with id {signal.id} because of it does not exist in context!");
                return;
            }
       
            var destroyProperties = e.ConvertToDestroyableRoomProperties();
            PhotonNetwork.CurrentRoom.SetCustomProperties(destroyProperties);
            _contextDataService.Remove(e);
        }

    }
}