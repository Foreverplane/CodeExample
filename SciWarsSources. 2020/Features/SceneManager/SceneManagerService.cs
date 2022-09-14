using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class SwitchSceneSignal : ISignal {
        public readonly string sceneName;
        public readonly Action onSceneLoaded;

        public SwitchSceneSignal(string sceneName, Action onSceneLoaded = null) {
            this.sceneName = sceneName;
            this.onSceneLoaded = onSceneLoaded;
        }
    }

    public class OnSceneLoadingProgressSignal : ISignal {
        public readonly float progress;

        public OnSceneLoadingProgressSignal(float progress) {
            this.progress = progress;
        }
    }

    public class SceneManagerService : ISignalListener<SwitchSceneSignal> {

        private AsyncOperation _operation;
        [Zenject.Inject]
        private readonly MonoProviderService _monoProviderService;
        [Zenject.Inject]
        private readonly ZenjectSceneLoader _sceneLoader;
        [Zenject.Inject]
        private readonly Zenject.SignalBus _signalBus;

        void ISignalListener<SwitchSceneSignal>.OnSignal(SwitchSceneSignal signal) {

            _monoProviderService.StartCoroutine(LoadScene(signal.sceneName, signal.onSceneLoaded));
        }

        private IEnumerator LoadScene(string sceneName, Action onSceneLoaded) {
            var op = _sceneLoader.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            //yield return op;
            while (op.progress < 1) {
                _signalBus.Fire(new OnSceneLoadingProgressSignal(op.progress));
                yield return null;
            }

            onSceneLoaded?.Invoke();
        }
    }
}