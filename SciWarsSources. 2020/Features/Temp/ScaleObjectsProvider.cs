using System.Collections.Generic;
using UnityEngine;

public class ScaleObjectsProvider : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _gameObjects;

    void Update()
    {
        _gameObjects.ForEach(g=>g.transform.localScale = gameObject.transform.localScale);
    }
}