using System;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    [SerializeField]
    public struct AIDriverData {
        public Transform Transform;
        public ResourceService ResourceService;
        public Func<float> NormalizedHeat;
        public Func<bool> IsDead;
        public ControlPointView ControlPoint;
        public Func<Transform> DamageTarget;
        public Func<Vector3> TargetCourse;
        public Func<bool> IsTargetReachable;
        public Func<Vector3> CollisionPoint;
        public float SpeedStat;
        public Func<float> PotentionalShootHeat;
        public Func<float> CurrentHeat;
        public Func<float> MaxHeat;
    }
}