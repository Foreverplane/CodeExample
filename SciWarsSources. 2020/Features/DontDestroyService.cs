using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public class DontDestroyService : MonoBehaviour, ISignalListener {
        private GameObject _gameObject;
        void Awake()
        {
            if (_gameObject == null)
            {
                _gameObject = gameObject;
                DontDestroyOnLoad(_gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }
    }
}
