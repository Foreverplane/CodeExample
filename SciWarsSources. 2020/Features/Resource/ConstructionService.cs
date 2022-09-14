using System;
using System.Collections.Generic;
using System.Linq;
using MessagePack;
using UniRx;
using Zenject;

namespace Assets.Scripts.Core.Services {

    [Serializable]
    [MessagePackObject]
    public class RestrictionData : IEntityData {
        [IgnoreMember]
        public readonly ReactiveProperty<RestrictionReason> RestrictionReason = new ReactiveProperty<RestrictionReason>();

    }

    public abstract class RestrictionReason {
    }

    public class RestrictedByCurrency:RestrictionReason {
        public readonly int Amount;
        public RestrictedByCurrency(int amount) {
            Amount = amount;
        }
    }

    public class ConstructionService {
        [Zenject.Inject]
        private ResourceService _resourceService;
        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;

        [Zenject.Inject]
        private SettingsService _settingsService;
        [Inject]
        private ClientSessionData _sessionData;

        private List<View> _views = new List<View>();

        public Entity Construct(ResourceView resource) {
            var view = UnityEngine.Object.Instantiate(resource);
            var settings = _settingsService.GetSettings<ShipItemDataObject>().First(_ => _.name == resource.name);
            var entity = new Entity();
            entity.datas.Add(new SelectData(false));
            entity.datas.Add(new ViewMonoData(view.GetComponent<ShipView>()));
            entity.datas.Add(new ResourceData(resource.name));
            var restrictionData = new RestrictionData();
            if (_sessionData.ServerInventory.Count == 0) {
                _sessionData.ServerInventory.ObserveAdd().Subscribe(_ => {
                    if (_sessionData.ServerInventory.All(_ => _.ItemId != resource.name)) {
                        restrictionData.RestrictionReason.SetValueAndForceNotify(new RestrictedByCurrency(settings.currencyData.value));
                    }
                });
            }
            else {
                if (_sessionData.ServerInventory.All(_ => _.ItemId != resource.name)) {
                    restrictionData.RestrictionReason.SetValueAndForceNotify(new RestrictedByCurrency(settings.currencyData.value));
                }
            }

            entity.datas.Add(restrictionData);
            entity.datas.AddRange(settings.GetType().GetFields().Select(_ => _.GetValue(settings) as IEntityData).ToArray());
            var selectData = entity.GetData<StatsData>();
            var nameSelectData = entity.GetDataGroup<SelectNameGroup>();
            _contextDataServiceService.Add(entity);
            return entity;
        }

        public class SelectNameGroup : IEntityDataGroup {
            public SelectData selectData;
            public NameData nameData;
        }
    }
}
