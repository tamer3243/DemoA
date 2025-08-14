using UnityEngine;

public class JumpingState : IState
{
    private  PlayerStateController player;
    private StateMachine sm;

    public JumpingState(PlayerStateController player,StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
        player.Animator.CrossFadeInFixedTime("Jump", 0.15f);

        // Gán vận tốc nhảy ban đầu
        player.JumpAbility.verticalVelocity = player.JumpAbility.jumpForce;
    }

    public void Update()
    {
        // Áp dụng gravity mượt hơn
        if (player.JumpAbility.verticalVelocity > 0 && !player.playerInput.IsJumpPressed())
            player.JumpAbility.verticalVelocity += player.JumpAbility.gravity * player.JumpAbility.fallMultiplier * Time.deltaTime;
        else
            player.JumpAbility.verticalVelocity += player.JumpAbility.gravity * Time.deltaTime;

        Vector3 input = new Vector3(player.playerInput.GetHorizontal(), 0, player.playerInput.GetVertical());
        Vector3 move = Camera.main.transform.forward * input.z + Camera.main.transform.right * input.x;
        move.y = 0;

        move *= player.speed * Time.deltaTime;
        move.y = player.JumpAbility.verticalVelocity * Time.deltaTime;

        player.CharacterController.Move(move);

        // Đổi state khi chạm đất
        if (player.CharacterController.isGrounded && player.JumpAbility.verticalVelocity < 0)
        {
            if (player.HasMovementInput())
                sm.ChangeState(new RunningState(player, sm));
            else
                sm.ChangeState(new IdleState(player, sm));
        }
        if (player.playerInput.IsDashPressed()) sm.ChangeState(new DashingState(player, sm));
    }

    public void Exit() { }
}
