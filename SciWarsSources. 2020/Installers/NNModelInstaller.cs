using Assets.Scripts.Core.Services;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "NNModelInstaller", menuName = "Installers/NNModelInstaller")]
public class NNModelInstaller : ScriptableObjectInstaller<NNModelInstaller> {
    public NNModelService dataService;
    public override void InstallBindings() {
        Container.Bind<NNModelService>().FromInstance(dataService);
    }
}