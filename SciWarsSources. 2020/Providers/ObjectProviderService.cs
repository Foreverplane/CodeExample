using System;
using System.Collections.Generic;

namespace Providers {
    public class ObjectProviderService {
        private readonly Dictionary<object, ObjectProviderController> _controllers = new Dictionary<object, ObjectProviderController>();

        public void AddController(object id, ObjectProviderController controller) {
            _controllers[id] = controller;
        }

        public void ProcessObject(object id, object obj) {
            _controllers[id].PassObject(obj);
        }

        public void Action<TAction>() where TAction : IAction {
            foreach (var objectProviderController in _controllers.Values) {
                foreach (var receiver in objectProviderController.Receivers) {
                    if (receiver is IActionReceiver<TAction> actionReceiver)
                        actionReceiver.Action();
                }
            }
        }
    }

    public interface IActionReceiver<TAction> where TAction : IAction {
        void Action();
    }

    public interface IAction { }

    public struct UpdateAction : IAction { }
    public struct FixedUpdateAction : IAction { }
    public struct InitializeAction : IAction { }


}