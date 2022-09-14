namespace Assets.Scripts.Core.Services
{
    public class OnRocketHitSignal : ISignal
    {
        public readonly int rocketEntityId;
        public readonly int? damagableId;
        public readonly SurfaceType surfaceType;
        public readonly float[] position;
        public readonly float[] normal;

        public OnRocketHitSignal(int rocketEntityId, int? damagableId, SurfaceType surfaceType, float[] position, float[] normal) {
            this.rocketEntityId = rocketEntityId;
            this.damagableId = damagableId;
            this.surfaceType = surfaceType;
            this.position = position;
            this.normal = normal;
        }
    }
}