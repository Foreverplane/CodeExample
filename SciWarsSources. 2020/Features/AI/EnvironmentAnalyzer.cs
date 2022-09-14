using System;
using System.Collections.Generic;
using UnityEngine;

//[Serializable]
//public class EnvironmentAnalyzer : Analyzer<Transform> {
//    [SerializeField]
//    private float _avoidanceDistance = 10;
//    [SerializeField]
//    private float _avoidanceOffset;
//    [SerializeField]
//    private int _avoidanceRaysCount = 21;
//    [SerializeField]
//    private List<AvoidencePoint> _avoidencePoints;
//    [SerializeField]
//    private Collider _collider;
//    [SerializeField]
//    private int _layerMask;
//    [SerializeField]
//    private Transform _transform;
//    [SerializeField]
//    private AvoidencePoint _resultPoint;

//    public EnvironmentAnalyzer(Transform data) : base(data) {
//        _collider = data.GetComponentInChildren<Collider>();
//        _transform = data;
//        _avoidencePoints = new List<AvoidencePoint>();
//        var g = new GameObject($"EnvironmentAnalyzer for {data.name}");
//        g.AddComponent<EnvironmentAnalyzerWrapper>().Data = this;
//    }
//    public override void Analyze() {
//        var rays = _avoidanceRaysCount;

//        if (_avoidencePoints.Count > 20) {
//            _resultPoint = GetAvoidenceDirection();
//            Debug.DrawRay(_transform.position, (-_resultPoint.point + _transform.position) * 2, Color.blue, Time.deltaTime,false);
//            _avoidencePoints.Clear();
//        }


//        for (int i = 0; i < rays; i++) {
//            var dir = Quaternion.Euler(0, 0, 360 / rays * i + _avoidanceOffset) * Vector3.up;
//            Debug.DrawRay(_transform.position, dir * _avoidanceDistance, Color.white,Time.deltaTime,false);
//            var endPoint = _transform.position + dir * _avoidanceDistance;
//            var endAvoidancePoint = new AvoidencePoint(endPoint, Vector3.Distance(endPoint, _transform.position));

//            PhysicsJobsBatcher.Instance.ProcessRequest(new RaycastRequest() {
//                onPreProcess = () => { _collider.enabled = false; },
//                onPostProcess = () => { _collider.enabled = true; },
//                direction = dir,
//                origin = _transform.position,
//                distance = _avoidanceDistance,
//                onResult = hit => {


//                    if (hit.transform == null) {
//                        _avoidencePoints.Add(endAvoidancePoint);
//                        return;
//                    }

//                    _avoidencePoints.Add(new AvoidencePoint(hit.point, Vector3.Distance(_transform.position, hit.point)));

//                    Debug.DrawLine(_transform.position, hit.point, Color.green, Time.deltaTime,false);
//                }
//            });
//        }


//    }

//    public AvoidencePoint GetResults() {
//        return _resultPoint;
//    }
//    private AvoidencePoint GetAvoidenceDirection() {
//        var center = new AvoidencePoint();
//        foreach (var hit in _avoidencePoints) {
//            center.point += hit.point;
//            center.weight += hit.weight;
//        }

//        center.point /= _avoidencePoints.Count;
//        center.weight /= _avoidencePoints.Count;
//        return center;
//    }
//}