using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services
{
    public class RaycastRequest {
        public Vector3 origin;
        public Vector3 direction;
        public Action<RaycastHit> onResult;
        public float distance;
        public int layerMask = -1;
        public Action onPreProcess;
        public Action onPostProcess;
    }

    public class PhysicsJobsBatcher : ITickable, ILateTickable  {


        private readonly List<RaycastRequest> _requests = new List<RaycastRequest>();
        private JobHandle? _handle;
        private NativeArray<RaycastHit> _results;
        private NativeArray<RaycastCommand> _commands;
        private RaycastRequest[] _currentRequests;

        public void ProcessRequest(RaycastRequest request) {
            _requests.Add(request);
        }

        void ITickable.Tick() {
            if (_requests.Count == 0)
                return;
            _currentRequests = _requests.ToArray();
            _requests.Clear();
            var count = _currentRequests.Length;
            _results = new NativeArray<RaycastHit>(count, Allocator.TempJob);

            _commands = new NativeArray<RaycastCommand>(count, Allocator.TempJob);
            for (int i = 0; i < count; i++) {
                var r = _currentRequests[i];
                r.onPreProcess();
                _commands[i] = new RaycastCommand(r.origin, r.direction, r.distance, r.layerMask);
            }

            _handle = RaycastCommand.ScheduleBatch(_commands, _results, 1);

        }


        void ILateTickable.LateTick() {

            if (Equals(_handle, null))
                return;


            _handle.Value.Complete();

            // Copy the result. If batchedHit.collider is null there was no hit
            var count = _currentRequests.Length;
            for (int i = 0; i < count; i++) {
                try {
                    var res = _results[i];
                    var r = _currentRequests[i];
                    r.onPostProcess();
                    r.onResult(res);
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    throw;
                }

            }

            // Dispose the buffers
            _results.Dispose();
            _commands.Dispose();
            _handle = null;
        }

        private void RaycasExample() {
            // Perform a single raycast using RaycastCommand and wait for it to complete
            // Setup the command and result buffers
            var results = new NativeArray<RaycastHit>(1, Allocator.Temp);

            var commands = new NativeArray<RaycastCommand>(1, Allocator.Temp);

            // Set the data of the first command
            Vector3 origin = Vector3.forward * -10;

            Vector3 direction = Vector3.forward;

            commands[0] = new RaycastCommand(origin, direction);

            // Schedule the batch of raycasts
            JobHandle handle = RaycastCommand.ScheduleBatch(commands, results, 1, default(JobHandle));

            // Wait for the batch processing job to complete
            handle.Complete();

            // Copy the result. If batchedHit.collider is null there was no hit
            RaycastHit batchedHit = results[0];

            // Dispose the buffers
            results.Dispose();
            commands.Dispose();
        }





    }
}