using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class TestService : ITickable
    {
        [Zenject.Inject]
        private Zenject.SignalBus _signalBus;
        [Zenject.Inject]
        private SettingsService _settingsService;

        void ITickable.Tick()
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            Debug.Log($"Start test game requested!");
            var e = GetStartGameEntity;
            var signal = new StartGameSignal(e);
            _signalBus.Fire(signal);
        }

        private Entity GetStartGameEntity
        {
            get
            {
                var settings = _settingsService.GetSettings<ShipItemDataObject>().OrderBy(_ => Guid.NewGuid()).First();
                var datas = settings.GetType().GetFields().Select(_ => _.GetValue(settings) as IEntityData).ToArray();

                var inventoryData = datas.OfType<InventoryData>().First();
                var nameData = datas.OfType<NameData>().First();
                var statsData = datas.OfType<StatsData>().First();
                var resourceData = new ResourceData(settings.name);

                return new Entity(inventoryData, nameData, statsData, resourceData);
            }
        }

   
    }
}