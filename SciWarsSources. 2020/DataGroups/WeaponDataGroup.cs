namespace Assets.Scripts.Core.Services
{
    public class WeaponDataGroup : IEntityDataGroup {
        public OwnerData ownerData;
        public IdData idData;
        public ViewMonoData viewMonoData;
        public DriverData driverData;
        public DeathData deathData;
        public CurrentDriverData currentDriverData;
        public ShootData shootData;
        public CurrentTargetData currentTargetData;
    }


    public class ShipDataGroup : IEntityDataGroup
    {
        public IdData idData;
        public ViewMonoData viewMonoData;
        public OwnerData ownerData;
        public DriverData driverData;
        public DeathData deathData;
    }
}