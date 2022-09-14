using Photon.Pun;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class CameraService : IFixedTickable, ISignalListener<OnSpawnedSignal>
    {


        [Zenject.Inject]
        private GameContextDataService _contextDataService;

        [Zenject.Inject]
        private CameraTargetView _cameraTarget;


        private Transform _followedObject;

        public class CameraFollowGroup : IEntityDataGroup
        {
            public CameraFollowData _cameraFollowData;
            public ViewMonoData _viewMonoData;
            public OwnerData ownerData;
        }

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal)
        {
            var cameraFollowDataGroup = _contextDataService.GetDataGroupById<CameraFollowGroup>(signal.idData.Id);

            if (cameraFollowDataGroup?.ownerData.ownerId!=PhotonNetwork.LocalPlayer.UserId)
                return;
            _followedObject = cameraFollowDataGroup._viewMonoData.GetView<View>().transform;
        }

        void IFixedTickable.FixedTick()
        {
            if(_followedObject==null)
                return;
            _cameraTarget.transform.position = _followedObject.transform.position;
        }


    }
}