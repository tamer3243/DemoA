using UnityEngine;

public interface ICameraController
{
    void CalculateBaseTransform(out Vector3 position, out Quaternion rotation);
}
