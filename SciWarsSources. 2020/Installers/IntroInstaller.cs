using Assets.Scripts.Core.Services;
using UnityEngine;
using Zenject;

public class IntroInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IInitializable>().To<IntroService>().AsSingle();
    }
}