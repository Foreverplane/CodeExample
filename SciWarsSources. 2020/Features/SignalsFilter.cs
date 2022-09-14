using Zenject;

namespace Assets.Scripts.Core.Services
{
    public abstract class SignalsFilter {
        protected SignalBus signalBus;

        protected SignalsFilter(SignalBus signalBus) {
            this.signalBus = signalBus;
        }
    }
}