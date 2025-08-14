using UnityEngine;

public class JumpAbility : MonoBehaviour
{


    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float fallMultiplier = 2f; // rơi nhanh hơn
    public float verticalVelocity;

    public void Use(CharacterController controller)
    {
        // Khi bắt đầu nhảy → set vận tốc lên
        verticalVelocity = jumpForce;
    }

    public void ApplyJump(CharacterController controller)
    {
        if (!controller.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else if (verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Giữ nhân vật dính đất
        }

        // Di chuyển nhân vật theo vận tốc dọc
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }

    public bool IsFalling()
    {
        return verticalVelocity <= 0;
    }
}
