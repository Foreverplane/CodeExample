using System.Collections.Generic;
using Providers;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class DriversService : DataGroupService, ISignalListener<OnSpawnedSignal>, ITickable,ILateDisposable {
        private readonly ControlPointView _controlPoint;

        public class DriversDataGroup : IEntityDataGroup {
            public IdData idData;
            public DriverData driverData;
            public CurrentDriverData currentDriverData;
            public OwnerData ownerData;
            public ViewMonoData viewMonoData;
            public CurrentHeatData currentHeatData;
            public DeathData deathData;
            public CurrentTargetData currentTargetData;
            public StatsData statsData;
            public MaxHeatData maxHeatData;
        }

        [Zenject.Inject]
        private ResourceService _resourceService;

        [Zenject.Inject]
        public DriversService(ControlPointView controlPoint) {
            _controlPoint = controlPoint;
        }



        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal) {
            base.CreateControllers<DriversDataGroup>(signal.idData);

        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            yield return new DriversController(datagroup, _resourceService, _controlPoint);
        }

        void ITickable.Tick() {
            base.objectProviderService.Action<UpdateAction>();
        }

        public void LateDispose()
        {
            DriversStorage.Dispose();
        }
    }
}