using System.Linq;
using DG.Tweening;

namespace Assets.Scripts.Core.Services
{
    public class LocalUiService : ISignalListener<SelectSignal> {
    
        [Zenject.Inject]
        private UiRoot _ArrowsView;

        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;

        private CameraMovementService.CameraSelectedGroup _cameraSelectedGroup;
   
        void ISignalListener<SelectSignal>.OnSignal(SelectSignal signal)
        {
            _cameraSelectedGroup = _contextDataServiceService.GetDataGroups<CameraMovementService.CameraSelectedGroup>()
                .FirstOrDefault(_ => _.selectData.IsSelected);
            _ArrowsView.transform.DOMove(_cameraSelectedGroup.viewMonoData.GetView<ShipView>().transform.localPosition, 0.7f);
        }
    }
}
