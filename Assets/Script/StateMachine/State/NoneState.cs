public class NoneState : IState
{
    private PlayerStateController player;
    private StateMachine sm;

    public NoneState(PlayerStateController player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter() { }
    public void Update()
    {
        if (player.IsAttackPressed())
            sm.ChangeState(new AttackState(player, sm));
    }
    public void Exit() { }
}
