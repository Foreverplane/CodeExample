using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Zenject;
using Zenject.Asteroids;

namespace Assets.Scripts.Core.Services
{
    
    
    public class SelectSignal : ISignal {
        public readonly Vector3 Direction;

        public SelectSignal(Vector3 direction) {
            Direction = direction;
        }
    }

    public class SelectService : IInitializable, ISignalListener<SelectSignal> {

        [Zenject.Inject]
        private ResourceService _resourceService;
        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;

        [Zenject.Inject]
        private ConstructionService _constructionService;
        [Zenject.Inject]
        private Zenject.SignalBus _signalBus;

        [Zenject.Inject]
        private List<RootView> _rootViews;
        [Inject]
        private ClientSessionData _sessionData;

        private List<SelectDataGroup> _selectDatas;

        private int _currentSelected;

        void IInitializable.Initialize() {
            var ships = _resourceService.GetDatas<ResourceView>().Where(_ => _.GetComponent<ShipView>() != null);
            var i = 0;
            foreach (var ship in ships) {
                var entity = _constructionService.Construct(ship);
                var view = entity.GetData<ViewMonoData>();
                var rootView = _rootViews[i];
                view.Views[0].transform.position = rootView.transform.localPosition;
                var lockedView = rootView.GetView<LockWidgetView>();
                var settings = rootView.GetView<ShipSettingsWidgetView>();
                var restrictionData = entity.GetData<RestrictionData>();
                settings.gameObject.SetActive(false);
                lockedView.gameObject.SetActive(false);
                restrictionData.RestrictionReason.Subscribe(_ => {
                    if (_ is RestrictedByCurrency currencyRestriction) {
                        settings.gameObject.SetActive(false);
                        lockedView.gameObject.SetActive(true);
                        var lockText = lockedView.GetView<LockTextView>();
                        lockText.Value = $"{currencyRestriction.Amount.ToString()} c";
                    }
                    else {
                        settings.gameObject.SetActive(true);
                        lockedView.gameObject.SetActive(false);
                    }  
                });
                i++;
            }

            _selectDatas = _contextDataServiceService.GetDataGroups<SelectDataGroup>().ToList();
            _signalBus.Fire(new SelectSignal(default));
        }

        [Serializable]
        public class SelectDataGroup : IEntityDataGroup {
            public SelectData selectData;
        }



        void ISignalListener<SelectSignal>.OnSignal(SelectSignal signal) {

            if (signal.Direction.x < 0) {
                _currentSelected++;
            }
            if (signal.Direction.x > 0) {
                _currentSelected--;
            }

            if (signal.Direction == default) {
                _currentSelected = 0;
            }
            if (_currentSelected == _selectDatas.Count)
                _currentSelected = 0;
            if (_currentSelected < 0)
                _currentSelected = _selectDatas.Count - 1;

            var currentSelected = _selectDatas[_currentSelected];
            currentSelected.selectData.IsSelected = true;
            foreach (var selectData in _selectDatas.Where(_ => _ != currentSelected)) {
                selectData.selectData.IsSelected = false;
            }

            Debug.Log($"{_contextDataServiceService}");
        }

    }
}