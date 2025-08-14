using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateController : MonoBehaviour
{
    public Animator Animator;
    public JumpAbility JumpAbility;
    public DashAbility DashAbility;
    public WeaponHandler WeaponHandler;
    public Rig aim;
    [HideInInspector] public CharacterController CharacterController;
    public StateMachine MovementSM { get; private set; }
    public StateMachine ActionSM { get; private set; }

    [HideInInspector] public float LastDashTime;

    public float speed;

    public IPlayerInput playerInput;


    public bool CanRotate { get; set; }
    public void RotateTowardsCamera()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Bỏ độ nghiêng dọc để tránh nhân vật ngửa/nghiêng
        if (cameraForward.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void Awake()
    {
        CanRotate = true;
        CharacterController = GetComponent<CharacterController>();
        MovementSM = new StateMachine();
        ActionSM = new StateMachine();
        playerInput = GetComponent<IPlayerInput>();
    }



    private void Start()
    {
        // Khởi động ở Idle + None
        MovementSM.ChangeState(new IdleState(this, MovementSM));
        ActionSM.ChangeState(new NoneState(this, ActionSM));
    }
    private void Update()
    {
        MovementSM.Update();
        ActionSM.Update();

        // Điều khiển Upper Body Layer (aim tay)
        Animator.SetLayerWeight(1, playerInput.IsAttackPressed() ? 1f : 0f);
    }

    public bool HasMovementInput()
    {
        return Mathf.Abs(playerInput.GetHorizontal()) > 0.1f || Mathf.Abs(playerInput.GetVertical()) > 0.1f;
    }

    public bool IsJumpPressed() => playerInput.IsJumpPressed();
    public bool IsDashPressed() => playerInput.IsDashPressed();
    public bool IsAttackPressed() => playerInput.IsAttackPressed();

    public void Move()
    {
        Vector3 input = new Vector3(playerInput.GetHorizontal(), 0, playerInput.GetVertical());
        Vector3 move = Camera.main.transform.forward * input.z + Camera.main.transform.right * input.x;
        move.y = 0;

        CharacterController.Move(move * speed* Time.deltaTime);

        if (move.magnitude > 0.1f && CanRotate)
        {
            // Chỉ xoay khi không bắn
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    

    }


    public void ApplyGravity()
    {
        if (!CharacterController.isGrounded)
            CharacterController.Move(Vector3.up * Physics.gravity.y * Time.deltaTime);
    }
}
