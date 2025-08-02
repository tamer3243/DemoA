using UnityEngine;

public class DashingState : IState
{
    private PlayerStateController player;

    public DashingState(PlayerStateController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.Animator.Play("Dash");
        player.DashAbility.Use(player.CharacterController, player.transform);
    }

    public void Update()
    {
        // Dash thường là 1 action ngắn, sau đó về Idle/Run
        if (Time.time >= player.LastDashTime + player.DashAbility.dashCooldown)
        {
            if (player.HasMovementInput())
                player.StateMachine.ChangeState(new RunningState(player));
            else
                player.StateMachine.ChangeState(new IdleState(player));

        }
    }

    public void Exit() { }
}
