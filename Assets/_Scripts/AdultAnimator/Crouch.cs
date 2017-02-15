using UnityEngine;
using System.Collections;

public class Crouch : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AdultController adultController = (AdultController)FindObjectOfType(typeof(AdultController));
        adultController.Crouch();
    }
}
