using UnityEngine;

public class WorldEdgeVfxView : View
{
    public ParticleSystem particleSystem;

    void OnValidate()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
}