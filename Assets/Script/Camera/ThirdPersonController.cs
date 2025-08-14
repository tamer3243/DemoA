using UnityEngine;

[RequireComponent(typeof(CameraEffect))]
public class ThirdPersonController: MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 3, -6);
    public float sensitivity = 3f;
    public float minY = -20f;
    public float maxY = 80f;

    private float yaw;
    private float pitch;

    private CameraEffect effectPipeline;

    private void Start()
    {
        yaw = target.eulerAngles.y;
        pitch = target.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        effectPipeline = GetComponent<CameraEffect>();

        var controller = target.GetComponent<CharacterController>();
        effectPipeline.AddEffect(new CameraViewBobbing(target, controller));
    }

    private void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * sensitivity;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        transform.position = target.position + rotation * offset;
        transform.rotation = rotation;
    }
}
