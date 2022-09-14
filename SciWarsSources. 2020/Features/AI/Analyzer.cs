namespace Assets.Scripts.Core.Services
{
    public abstract class Analyzer<TData> {
        protected TData data;

        protected Analyzer(TData data) {
            this.data = data;
        }

        public abstract void Analyze();

    }
}