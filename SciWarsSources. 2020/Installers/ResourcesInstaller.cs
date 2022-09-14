using Assets.Scripts.Core.Services;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ResourcesInstaller", menuName = "Installers/ResourcesInstaller")]
public class ResourcesInstaller : ScriptableObjectInstaller<ResourcesInstaller>
{
    public ResourceService resourceService;
    public override void InstallBindings()
    {
        Container.Bind<ResourceService>().FromInstance(resourceService);
    }
}