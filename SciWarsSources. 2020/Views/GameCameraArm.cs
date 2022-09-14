using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class GameCameraArm : MonoBehaviour {
    [SerializeField]
    private float _minHeight;

    [SerializeField]
    private Transform[] _rotationWaypoints;
    [SerializeField]
    private float _speed = 50f;


    // Update is called once per frame
    void Update() {
        //Move();
        //Rotate();

    }

    private void Rotate() {

    }

    //void OnDrawGizmos() {
    //    for (int i = 0; i < _rotationWaypoints.Length; i++)
    //    {
    //        if (i < _rotationWaypoints.Length-1)
    //        {
    //            var previous = _rotationWaypoints[i];
    //            var next = _rotationWaypoints[i + 1];
    //            Gizmos.DrawLine(previous.transform.position,next.transform.position);
    //        }

    //    }
    //}

    private void Move() {
        var movementVector = new Vector3 {
            x = Input.GetKey(KeyCode.A) ? 1 : Input.GetKey(KeyCode.D) ? -1 : 0,
            y = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
        };
        transform.Translate(movementVector * Time.deltaTime * _speed, Space.Self);
        var pos = transform.position;
        pos.y = _minHeight <= pos.y ? pos.y : _minHeight;
        transform.position = pos;
    }
}
