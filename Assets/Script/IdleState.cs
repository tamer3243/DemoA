using UnityEngine;

public class IdleState : IState
{
    private PlayerStateController player;

    public IdleState(PlayerStateController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.Play("Idle");
    }

    public void Update()
    {
        if (player.HasMovementInput())
        {
            player.StateMachine.ChangeState(new RunningState(player));
        }
        else if (player.IsJumpPressed())
        {
            player.StateMachine.ChangeState(new JumpingState(player));
        }
        else if (player.IsDashPressed())
        {
            player.StateMachine.ChangeState(new DashingState(player));
        }
    }

    public void Exit() { }
}
