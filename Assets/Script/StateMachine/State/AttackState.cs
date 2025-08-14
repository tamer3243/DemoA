using UnityEditor;
using UnityEngine;

public class AttackState : IState
{
    private PlayerStateController player;
    private StateMachine sm;

    private float shootCooldown = 0.2f;
    private float lastShootTime;
 
    public AttackState(PlayerStateController player, StateMachine sm)
    {
        this.player = player;
        this.sm = sm;
    }

    public void Enter()
    {
        lastShootTime = -shootCooldown; // Cho phép bắn ngay lập tức
        player.Animator.SetLayerWeight(1, 1f);
        player.Animator.CrossFadeInFixedTime("Shoot", 0.3f);
        player.aim.weight =1;
    }

    public void Update()
    {
        player.RotateTowardsCamera();
        if (player.IsAttackPressed())
        {
    

            if (Time.time - lastShootTime >= shootCooldown)
            {
                player.WeaponHandler.Attack();
                lastShootTime = Time.time;
            }
        }
        else
        {
            sm.ChangeState(new NoneState(player, sm));
        }   
    }

    public void Exit() { 
        player.Animator.SetLayerWeight(1,0f);
        player.aim.weight = 0;
    }
}
