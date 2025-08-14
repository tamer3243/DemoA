using UnityEngine;

public class InputPc : Singleton<InputPc>, IPlayerInput
{
    public float GetHorizontal() => Input.GetAxis("Horizontal");
    public float GetVertical() => Input.GetAxis("Vertical");
    public bool IsJumpPressed() => Input.GetKeyDown(KeyCode.Space);
    public bool IsDashPressed() => Input.GetKeyDown(KeyCode.LeftShift);

    // 🔑 Thêm input bắn và đổi vũ khí
    public bool IsAttackPressed() => Input.GetMouseButton(1);
    public bool IsSwapPressed() => Input.GetKeyDown(KeyCode.Q);
   
}
