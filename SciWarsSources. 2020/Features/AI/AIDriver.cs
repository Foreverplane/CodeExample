using System;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Core.Services
{
    [Serializable]
    public class AIDriver : Driver {
        [SerializeField]
        private AIDriverData _data;

        private readonly SciWarsDefaultAgent _agent;
        private readonly RayPerceptionSensorComponent3D _sensors;
        private PerceptionReceiver _target;
        private Func<Transform> _shootTarget;
        private BehaviourAggregator _behaviourAggregator;

        public AIDriver(object data) : base(data) {

            _data = (AIDriverData)data;
            _data.Transform.gameObject.AddComponent<AIDriverWrapper>().Data = this;
            var prefab = _data.ResourceService.GetDataByName<ResourceView>("Agent");
            _agent = Object.Instantiate(prefab).GetComponent<SciWarsDefaultAgent>();
            _sensors = _agent.GetComponentInChildren<RayPerceptionSensorComponent3D>(true);
            _agent.Rigidbody = _data.Transform.GetComponentInChildren<Rigidbody>();
           // _agent.gameObject.SetActive(true);
            _agent.Heat = _data.NormalizedHeat;
            _agent.IsDead = _data.IsDead;
            _agent.ControlPoint = _data.ControlPoint;
            _target = _agent.Rigidbody.GetComponentInChildren<PerceptionReceiver>();
            _BlackBoard = _agent.Rigidbody.GetComponentInChildren<BlackBoard>();
            _agent.DamageTarget = _data.DamageTarget;
            _agent.TargetCourse = _data.TargetCourse;
            
            _agent.IsTargetReachable = _data.IsTargetReachable;
            _agent.CollisionPoint = _data.CollisionPoint;
            _agent.SpeedStat = _data.SpeedStat;
            _agent.transform.SetParent(_data.Transform);
            _agent.transform.localPosition = Vector3.zero;

           _behaviourAggregator= _BlackBoard.GetComponent<BehaviourAggregator>();
            _BlackBoard.PotentionalHeat = _data.PotentionalShootHeat;
            _BlackBoard.CurrentHeat = _data.CurrentHeat;
            _BlackBoard.MaxHeat = _data.MaxHeat;
            _BlackBoard.DamageTarget = _data.DamageTarget;
            _BlackBoard.TargetCourse = _data.TargetCourse;
            _BlackBoard.IsTargetReachable = _data.IsTargetReachable;
            _BlackBoard.CollisionPoint = _data.CollisionPoint;
            _BlackBoard.IsDead = _data.IsDead;
        }

        private Vector2 _CurrentInput;
        private float _ReactionTime;
        private BlackBoard _BlackBoard;

        public override Vector2 MovementInput
        {
            get
            {
                _ReactionTime -= Time.deltaTime;
                if (_ReactionTime < 0)
                {
                    var b = _BlackBoard.MoveInput;
                    _CurrentInput = new Vector2(b[0], b[1]);
                    _ReactionTime = _behaviourAggregator.TargetReactionTime;
                }

                return _CurrentInput;
            }
        }

        public override bool IsShoot=> _BlackBoard.IsShoot;

        public override bool IsResurrectRequested => _BlackBoard.IsResurrectRequested;

        public override bool IsFindTargetRequested => _BlackBoard.IsFindTargetRequested;


        public override void Update() {
            if(Input.GetKeyDown(KeyCode.L))
                _agent.gameObject.SetActive(true);

           // _sensors.transform.position = _target.transform.position;
           // _sensors.transform.rotation = _target.transform.rotation;
        }

        public override void ProvideReward(float reward = 2f)
        {
            _agent.ProvideReward(reward);
        }

    }
}