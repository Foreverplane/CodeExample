namespace Assets.Scripts.Core.Services
{
    public class OnSpawnedSignal : ISignal {
        public IdData idData;

        public OnSpawnedSignal(IdData idData) {
            this.idData = idData;
        }
    }
}