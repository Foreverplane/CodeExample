using Assets.Scripts.Core.Services;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller> {
  
    public SettingsService settingsService;

    public override void InstallBindings() {
        Container.Bind<SettingsService>().FromInstance(settingsService);
    }
}