using System;
using System.Collections.Generic;
using System.Linq;

namespace Providers
{
    public class ObjectProviderController {
        private Dictionary<Type, object> _receivers = new Dictionary<Type, object>();
        private Dictionary<Type, ObjectProvider> _providers = new Dictionary<Type, ObjectProvider>();
        public ICollection<object> Receivers => _receivers.Values;

        public void AddProviders(IEnumerable<ObjectProvider> objectProviders) {
            _providers = objectProviders.ToDictionary(k => k.ObjectType, v => v);
        }

        public void AddReceivers(IEnumerable<object> objectReceivers) {
            _receivers = objectReceivers.ToDictionary(k => k.GetType(), v => v);
        }


        public void PassObject(object obj) {
            ObjectProvider provider = _providers[obj.GetType()];
            object receiver = _receivers[provider.ReceiverType];
            provider.ObjectFunc.Invoke(receiver).Invoke(obj);
        }
    }

  
}