using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    [Serializable]
    public class SettingsService {

        [SerializeField]
        private ScriptableObject[] _scriptableObjects;

        public TSettings[] GetSettings<TSettings>() {
            return _scriptableObjects.OfType<TSettings>().ToArray();
        }

    }
}
