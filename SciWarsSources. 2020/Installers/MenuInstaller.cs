using System.Collections.Generic;
using Assets.Scripts.Core.Services;
using Stateless;
using Stateless.Graph;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller {

    [SerializeField]
    private StatsPanel _statsPanel;
    [SerializeField]
    private UiRoot _uiRoot;
    [SerializeField]
    private CameraArm _cameraArm;

    [SerializeField]
    private List<RootView> _rootViews;

    [SerializeField]
    private List<Panel> _panels;

    public override void InstallBindings() {
        
        Container.Bind<MainMenuContextDataService>().AsSingle();
        Container.DeclareSignal<ShipSettingsClickSignal>();
        Container.DeclareSignal<StatsPanelChange>().OptionalSubscriber();
        Container.BindInterfacesAndSelfTo<StatsPanel>().FromInstance(_statsPanel);
        Container.BindInterfacesAndSelfTo<UiRoot>().FromInstance(_uiRoot);
        Container.BindInterfacesAndSelfTo<CameraArm>().FromInstance(_cameraArm);
        Container.BindInterfacesAndSelfTo<List<RootView>>().FromInstance(_rootViews);
        Container.BindInterfacesAndSelfTo<List<Panel>>().FromInstance(_panels);
        Container.BindInterfacesAndSelfTo<StatsUiWindowService>().AsSingle();
        Container.BindInterfacesAndSelfTo<LocalUiService>().AsSingle();
        Container.BindInterfacesAndSelfTo<CameraMovementService>().AsSingle();
        Container.Bind(typeof(IInitializable), typeof(SelectService)).To<SelectService>().AsSingle();
        Container.DeclareSignal<SelectSignal>();
        Container.BindSignal<SelectSignal>().ToMethod<SelectService>(x => ((ISignalListener<SelectSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<SelectSignal>().ToMethod<StatsUiWindowService>(x => ((ISignalListener<SelectSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<SelectSignal>().ToMethod<LocalUiService>(x => ((ISignalListener<SelectSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<SelectSignal>().ToMethod<CameraMovementService>(x => ((ISignalListener<SelectSignal>)x).OnSignal).FromResolve();
        Container.BindInterfacesAndSelfTo<ConstructionService>().AsSingle();
        Container.BindInterfacesAndSelfTo<StartGameService>().AsSingle();
        Container.DeclareSignal<PlayClickSignal>();
        Container.BindSignal<PlayClickSignal>().ToMethod<StartGameService>(x => ((ISignalListener<PlayClickSignal>)x).OnSignal).FromResolve();
        Container.BindInterfacesAndSelfTo<MenuUiService>().AsSingle();
        Container.DeclareSignal<SetUIStateSignal>();
        Container.BindSignal<SetUIStateSignal>().ToMethod<MenuUiService>(x => ((ISignalListener<SetUIStateSignal>)x).OnSignal).FromResolve();
        Container.BindInterfacesAndSelfTo<ProfileDisplayService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MenuStateService>().AsSingle();
        Container.BindSignal<ShipSettingsClickSignal>().ToMethod<MenuStateService>(_ => _.OnShipSettingsClickSignal).FromResolve();
    }
}

public class MenuStateService : IInitializable {

    private StateMachine<MenuState, MenuTrigger> _StateMachine;
    [Inject]
    private SignalBus _SignalBus;

    public void Initialize() {
        _StateMachine = new StateMachine<MenuState, MenuTrigger>(MenuState.None);
        _StateMachine.Configure(MenuState.None).Permit(MenuTrigger.ToReadToPlay, MenuState.ReadyForPlay);
        _StateMachine.Configure(MenuState.ReadyForPlay).OnEntry(() => {
            Debug.Log($"On {MenuState.ReadyForPlay} enter");
        }).OnExit(() => {
            Debug.Log($"On {MenuState.ReadyForPlay} exit");
        }).Permit(MenuTrigger.ToShop, MenuState.Shop).Permit(MenuTrigger.ToTryBuy, MenuState.TryBuy).Permit(MenuTrigger.ToShipSettings, MenuState.ShipSettings);
        _StateMachine.Configure(MenuState.ShipSettings).OnEntry(() => {
            Debug.Log($"enter to: {MenuState.ShipSettings}");
            // _SignalBus.Fire<StatsPanelChange>(new StatsPanelChange(false));
        }).OnExit(() => {
            // _SignalBus.Fire<StatsPanelChange>(new StatsPanelChange(true));
        });
        _StateMachine.Fire(MenuTrigger.ToReadToPlay);
    }
    public void OnShipSettingsClickSignal(ShipSettingsClickSignal obj) {
        Debug.Log($"On ship settings {obj.selectableId} click");
        // _StateMachine.Fire(MenuTrigger.ToShipSettings);
    }
}

public enum MenuState {
    None,
    ReadyForPlay,
    TryBuy,
    Shop,
    ShipSettings
}

public enum MenuTrigger {
    None,
    ToReadToPlay,
    ToTryBuy,
    ToShop,
    ToShipSettings
}
