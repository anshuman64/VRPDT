using UnityEngine;
using System.Collections;

public class HappyIdleThirdPrompt : StateMachineBehaviour
{
    bool hasEntered;
    DogController dogController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasEntered)
        {
            hasEntered = true;
            dogController = (DogController)FindObjectOfType(typeof(DogController));
            dogController.ThirdPrompt();
        }
    }
}
