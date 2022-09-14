using UnityEngine;

namespace Assets.Scripts.Core.Services
{
    public abstract class Driver {
        protected readonly object data;

        protected Driver(object data) {
            this.data = data;
        }

        public abstract Vector2 MovementInput { get; }
        public abstract bool IsShoot { get; }
        public abstract bool IsResurrectRequested { get; }
        public abstract bool IsFindTargetRequested { get; }

        public virtual void Update() {

        }

        public abstract void ProvideReward(float reward = 2f);
    }
}