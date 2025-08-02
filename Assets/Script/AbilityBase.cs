using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public abstract void Use(CharacterController controller, Transform playerTransform);
}
