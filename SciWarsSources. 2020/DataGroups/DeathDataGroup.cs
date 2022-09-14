namespace Assets.Scripts.Core.Services {
    public class DeathDataGroup : IEntityDataGroup {
        public DeathData deathData;
        public ViewMonoData viewData;
        public IdData idData;
        public OwnerData ownerData;
        public DriverData driverData;
        public CurrentDriverData currentDriverData;
        public SpawnData spawnData;
        public KilledByData killedByData;
        public KillsData killsData;

    }

    public class ChallangeDataGroup : IEntityDataGroup {
        public KillsData killsData;
        public ProfileData profileData;
        public IdData idData;
        public DeathData deathData;

    }
}