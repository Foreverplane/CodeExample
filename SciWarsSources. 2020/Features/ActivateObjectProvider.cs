using System.Collections.Generic;
using UnityEngine;

public class ActivateObjectProvider : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _gameObjects;
    [SerializeField]
    private bool _isActive;

    private bool _currentIsActive;

    void Update()
    {
        if(_currentIsActive==_isActive)
            return;
        _gameObjects.ForEach(g=>g.SetActive(_isActive));
        _currentIsActive = _isActive;
    }
}