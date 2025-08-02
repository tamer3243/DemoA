using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DashAbility : AbilityBase
{
    public float dashDistance = 8f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 1f;

    private CharacterController controller;
    private float lastDashTime;
    private bool isDashing;
    private float dashStartTime;
    private Vector3 dashDirection;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public override void Use(CharacterController controller, Transform playerTransform)
    {
        if (Time.time >= lastDashTime + dashCooldown && !isDashing)
        {
            // Dash theo hướng đang đi hoặc hướng nhìn
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            dashDirection = input.magnitude > 0.1f
                ? (Camera.main.transform.forward * input.z + Camera.main.transform.right * input.x).normalized
                : playerTransform.forward;

            isDashing = true;
            dashStartTime = Time.time;
            lastDashTime = Time.time;
        }
    }

    private void Update()
    {
        if (isDashing)
        {
            float elapsed = Time.time - dashStartTime;
            float dashSpeed = dashDistance / dashDuration;

            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            // Không để gravity kéo xuống khi dash
            if (controller.isGrounded == false)
            {
                controller.Move(Vector3.up * -2f * Time.deltaTime);
            }

            if (elapsed >= dashDuration)
            {
                isDashing = false;
            }
        }
    }
}
