using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private ICharacterController inputController;
    private CharacterController controller;     
    private Transform cam;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputController = GetComponent<ICharacterController>();
        cam = Camera.main.transform;
    }

    private void Update()
    {
        Vector3 input = inputController.GetMovementInput();
        Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 move = camForward * input.z + cam.right * input.x;

        controller.Move(move * moveSpeed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
