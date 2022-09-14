using Providers;

namespace Assets.Scripts.Core.Services
{
    public class TargetInputController : DataController<TargetDataGroup>, IActionReceiver<UpdateAction>
    {
        private readonly TargetService.TargetInputSignals _signals;
        private bool _requested;

        public TargetInputController(IEntityDataGroup data, TargetService.TargetInputSignals signals) : base(data)
        {
            _signals = signals;
        }

        void IActionReceiver<UpdateAction>.Action()
        {
            if (_data.currentDriverData.Driver == null)
                return;
            if (_data.deathData.IsDead) {
                return;
            }

            if (_requested) {
                _requested = false;
                return;
            }

        

            if (!_requested && _data.currentDriverData.Driver.IsFindTargetRequested) {
                _signals.FireFindTargetSignal(new FindTargetSignal(_data.idData));
                //Debug.Log($"FindTargetSignal fired for {_data.idData.Id} as false by AI");
                _requested = true;
            }
        }
    }
}