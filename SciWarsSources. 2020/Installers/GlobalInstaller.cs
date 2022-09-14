using System;
using System.Runtime.CompilerServices;
using Assets.Scripts.Core.Services;
using UniRx;
using Zenject;

public class GlobalInstaller : MonoInstaller, IUnityEventProvider {
    public override void InstallBindings() {
        SignalBusInstaller.Install(Container);
        Container.BindInterfacesAndSelfTo<ClientSessionData>().AsSingle();
        BindSignals();
        Container.BindInterfacesAndSelfTo<ShaderService>().AsSingle();
        Container.BindInterfacesAndSelfTo<IUnityEventProvider>().FromInstance(this);
        Container.BindInterfacesAndSelfTo<MonoProviderService>().FromNewComponentOnNewGameObject().AsSingle();
        Container.BindInterfacesAndSelfTo<GlobalContextDataService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayFabPlayerStatisticsService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayFabInventoryService>().AsSingle();
        Container.BindInterfacesAndSelfTo<PlayFabLoginService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProfileService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ProfileChangesHandler>().AsSingle();
        Container.BindSignal<ChangeExperienceSignal>().ToMethod<ProfileService>(s => s.AddExperience).FromResolve();
        Container.BindSignal<ChangePointsSignal>().ToMethod<ProfileService>(s => s.AddPoints).FromResolve();
        Container.Bind<ISignalListener<SwitchSceneSignal>>().To<SceneManagerService>().AsSingle();
    }

    private void BindSignals() {
        Container.DeclareSignal<UserChangeNickNameSignal>().OptionalSubscriber();
        Container.DeclareSignal<OnLoginSuccessSignal>().OptionalSubscriber();
        Container.DeclareSignal<ChangePointsSignal>().OptionalSubscriber();
        Container.DeclareSignal<ChangeExperienceSignal>().OptionalSubscriber();
        Container.DeclareSignal<OnSceneLoadingProgressSignal>().OptionalSubscriber();
        Container.DeclareSignal<StartGameSignal>();
        Container.DeclareSignal<SwitchSceneSignal>();
        Container.BindSignal<SwitchSceneSignal>().ToMethod<ISignalListener<SwitchSceneSignal>>(x => x.OnSignal).FromResolve();
    }

    void OnDrawGizmos() {
        OnDrawGizmosAction();
    }

    public event Action OnDrawGizmosAction = () => {};
}