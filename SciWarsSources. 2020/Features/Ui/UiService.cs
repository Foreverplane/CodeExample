using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class SetUIStateSignal : ISignal {
        public readonly Type uiState;

        public SetUIStateSignal(Type uiState) {
            this.uiState = uiState;
        }
    }

    public abstract class UiService : ISignalListener<SetUIStateSignal> {

        [Zenject.Inject]
        private List<Panel> _panels;
        protected List<UiState> _states;
        protected abstract List<UiState> UiStates { get; }
        private UiState _state;


        public TPanel GetPanel<TPanel>() where TPanel : Panel {
            return _panels.OfType<TPanel>().First();
        }


        public void SetState<TUiState>() where TUiState : UiState {
            if(_state?.GetType() == typeof(TUiState))
                return;
            _state = UiStates.OfType<TUiState>().First();
            Debug.Log($"Set state to <b>{_state.GetType().Name}</b>");
            foreach (var panel in _panels) {
                var isActive = _state.Panels.Contains(panel.GetType());
                Debug.Log($"<color=red>Set panel {panel.GetType().Name} active {isActive}</color>");
                panel.gameObject.SetActive(isActive);
            }
        }

        void ISignalListener<SetUIStateSignal>.OnSignal(SetUIStateSignal signal) {
            if (_state?.GetType() == signal.uiState)
                return;
            Debug.Log($"Set new UIState <b>{signal.uiState.Name}</b>");
            _state = UiStates.First(_ => _.GetType() == signal.uiState);
            foreach (var panel in _panels)
            {
                var isActive = _state.Panels.Contains(panel.GetType());
                Debug.Log($"<color=red>Set panel {panel.GetType().Name} active {isActive}</color>");
                panel.gameObject.SetActive(isActive);
            }

        }
    }

    public abstract class UiState {
        public Type[] Panels;

        protected UiState(params Type[] panels) {
            Panels = panels;
        }

        public class AliveUiState : UiState {
            public AliveUiState(params Type[] panels) : base(panels) {
            }
        }

        public class SpawnRequestedState : UiState {
            public SpawnRequestedState(params Type[] panels) : base(panels) {
            }
        }

        public class DeathUiState : UiState {
            public DeathUiState(params Type[] panels) : base(panels) {
            }
        }
    }
}