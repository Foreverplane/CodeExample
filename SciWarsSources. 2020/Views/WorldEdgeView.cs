using UnityEngine;

public class WorldEdgeView : View
{


    public WorldEdgeVfxView vfxView;

    void OnCollisionEnter(Collision col)
    {
        foreach (var contactPoint in col.contacts)
        {
            var emitParams = new ParticleSystem.EmitParams {
                position = contactPoint.point,
                velocity = contactPoint.normal * 0.001f
            };
            vfxView.particleSystem.Emit(emitParams,1);

        }
    }


}