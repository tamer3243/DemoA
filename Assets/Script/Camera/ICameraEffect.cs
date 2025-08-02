
using UnityEngine;

public interface ICameraEffect
{

    void ApplyEffect(ref Vector3 position, ref Quaternion rotation, float deltaTime);
}
