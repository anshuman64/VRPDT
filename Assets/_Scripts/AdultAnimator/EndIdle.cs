using UnityEngine;
using System.Collections;

public class EndIdle : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AdultController adultController = (AdultController)FindObjectOfType(typeof(AdultController));
        adultController.EndIdle();
    }
}
