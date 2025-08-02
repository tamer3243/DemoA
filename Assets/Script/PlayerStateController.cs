
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateController : MonoBehaviour
{
    public Animator Animator;
    public JumpAbility JumpAbility;
    public DashAbility DashAbility;

    [HideInInspector] public CharacterController CharacterController;
    [HideInInspector] public StateMachine StateMachine;

    [HideInInspector] public float LastDashTime;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        StateMachine = new StateMachine();
    }

    private void Start()
    {
        StateMachine.ChangeState(new IdleState(this));
    }

    private void Update()
    {
        StateMachine.Update();
    }

    public bool HasMovementInput()
    {
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;
    }

    public bool IsJumpPressed()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }

    public bool IsDashPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftShift);
    }

    public void Move()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 move = Camera.main.transform.forward * input.z + Camera.main.transform.right * input.x;
        move.y = 0;
        CharacterController.Move(move * 5f * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
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
