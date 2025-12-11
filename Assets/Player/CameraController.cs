using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float speed = 20f;

    private PlayerInput input;

    InputAction Movement;

    private void OnEnable()
    {
        input = GetComponent<PlayerInput>();
        input.enabled = true;
        Movement = input.actions["Player/Move"];
    }

    private void OnDisable()
    {
        input.enabled = false;
    }

    void LateUpdate()
    {
        Vector2 moveInput = Movement.ReadValue<Vector2>();

        this.transform.position += new Vector3(moveInput.x, 0 , moveInput.y) * speed * Time.deltaTime;
    }
}
