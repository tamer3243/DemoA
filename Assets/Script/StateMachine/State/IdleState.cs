public class IdleState : IState
{
    private PlayerStateController player;
    private StateMachine sm;

    public IdleState(PlayerStateController player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter() { player.Animator.CrossFadeInFixedTime("Idle", 0.15f); }
    public void Update()
    {
        if (player.HasMovementInput())
            sm.ChangeState(new RunningState(player, sm));
        if (player.IsJumpPressed())
            sm.ChangeState(new JumpingState(player, sm));
    }
    public void Exit() { }
}
