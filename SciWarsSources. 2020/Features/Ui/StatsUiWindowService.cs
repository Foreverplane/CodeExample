using System.Collections.Generic;
using System.Linq;
using TMPro;

namespace Assets.Scripts.Core.Services
{
    public class StatsUiWindowService : ISignalListener<SelectSignal> {
        [Zenject.Inject]
        private StatsPanel _statsPanel;

        [Zenject.Inject]
        private ResourceService _resourceService;

        [Zenject.Inject]
        private MainMenuContextDataService _contextDataServiceService;


        private List<PanelStatView> _views = new List<PanelStatView>();

        public class CurrentSelectedGroup : IEntityDataGroup {
            public SelectData selectData;
            public StatsData statsData;
            public NameData nameData;
        }

        private CurrentSelectedGroup _currentSelectedGroup;

        void ISignalListener<SelectSignal>.OnSignal(SelectSignal signal) {
            _currentSelectedGroup = _contextDataServiceService.GetDataGroups<CurrentSelectedGroup>().First(_ => _.selectData.IsSelected);
            _views.ForEach(Clear);
            _statsPanel.GetView<HeaderView>().text.text = _currentSelectedGroup.nameData.Name;
            foreach (var fieldInfo in _currentSelectedGroup.statsData.GetType().GetFields()) {
                var statView = GetStatView();
                statView.GetView<NameView>().GetComponent<TextMeshProUGUI>().text = fieldInfo.FieldType.Name;
                statView.GetView<CountView>().GetComponent<TextMeshProUGUI>().text =
                    (fieldInfo.GetValue(_currentSelectedGroup.statsData) as Stat<float>).value.ToString();
                statView.gameObject.GetComponent<IsDirtyComponent>().IsDirty = true;

            }
        }

        private void Clear(PanelStatView obj) {
            var isDirtyComponent = obj.GetComponent<IsDirtyComponent>();
            isDirtyComponent.IsDirty = false;
        }

        private PanelStatView GetStatView() {
            var inst = _views.Find(_ => !(bool)_.GetComponent<IsDirtyComponent>()?.IsDirty) ?? Create();

            return inst;
        }

        private PanelStatView Create() {
            var inst = UnityEngine.Object.Instantiate(_resourceService.GetData<PanelStatView>(), _statsPanel.GetView<AreaView>().transform).GetComponent<PanelStatView>();
            inst.gameObject.AddComponent<IsDirtyComponent>();
            _views.Add(inst);
            return inst;


        }

    }
}
