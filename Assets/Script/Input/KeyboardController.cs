using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class KeyboardController : MonoBehaviour, ICharacterController
{
    private JumpAbility jump;
    private DashAbility dash;
    private CharacterController controller;
    private IPlayerInput input;

    private void Awake()
    {
        input = GetComponent<IPlayerInput>();
        controller = GetComponent<CharacterController>();
        jump = GetComponent<JumpAbility>();
        dash = GetComponent<DashAbility>();
    }

    public Vector3 GetMovementInput()
    {
        float h = input.GetHorizontal();
        float v = input.GetVertical();

        if ( input.IsJumpPressed())
            jump.Use(controller);

        if (input.IsDashPressed())
            dash.Use(controller, transform);

        return new Vector3(h, 0, v);
    }

    public float GetRotationInput()
    {
        return input.GetHorizontal();
    }
}
