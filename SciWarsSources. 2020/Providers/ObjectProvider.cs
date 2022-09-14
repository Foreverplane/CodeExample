using System;

namespace Providers
{
    public class ObjectProvider {
        public Type ObjectType { get; }
        public Type ReceiverType { get; }
        public Func<object, Action<object>> ObjectFunc { get; }

        private ObjectProvider(Type objectType, Type receiverType, Func<object, Action<object>> objectFunc) {
            ObjectType = objectType;
            ReceiverType = receiverType;
            ObjectFunc = objectFunc;
        }

        public static ObjectProvider Create<TObject, TReceiver>(Func<TReceiver, Action<TObject>> func) {
            var objectFunc = new Func<object, Action<object>>(tReceiver => (tSignal) => {
                func.Invoke((TReceiver)tReceiver).Invoke((TObject)tSignal);
            });
            return new ObjectProvider(typeof(TObject), typeof(TReceiver), objectFunc);
        }
    }
}