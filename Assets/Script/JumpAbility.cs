using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class JumpAbility : AbilityBase
{
    public float jumpHeight = 5f;
    public float gravity = -9.81f;

    private float verticalVelocity;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void Use(CharacterController controller, Transform playerTransform)
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Update()
    {
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}
