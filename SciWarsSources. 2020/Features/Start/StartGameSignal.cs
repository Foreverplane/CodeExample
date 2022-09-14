namespace Assets.Scripts.Core.Services
{
    public class StartGameSignal : ISignal {
        public readonly Entity e;

        public StartGameSignal(Entity e) {
            this.e = e;
        }
    }
}