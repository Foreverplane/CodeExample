using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Services;
using UnityEngine;
using Zenject;

public class SWGameInstaller : MonoInstaller {

    [SerializeField]
    private string _RoomName;
    [SerializeField]
    private SpawnerView[] _SpawerViews;
    
    [SerializeField]
    private ControlPointView _ControlPoint;

    [SerializeField]
    private bool _learnMode;

    [SerializeField]
    private CanvasProvider _CanvasProvider;
    [SerializeField]
    private ArtifactPointsHolder _artifactPointsHolder;

    public override void InstallBindings() {
        Install();
    }

    private void Install() {
  
        Container.Bind<GameContextDataService>().AsSingle();
        Container.Bind(typeof(ITickable), typeof(TestService)).To<TestService>().AsSingle();
        Container.Bind<List<Panel>>().FromInstance(_CanvasProvider.Panels.ToList());
        Container.Bind<ArtifactPointsHolder>().FromInstance(_artifactPointsHolder);
        

        Container.Bind<GameUiService>().AsSingle();
        Container.DeclareSignal<SetUIStateSignal>();
        Container.BindSignal<SetUIStateSignal>().ToMethod<GameUiService>(x => ((ISignalListener<SetUIStateSignal>)x).OnSignal).FromResolve();
        Container.BindInterfacesAndSelfTo<GameUIStateService>().AsSingle();

        // Container.Bind<SciWarsAcademy>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();


        Container.BindInterfacesAndSelfTo<DriversService>().AsSingle().WithArguments(_ControlPoint);

        Container.DeclareSignal<OnSpawnedSignal>();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<DriversService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<PlayService>().AsSingle();

        Container.DeclareSignal<ExitSignal>();


        Container.BindInterfacesAndSelfTo<NetworkService>().FromNewComponentOnNewGameObject().AsSingle().WithArguments(_RoomName);
        Container.DeclareSignal<ConnectClientSignal>();
        Container.BindSignal<ConnectClientSignal>().ToMethod<NetworkService>(x => ((ISignalListener<ConnectClientSignal>)x).OnSignal).FromResolve();


        Container.DeclareSignal<SpawnClickSignal>();
        Container.DeclareSignal<RoomPropertiesUpdatedSignal>();
        Container.BindSignal<StartGameSignal>().ToMethod<PlayService>(x => ((ISignalListener<StartGameSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<ExitSignal>().ToMethod<PlayService>(x => ((ISignalListener<ExitSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<SpawnClickSignal>().ToMethod<PlayService>(x => ((ISignalListener<SpawnClickSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<SpawnClickSignal>().ToMethod<GameUIStateService>(x => ((ISignalListener<SpawnClickSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<RoomPropertiesUpdatedSignal>().ToMethod<PlayService>(x => ((ISignalListener<RoomPropertiesUpdatedSignal>)x).OnSignal).FromResolve();


        Container.BindInterfacesAndSelfTo<MovementService>().AsSingle();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<MovementService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<CameraService>().AsSingle();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<CameraService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();


        Container.BindInterfacesAndSelfTo<WeaponService>().AsSingle();
        Container.DeclareSignal<FireRocketSignal>();
        Container.DeclareSignal<OnEntitiesCreatedSignal>();
        Container.DeclareSignal<OnRocketHitSignal>();
        Container.DeclareSignal<OnEntitiesChangedSignals>();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<WeaponService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<FireRocketSignal>().ToMethod<WeaponService>(x => ((ISignalListener<FireRocketSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<OnEntitiesCreatedSignal>().ToMethod<WeaponService>(x => ((ISignalListener<OnEntitiesCreatedSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<OnRocketHitSignal>().ToMethod<WeaponService>(x => ((ISignalListener<OnRocketHitSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<OnEntitiesChangedSignals>().ToMethod<WeaponService>(x => ((ISignalListener<OnEntitiesChangedSignals>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<VfxService>().AsSingle();
        Container.DeclareSignal<PlayVfxSignal>();
        Container.BindSignal<PlayVfxSignal>().ToMethod<VfxService>(x => ((ISignalListener<PlayVfxSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<VfxService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();

        Container.Bind<DestroyEntityService>().AsSingle();
        Container.DeclareSignal<DestroyEntitySignal>();
        Container.BindSignal<DestroyEntitySignal>().ToMethod<DestroyEntityService>(x => ((ISignalListener<DestroyEntitySignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<HeatService>().AsSingle();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<HeatService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<DeadService>().AsSingle();

        Container.BindSignal<OnSpawnedSignal>().ToMethod<DeadService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();
        Container.DeclareSignal<DeathSignal>();
        Container.BindSignal<DeathSignal>().ToMethod<DeadService>(x => ((ISignalListener<DeathSignal>)x).OnSignal).FromResolve();
        Container.BindSignal<DeathSignal>().ToMethod<GameUIStateService>(x => ((ISignalListener<DeathSignal>)x).OnSignal).FromResolve();

        Container.Bind<SpawnerView[]>().FromInstance(_SpawerViews).AsSingle();

        Container.BindInterfacesAndSelfTo<BotService>().AsSingle();
        Container.DeclareSignal<CreateBotRequestSignal>();
        Container.BindSignal<CreateBotRequestSignal>().ToMethod<BotService>(x => ((ISignalListener<CreateBotRequestSignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<PhysicsJobsBatcher>().AsSingle();

        Container.BindInterfacesAndSelfTo<TargetService>().AsSingle();
        Container.BindSignal<OnSpawnedSignal>().ToMethod<TargetService>(x => ((ISignalListener<OnSpawnedSignal>)x).OnSignal).FromResolve();

        Container.DeclareSignal<FindTargetSignal>();
        Container.BindSignal<FindTargetSignal>().ToMethod<TargetService>(x => ((ISignalListener<FindTargetSignal>)x).OnSignal).FromResolve();

        Container.BindInterfacesAndSelfTo<PointsService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ChallengesService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ArtifactService>().AsSingle();

    }
}