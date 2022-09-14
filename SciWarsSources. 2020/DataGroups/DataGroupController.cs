namespace Assets.Scripts.Core.Services
{
    public abstract class DataGroupController<TDataGroup> : IDataGroupController
    {
        protected readonly TDataGroup _dataGroup;

        protected DataGroupController(TDataGroup dataGroup)
        {
            _dataGroup = dataGroup;
        }

        public abstract void Update();
    }
}
