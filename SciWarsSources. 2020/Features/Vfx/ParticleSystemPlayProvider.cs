using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPlayProvider : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystem> _particleSystems;

    [SerializeField]
    private bool _isPlaying;

    private bool _current;

    void Update()
    {
        if (_isPlaying == _current)
            return;
        if (_isPlaying)
            _particleSystems.ForEach(p => p.Play());
        else
            _particleSystems.ForEach(p => p.Stop());

        _current = _isPlaying;
    }
}