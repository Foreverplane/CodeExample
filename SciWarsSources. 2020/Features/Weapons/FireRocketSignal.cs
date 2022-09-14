namespace Assets.Scripts.Core.Services
{
    public class FireRocketSignal : ISignal {
        public readonly int id;
        public readonly float[] position;
        public readonly float[] rotation;

        public FireRocketSignal(int id, float[] position, float[] rotation) {
            this.id = id;
            this.position = position;
            this.rotation = rotation;
        }
    }
}