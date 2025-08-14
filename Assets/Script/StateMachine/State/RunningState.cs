using UnityEngine;

public class RunningState : IState
{
    private readonly PlayerStateController player;
    private StateMachine sm;
    private float factor = 2f;

    private string currentAnim; // lưu tên anim hiện tại để tránh restart liên tục

    public RunningState(PlayerStateController player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
        UpdateRunAnimation();
    }

    public void Update()
    {
        Vector3 input = new Vector3(player.playerInput.GetHorizontal(), 0, player.playerInput.GetVertical());
        Vector3 move = Camera.main.transform.forward * input.z + Camera.main.transform.right * input.x;
        move.y = 0;
        player.CharacterController.Move(move * factor * player.speed * Time.deltaTime);

        // Xoay nhân vật chỉ khi không bắn
        if (move.magnitude > 0.1f && !player.IsAttackPressed())
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Cập nhật blend tree
        player.Animator.SetFloat("MoveX", player.playerInput.GetHorizontal(), 0.1f, Time.deltaTime);
        player.Animator.SetFloat("MoveZ", player.playerInput.GetVertical(), 0.1f, Time.deltaTime);


        // Nếu đang chạy mà đổi giữa bắn / không bắn → đổi blend tree
        UpdateRunAnimation();

        // Đổi state
        if (!player.HasMovementInput())
        {
            player.MovementSM.ChangeState(new IdleState(player, sm));
        }

        if (player.IsJumpPressed())
        {
            player.MovementSM.ChangeState(new JumpingState(player, sm));
        }

        if (player.IsDashPressed())
        {
            player.MovementSM.ChangeState(new DashingState(player, sm));
        }
    }

    private void UpdateRunAnimation()
    {
        string targetAnim = player.IsAttackPressed() ? "ShootingMove" : "Run";
      
        if (currentAnim != targetAnim)
        {
            player.Animator.CrossFadeInFixedTime(targetAnim, 0.15f);
            currentAnim = targetAnim;
        }
    }

    public void Exit() { }
}
