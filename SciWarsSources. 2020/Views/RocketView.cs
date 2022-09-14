using System;
using UnityEngine;

public class RocketView : View {

    public event Action<ContactPoint> OnCollision = (col) => {
    };

    public IdData idData;

    public void ClearSubscribers() {
        OnCollision = (col) => { };
    }

    private bool _isCollide;
    void OnCollisionEnter(Collision collision) {
        if (_isCollide)
            return;
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
             //Debug.Log($"Ok. Contact. With {contact.otherCollider.name}");

            if (contact.otherCollider.GetComponent<RocketReceiverView>()) {
                _isCollide = true;
                OnCollision.Invoke(contact);
            }
            else
            {
                Debug.Log($"Cant get rocket receiver from {contact.otherCollider.name}");
            }

            return;
        }

    }

}