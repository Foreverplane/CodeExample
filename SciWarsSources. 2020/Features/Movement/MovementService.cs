using System.Collections.Generic;
using Providers;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class MovementService : DataGroupService, ISignalListener<OnSpawnedSignal>, IFixedTickable
    {

        [Zenject.Inject]
        private GameContextDataService _contextDataService;
        readonly Dictionary<int,List<IDataGroupController>> _dictionary = new Dictionary<int, List<IDataGroupController>>();

        public class MovementDataGroup : IEntityDataGroup {
            public IdData idData;
            public ViewMonoData viewMonoData;
            public MovementInputData movementInputData;
            public DriverData driverData;
            public OwnerData ownerData;
            public DeathData deathData;
            public StatsData statsData;
            public CurrentDriverData currentDriverData;
        }

        void ISignalListener<OnSpawnedSignal>.OnSignal(OnSpawnedSignal signal)
        {
            base.CreateControllers<MovementDataGroup>(signal.idData);
        }

        protected override IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) {
            yield return new MovementInputController(datagroup);
            yield return new RotationController(datagroup);
            yield return new MovementController(datagroup);
            yield return new ShipAnimatorController(datagroup);
        }

        void IFixedTickable.FixedTick()
        {
            base.objectProviderService.Action<UpdateAction>();
        }
    }
}