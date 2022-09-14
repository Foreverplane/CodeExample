using UnityEngine;

public class DestructedChunkView : View {
    public Collider Collider;
    public Rigidbody Rigidbody;
    public MeshRenderer MeshRenderer;
    public Vector3 InitialLocalPosition;
    public Quaternion InitialLocalRotation;
    public Vector3 InitialLocalScale;

    void OnValidate() {
        Collider = Collider ?? GetComponent<Collider>();
        Rigidbody = Rigidbody ?? GetComponent<Rigidbody>();
        MeshRenderer = MeshRenderer ?? GetComponent<MeshRenderer>();
        InitialLocalPosition = transform.localPosition;
        InitialLocalRotation = transform.localRotation;
        InitialLocalScale = transform.localScale;
    }
}