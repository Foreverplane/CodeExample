using DG.Tweening;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class HeatController : DataController<HeatGroup>, IActionReceiver<UpdateAction> {
        private readonly ResourceService _resourceService;
        private readonly HeatView _resource;
        private readonly HeatView _heatView;
        private readonly Transform _targetTransform;
        private Vector3 _tempVelocity;

        public HeatController(IEntityDataGroup heatGroup, ResourceService resourceService) : base(heatGroup) {
            _resourceService = resourceService;
            _resource = resourceService.GetData<HeatView>();
            _heatView = Object.Instantiate(_resource);
            _data.ViewMonoData.Views.Add(_heatView);
            _targetTransform = _data.ViewMonoData.GetView<ShipView>().gameObject.transform;
        
     
        }



        public void Action()
        {
            _heatView.transform.position = Vector3.SmoothDamp(_heatView.transform.position, _targetTransform.position + _heatView.Offset,
                ref _tempVelocity, 0.1f);
            _heatView.gameObject.SetActive(!_data.DeathData.IsDead);
        }
    }
}