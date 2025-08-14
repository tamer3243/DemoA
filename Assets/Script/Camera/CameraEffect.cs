using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class CameraEffect : MonoBehaviour
{
    private readonly List<ICameraEffect> effects = new List<ICameraEffect>();
    private Transform camTransform;

    private void Awake()
    {
        camTransform = transform;
    }

    public void AddEffect(ICameraEffect effect) => effects.Add(effect);

    private void LateUpdate()
    {
        float deltaTime = Time.deltaTime;

        Vector3 finalPos = camTransform.localPosition;
        Quaternion finalRot = camTransform.localRotation;

        foreach (var effect in effects)
            effect.ApplyEffect(ref finalPos, ref finalRot, deltaTime);

        camTransform.localPosition = finalPos;
        camTransform.localRotation = finalRot;
    }
}
