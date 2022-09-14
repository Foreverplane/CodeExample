using System.Linq;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class StartGameService : ISignalListener<PlayClickSignal> {
        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;
        [Zenject.Inject]
        private Zenject.SignalBus _signalBus;
        [Inject]
        private readonly ClientSessionData _ClientSessionData;
        public class PlayDataGroup : IEntityDataGroup {
            public SelectData selectData;
            public StatsData statsData;
            public NameData nameData;
            public InventoryData inventoryData;
            public ResourceData resourceData;
        }

        void ISignalListener<PlayClickSignal>.OnSignal(PlayClickSignal clickSignal) {
            var selectedShip = _contextDataServiceService.GetDataGroups<PlayDataGroup>().First(_ => _.selectData.IsSelected);
            _ClientSessionData.StartBattleWithShip.SetValueAndForceNotify(selectedShip.nameData.Name);
            var e = new Entity(selectedShip.inventoryData, selectedShip.nameData, selectedShip.statsData, selectedShip.resourceData);
            _signalBus.Fire(new SwitchSceneSignal("LoadingScene", () => {
                _signalBus.Fire(new SwitchSceneSignal("CityScene", () =>
                {
                    _signalBus.Fire(new StartGameSignal(e));
                }));

            }));

        }
    }
}
