namespace Assets.Scripts.Core.Services
{
    public abstract class DataController<TData> : Controller
    {
        protected readonly TData _data;

        protected DataController(IEntityDataGroup data)
        {
            _data = (TData)data;
        }
    }
}