using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    //public abstract class MonoService : MonoBehaviour, ISignalListener {
    //    protected virtual void Awake()
    //    {
    //        var type = GetType();
        
    //        var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
    //        var injectableFields = fields.Where(prop => prop.IsDefined(typeof(InjectAttribute), false)).ToArray();
    //        // Debug.Log($"For type {type.Name} find {fields.Length} fields with inject {injectableFields.Length}");
    //        foreach (var field in injectableFields) {
    //            var service = GetComponent(field.FieldType);
    //            field.SetValue(this, service);
    //        }
    //    }

    //    protected virtual void Start() { }

    //    void OnEnable()
    //    {
    //        StaticSignalBus.Subscribe(this);
    //    }

    //    void OnDisable()
    //    {
    //        StaticSignalBus.UnSubscribe(this);
    //    }
    //}
}
