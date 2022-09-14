using UnityEngine;

public class PreviewView : View
{
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,1);
    }
}