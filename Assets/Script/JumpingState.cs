using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class JumpingState : IState
{
    private PlayerStateController player;

    public JumpingState(PlayerStateController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.Play("Jump");
        player.JumpAbility.Use(player.CharacterController, player.transform);
    }

    public void Update()
    {
        player.ApplyGravity();

        if (player.CharacterController.isGrounded)
        {
            if (player.HasMovementInput())
                player.StateMachine.ChangeState(new RunningState(player));
            else
                player.StateMachine.ChangeState(new IdleState(player));

        }
    }

    public void Exit() { }
}
