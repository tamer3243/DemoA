using UnityEngine;

public interface ICharacterController
{
    Vector3 GetMovementInput();
    float GetRotationInput();
}
