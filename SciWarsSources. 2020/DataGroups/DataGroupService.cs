using System.Collections.Generic;
using Providers;

namespace Assets.Scripts.Core.Services
{
    public abstract class DataGroupService {
        [Zenject.Inject]
        protected readonly Zenject.SignalBus signalBus;
        [Zenject.Inject]
        protected readonly GameContextDataService ContextDataService;
        protected readonly ObjectProviderService objectProviderService = new ObjectProviderService();

        protected void CreateControllers<TDataGroup>(IdData idData) where TDataGroup : class, IEntityDataGroup, new() {
            var dataGroup = ContextDataService.GetDataGroupById<TDataGroup>(idData.Id);

            var providerController = new ObjectProviderController();
            providerController.AddProviders(GetProviders());
            providerController.AddReceivers(GetReceivers(dataGroup));
            objectProviderService.AddController(idData, providerController);
        }

        protected virtual IEnumerable<ObjectProvider> GetProviders() { yield break; }

        protected virtual IEnumerable<object> GetReceivers(IEntityDataGroup datagroup) { yield break; }
    }
}