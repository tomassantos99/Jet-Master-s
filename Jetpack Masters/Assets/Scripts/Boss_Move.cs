using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss_Move : StateMachineBehaviour
{
    // Updated on the scene
    public float speed = 50.0f;
    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(rb.position.x, player.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);

        // Travel to the player position
        rb.MovePosition(newPos);

        if (Math.Abs(player.position.y - rb.position.y) < 0.4f)
        {
            // So the attack animation gets triggered
            animator.SetTrigger("TriggerAttack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            animator.ResetTrigger("TriggerAttack");
    }

}
