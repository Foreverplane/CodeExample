using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    public bool IsBehaviourActive;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        IsBehaviourActive = true;
       // Debug.Log($"Enter to {GetType().Name}");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        IsBehaviourActive = false;
       // Debug.Log($"Exit form {GetType().Name}");
    } 
}