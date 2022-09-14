using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts.Core.Services
{
    public class UserDriver : Driver {

        public UserDriver(object data) : base(data) {
        }

        public override bool IsShoot => CrossPlatformInputManager.GetButtonDown("Fire1");
        public override bool IsResurrectRequested => Input.GetKeyDown(KeyCode.R);
        public override bool IsFindTargetRequested => Input.GetKeyDown(KeyCode.F);

        public override void ProvideReward(float reward = 2f)
        {
        
        }


        public override Vector2 MovementInput => new Vector2() {
            x = CrossPlatformInputManager.GetAxis("Horizontal"),
            y = CrossPlatformInputManager.GetAxis("Vertical")

        };
    }
}