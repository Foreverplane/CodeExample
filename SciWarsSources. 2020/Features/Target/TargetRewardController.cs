using Providers;
using UnityEngine;

namespace Assets.Scripts.Core.Services {
    public class TargetRewardController : DataController<TargetDataGroup>, IActionReceiver<UpdateAction> {
        private Vector3 _course;
        private readonly IUnityEventProvider _eventProvider;
        private bool _isCollisionPointReachable;
        private readonly RocketRootView _rocketRoot;

        private readonly ShipView _ship;
        private Vector3 _collisionPoint;

        private LayerMask _layerMask = LayerMask.GetMask(new[] { "Default","WorldEdge","LP.Buildings" });

        public TargetRewardController(IEntityDataGroup data, IUnityEventProvider eventProvider) : base(data) {
            _eventProvider = eventProvider;
            _ship = _data.viewData.GetView<ShipView>();
            _rocketRoot = _ship.GetComponentInChildren<RocketRootView>();
            eventProvider.OnDrawGizmosAction += OnDrawGizmosAction;
        }


        public static Vector3 FindInterceptVector(Vector3 shotOrigin, float shotSpeed,
            Vector3 targetOrigin, Vector3 targetVel, float shotRadius, float targetRadius, out Vector3 shotCollisionPoint) {

            Vector3 dirToTarget = Vector3.Normalize(targetOrigin - shotOrigin);
            shotCollisionPoint = default;
            // Decompose the target's velocity into the part parallel to the
            // direction to the cannon and the part tangential to it.
            // The part towards the cannon is found by projecting the target's
            // velocity on dirToTarget using a dot product.
            Vector3 targetVelOrth =
                Vector3.Dot(targetVel, dirToTarget) * dirToTarget;

            // The tangential part is then found by subtracting the
            // result from the target velocity.
            Vector3 targetVelTang = targetVel - targetVelOrth;

            /*
        * targetVelOrth
        * |
        * |
        *
        * ^...7  <-targetVel
        * |  /.
        * | / .
        * |/ .
        * t--->  <-targetVelTang
        *
        *
        * s--->  <-shotVelTang
        *
        */

            // The tangential component of the velocities should be the same
            // (or there is no chance to hit)
            // THIS IS THE MAIN INSIGHT!
            Vector3 shotVelTang = targetVelTang;

            // Now all we have to find is the orthogonal velocity of the shot

            float shotVelSpeed = shotVelTang.magnitude;
            if (shotVelSpeed > shotSpeed) {
                // Shot is too slow to intercept target, it will never catch up.
                // Do our best by aiming in the direction of the targets velocity.
                return targetVel.normalized * shotSpeed;
            }
            else {
                // We know the shot speed, and the tangential velocity.
                // Using pythagoras we can find the orthogonal velocity.
                float shotSpeedOrth =
                    Mathf.Sqrt(shotSpeed * shotSpeed - shotVelSpeed * shotVelSpeed);
                Vector3 shotVelOrth = dirToTarget * shotSpeedOrth;

                // Finally, add the tangential and orthogonal velocities.

                // Find the time of collision (distance / relative velocity)
                float timeToCollision = ((shotOrigin - targetOrigin).magnitude - shotRadius - targetRadius)
                                        / (shotVelOrth.magnitude - targetVelOrth.magnitude);

                // Calculate where the shot will be at the time of collision
                Vector3 shotVel = shotVelOrth + shotVelTang;
                shotCollisionPoint = shotOrigin + shotVel * timeToCollision;

                return shotVelOrth + shotVelTang;
            }
        }




        void IActionReceiver<UpdateAction>.Action() {
            if (_data.currentTargetData.Target == null)
                return;
            // _course = GetCourse(_data.currentTargetData.Target, _rocketRoot.transform.position, out _targetTransform);

            var target = _data.currentTargetData.Target;
            var targetView = target.viewData.GetView<View>();
            var rigidBody = targetView.GetComponent<Rigidbody>();

            var shotOrigin = _rocketRoot.transform.position;
            var targetOrigin = targetView.transform.position;
            var shotRadius = 1f;
            var targetRadius = 5f;

            _course = FindInterceptVector(shotOrigin, 50, targetOrigin, rigidBody.velocity, shotRadius, targetRadius, out _collisionPoint);

            _isCollisionPointReachable = false;
            if (Physics.Linecast(shotOrigin, _collisionPoint, out var hit, _layerMask)) {
                _isCollisionPointReachable = hit.transform == targetView.transform;

            }
            else {
                _isCollisionPointReachable = true;
            }

            _data.currentTargetData.CollisionPoint = _collisionPoint;
            _data.currentTargetData.Course = _course;

            _data.currentTargetData.IsReachable = _isCollisionPointReachable;




        }

        private void OnDrawGizmosAction() {

            Gizmos.color = Color.green;

            if(_ship==null)
                return;
            Debug.DrawLine(_ship.transform.position, _collisionPoint, _isCollisionPointReachable ? Color.green : Color.red);
            Gizmos.DrawWireSphere(_collisionPoint, 1f);
        }




    }
}