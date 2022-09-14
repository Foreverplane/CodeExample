using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class CameraMovementService : ISignalListener<SelectSignal> {
        [Zenject.Inject]
        private CameraArm _cameraArm;

        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;

        public class CameraSelectedGroup : IEntityDataGroup {
            public ViewMonoData viewMonoData;
            public SelectData selectData;
        }

        [SerializeField]
        private CameraSelectedGroup _cameraSelectedGroup;

        void ISignalListener<SelectSignal>.OnSignal(SelectSignal signal) {
            _cameraSelectedGroup = _contextDataServiceService.GetDataGroups<CameraSelectedGroup>().FirstOrDefault(_ => _.selectData.IsSelected);

            _cameraArm.transform.DOMove(_cameraSelectedGroup.viewMonoData.GetView<ShipView>().transform.localPosition, 0.5f);
        }



    }
}
