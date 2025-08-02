using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KeyboardController : MonoBehaviour, ICharacterController
{
    private JumpAbility jump;
    private DashAbility dash;
    private CharacterController controller;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        jump = GetComponent<JumpAbility>();
        dash = GetComponent<DashAbility>();
    }

    public Vector3 GetMovementInput()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            jump.Use(controller, transform);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            dash.Use(controller, transform);

        return new Vector3(h, 0, v);
    }

    public float GetRotationInput()
    {
        return Input.GetAxis("Mouse X");
    }
}
