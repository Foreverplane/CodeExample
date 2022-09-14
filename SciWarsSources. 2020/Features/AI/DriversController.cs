using System;
using Photon.Pun;
using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services {
    public class DriversController : DataController<DriversService.DriversDataGroup>, IActionReceiver<UpdateAction> {
        private readonly ResourceService _resourceService;
        private readonly ControlPointView _controlPoint;
        private readonly Transform _transform;
        private readonly Func<float> _normalizedHeat;
        private readonly Func<bool> _isDead;
        private readonly Func<Transform> _damageTarget;
        private Func<Vector3> _targetCourse;
        private Func<bool> _isTargetReachable;
        private AIDriverData _DriverData;
        private Func<Vector3> _collisionPoint;
        private Func<float> _potentionalShootHeat;
        private Func<float> _currentHeat;
        private Func<float> _maxHeat;

        public DriversController(IEntityDataGroup dataGroup, ResourceService resourceService,
            ControlPointView controlPoint) : base(dataGroup) {
            _resourceService = resourceService;
            _controlPoint = controlPoint;
            _transform = _data.viewMonoData.GetView<View>().transform;
            _normalizedHeat = () => _data.currentHeatData.NormalizedValue;
            _isDead = () => _data.deathData.IsDead;
            _damageTarget = () => _data.currentTargetData.Target?.viewData.GetView<View>()?.transform;
            _targetCourse = () => _data.currentTargetData.Course;
            _isTargetReachable = () => _data.currentTargetData.IsReachable;
            _collisionPoint = () => _data.currentTargetData.CollisionPoint;
            _potentionalShootHeat = () => 20;
            _currentHeat = () => _data.currentHeatData.CurrentValue;
            _maxHeat = () => _data.maxHeatData.maxHeat;
            _DriverData = new AIDriverData() {
                Transform = _transform,
                ResourceService = _resourceService,
                NormalizedHeat = _normalizedHeat,
                CurrentHeat = _currentHeat,
                MaxHeat = _maxHeat,
                IsDead = _isDead,
                PotentionalShootHeat = _potentionalShootHeat,
                ControlPoint = _controlPoint,
                DamageTarget = _damageTarget,
                TargetCourse = _targetCourse,
                IsTargetReachable = _isTargetReachable,
                CollisionPoint = _collisionPoint,
                SpeedStat = _data.statsData.speed.value
            };

        }
        private Driver Driver => DriversStorage.GetDriver(new DriverDescriptor(_data.idData, _data.driverData.driverType), GetDriverData);
        private object GetDriverData => _data.driverData.driverType == DriverData.DriverType.AI
            ? _DriverData
            : _transform as object;

        void IActionReceiver<UpdateAction>.Action() {

            _data.currentDriverData.Driver = _data.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId ? Driver : null;
            _data.currentDriverData.Driver?.Update();
        }
    }
}