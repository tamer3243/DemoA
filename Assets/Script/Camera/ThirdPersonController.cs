using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [Header("Camera Follow")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float sensitivity = 3f;
    public float minY = -20f;
    public float maxY = 80f;

    [Header("View Bobbing")]
    public CharacterController playerController;
    public float bobFrequency = 12f;
    public float bobAmplitude = 0.3f;
    public float rotationAmplitude = 6f;
    public float bobSmoothing = 10f;

    private float yaw;
    private float pitch;
    private float bobTimer;
    private Vector3 currentLocalPos;
    private Quaternion currentLocalRot;
    private Vector3 lastPlayerPos;

    // Debug
    private Vector3 debugBobDelta;

    private void Start()
    {
        yaw = target.eulerAngles.y;
        pitch = target.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentLocalPos = offset;
        currentLocalRot = Quaternion.identity;
        lastPlayerPos = playerController.transform.position;
    }

    private void LateUpdate()
    {
        // --- Camera rotation ---
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // --- Manual speed calculation ---
        Vector3 displacement = playerController.transform.position - lastPlayerPos;
        float speed = new Vector2(displacement.x, displacement.z).magnitude / Time.deltaTime;
        lastPlayerPos = playerController.transform.position;

        float normalizedSpeed = Mathf.Clamp01(speed / 6f);

        if (normalizedSpeed > 0.05f && playerController.isGrounded)
        {
            bobTimer += Time.deltaTime * bobFrequency * normalizedSpeed;

            float bobOffsetY = Mathf.Sin(bobTimer) * bobAmplitude;
            float bobOffsetX = Mathf.Cos(bobTimer * 0.5f) * bobAmplitude * 0.6f;

            debugBobDelta = new Vector3(bobOffsetX, bobOffsetY, 0);

            Quaternion rollDelta = Quaternion.Euler(0, 0, Mathf.Sin(bobTimer) * rotationAmplitude);

            currentLocalPos = Vector3.Lerp(currentLocalPos, offset + debugBobDelta, Time.deltaTime * bobSmoothing);
            currentLocalRot = Quaternion.Slerp(currentLocalRot, rollDelta, Time.deltaTime * bobSmoothing);
        }
        else
        {
            bobTimer = 0;
            debugBobDelta = Vector3.zero;
            currentLocalPos = Vector3.Lerp(currentLocalPos, offset, Time.deltaTime * bobSmoothing);
            currentLocalRot = Quaternion.Slerp(currentLocalRot, Quaternion.identity, Time.deltaTime * bobSmoothing);
        }

        transform.position = target.position + rotation * currentLocalPos;
        transform.rotation = rotation * currentLocalRot;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // Vẽ sphere tại vị trí bobbing hiện tại (offset gốc + delta)
            Gizmos.color = Color.cyan;
            Vector3 bobWorldPos = target.position + Quaternion.Euler(pitch, yaw, 0) * (offset + debugBobDelta);
            Gizmos.DrawSphere(bobWorldPos, 0.1f);

            // Vẽ line từ offset gốc -> bobbing
            Gizmos.color = Color.yellow;
            Vector3 baseWorldPos = target.position + Quaternion.Euler(pitch, yaw, 0) * offset;
            Gizmos.DrawLine(baseWorldPos, bobWorldPos);
        }
    }
}
