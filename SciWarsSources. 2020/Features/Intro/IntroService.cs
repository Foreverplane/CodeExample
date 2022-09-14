using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services {
    public class IntroService : IInitializable {
        [Zenject.Inject]
        private readonly MonoProviderService _monoProviderService;
        [Zenject.Inject]
        private readonly Zenject.SignalBus _signalBus;
        [Inject]
        private readonly ClientSessionData _sessionData;

        private IEnumerator WaitAndLoad() {
            yield return new WaitForSeconds(3f);
            _signalBus.Fire(new SwitchSceneSignal("MainMenu"));
        }

        void IInitializable.Initialize() {
            // _monoProviderService.StartCoroutine(WaitAndLoad());
            _sessionData.IsCanLoading.Where(_=>_).Subscribe(_ => {
                _signalBus.Fire(new SwitchSceneSignal("MainMenu"));
            });
        }
    }
}
