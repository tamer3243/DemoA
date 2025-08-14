using UnityEngine;

public class CameraViewBobbing : ICameraEffect
{
    private readonly Transform player;
    private readonly CharacterController controller;

    private float bobTimer;
    private Vector3 lastPlayerPos;

    public float bobFrequency = 12f;
    public float bobAmplitude = 0.3f;
    public float rotationAmplitude = 6f;
    public float bobSmoothing = 10f;

    // 🔑 Lưu lại base offset (ban đầu)
    private readonly Vector3 baseOffset;

    public CameraViewBobbing(Transform playerTransform, CharacterController charController)
    {
        player = playerTransform;
        controller = charController;
        lastPlayerPos = playerTransform.position;

        // Base offset mặc định = (0,0,0) vì position đã follow
        baseOffset = Vector3.zero;
    }

    public void ApplyEffect(ref Vector3 position, ref Quaternion rotation, float deltaTime)
    {
        // Tính tốc độ
        Vector3 displacement = player.position - lastPlayerPos;
        float speed = new Vector2(displacement.x, displacement.z).magnitude / deltaTime;
        lastPlayerPos = player.position;

        float normalizedSpeed = Mathf.Clamp01(speed / 6f);

        // Bắt đầu bobbing
        Vector3 bobDelta = Vector3.zero;
        Quaternion rollDelta = Quaternion.identity;

        if (normalizedSpeed > 0.05f && controller.isGrounded)
        {
            bobTimer += deltaTime * bobFrequency * normalizedSpeed;

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.6f;

            bobDelta = new Vector3(bobOffsetX, bobOffsetY, 0);
            rollDelta = Quaternion.Euler(0, 0, Mathf.Sin(bobTimer) * rotationAmplitude);
        }
        else
        {
            bobTimer = 0;
        }

        // 🔥 Dùng baseOffset thay vì cộng dồn position
        Vector3 targetPos = baseOffset + bobDelta;
        position = Vector3.Lerp(position, position + targetPos, deltaTime * bobSmoothing);
        rotation = Quaternion.Slerp(rotation, rotation * rollDelta, deltaTime * bobSmoothing);
    }
}
