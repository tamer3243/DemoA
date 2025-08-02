using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class RunningState : IState
{
    private PlayerStateController player;

    public RunningState(PlayerStateController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.Play("Run");
    }

    public void Update()
    {
        player.Move();

        if (!player.HasMovementInput())
            player.StateMachine.ChangeState(new IdleState(player));

        if (player.IsJumpPressed())
            player.StateMachine.ChangeState(new JumpingState(player));

        if (player.IsDashPressed())
            player.StateMachine.ChangeState(new DashingState(player));
    }

    public void Exit() { }
}
