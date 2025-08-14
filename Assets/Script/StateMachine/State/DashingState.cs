
using UnityEngine;

public class DashingState : IState
{
    private readonly PlayerStateController player;
    private float dashStartTime;
    StateMachine sm;

    public DashingState(PlayerStateController player,StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
        player.Animator.CrossFadeInFixedTime("Dash", 0.05f);
        player.LastDashTime = Time.time;
        dashStartTime = Time.time;
        player.DashAbility.Use(player.CharacterController,player.transform);
    }

    public void Update()
    {
        player.Move();
        if (!player.CharacterController.isGrounded) sm.ChangeState(new JumpingState(player, sm));
        if (Time.time >= dashStartTime + player.DashAbility.dashDuration)
        {
            if (player.HasMovementInput())
                sm.ChangeState(new RunningState(player,sm));
            else
               sm.ChangeState(new IdleState(player,sm));
        }
    }

    public void Exit() { }
}
