using UnityEngine;

public class ArtifactPointView : View
{
    [SerializeField]
    private Color _color = Color.red;
    [SerializeField]
    private float _radius = 1;
    void OnDrawGizmos() {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}