using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Providers;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Core.Services
{
    public class PlayVfxSignal : ISignal {
        public string vfxName;
        public float[] position;
        public float[] normal;

        public PlayVfxSignal(string vfxName, float[] position, float[] normal) {
            this.vfxName = vfxName;
            this.position = position;
            this.normal = normal;
        }
    }


    public class VfxDataGroup : IEntityDataGroup
    {
        public ViewMonoData ViewMonoData;
        public MovementInputData MovementInputData;
        public IdData Idata;
    }

    public class VfxService : ISignalListener<PlayVfxSignal>, ISignalListener<OnSpawnedSignal>, ITickable {
        [Zenject.Inject]
        private ResourceService _resourceService;
        [Zenject.Inject]
        private GameContextDataService _contextDataService;

        private readonly Dictionary<IdData, GroundDustController> _controllers = new Dictionary<IdData, GroundDustController>();

        void ISignalListener<PlayVfxSignal>.OnSignal(PlayVfxSignal signal) {
            // Debug.Log($"Get vfx: <b>{signal.vfxName}</b>");
            var vfxResource = _resourceService.GetDataByName<ResourceView>(signal.vfxName);
            var vfxInstance = UnityEngine.Object.Instantiate(vfxResource, signal.position.ToVector3(), Quaternion.FromToRotation(Vector3.up, signal.normal.ToVector3()));
            Object.Destroy(vfxInstance.gameObject, 2f);
        }

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {
            _controllers.TryGetValue(signal.idData, out var controller);
            if (controller == null)
            {
                var dataGroup = _contextDataService.GetDataGroupById<VfxDataGroup>(signal.idData.Id);
                var vfxResource = _resourceService.GetData<GroundDustVfx>();

                controller = new GroundDustController(vfxResource, dataGroup);
                _controllers[signal.idData] = controller;
            }
        }

        public void Tick()
        {
            foreach (var controller in _controllers)
            {
                if (controller.Value is IActionReceiver<UpdateAction> c)
                {
                    c.Action();
                }
            }
        }
    }

    public class GroundDustController : IActionReceiver<UpdateAction> {
        private readonly GroundDustVfx _vfx;
        private readonly VfxDataGroup _dataGroup;
        private ParticleSystem _ps;
        private Transform _transform;
        private readonly LayerMask _layerMask;
        private TimerTime _timerTime = new TimerTime(){Time = 0.1f};
        private float _tempVal;
        private Animator _animator;

        public GroundDustController(GroundDustVfx vfx, VfxDataGroup dataGroup)
        {
            var shipView = dataGroup.ViewMonoData.GetView<ShipView>();
            _animator = shipView.GetComponent<Animator>();
            _transform = shipView.GetView<GroundVfxRootView>().transform;
            _vfx = UnityEngine.Object.Instantiate(vfx,_transform);
            _vfx.transform.localPosition = Vector3.zero;
            _vfx.transform.localRotation = Quaternion.identity;
            _dataGroup = dataGroup;
            _ps = _vfx.GetComponent<ParticleSystem>();
            _layerMask = vfx.Layer;

        }

        public void Action()
        {
            _timerTime.ProcessTimer(ref _tempVal, DoSmoke);

        }

        private void DoSmoke()
        {
            var front = _animator.GetBool(StaticAnimatorParameters.EngineFrontActive);
            var rear = _animator.GetBool(StaticAnimatorParameters.EngineRearActive);

            var sign = rear?1:front?-1:0;
            if(sign==0)
                return;
            var dir = _transform.position + _transform.up * 2f - _transform.up;
            if (Physics.Raycast(new Ray(_transform.position + _transform.up * 2f * sign, _transform.up * sign), out var hit, 15, _layerMask)) {
                Debug.DrawLine(_transform.position, hit.point, Color.blue, 0.1f);
                _vfx.transform.position = hit.point;
                _vfx.transform.LookAt(_transform);
                _ps.Emit(1);
            }
        }
    }
}