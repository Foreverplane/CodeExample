using System;
using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    [Serializable]
    public struct AvoidancePoint {
        public Vector3 point;
        public float weight;

        public AvoidancePoint(Vector3 point, float weight) {
            this.point = point;
            this.weight = weight;
        }

        public override string ToString() {
            return JsonUtility.ToJson(this);
        }
    }
}