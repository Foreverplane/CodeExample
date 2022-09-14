namespace Assets.Scripts.Core.Services
{
    public class SpawnableDataGroup : IEntityDataGroup {
        public IdData idData;
        public SpawnableData spawnableData;
        public OwnerData ownerData;
        public ResourceData resourceData;
        public DriverData driverData;
        public SpawnData spawnData;
        public DeathData deathData;
        public CurrentHeatData currentHeatData;
        public PhotonViewData photonViewData;
    }
}