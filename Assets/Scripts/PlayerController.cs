using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private PlayerCamera camera;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnMove(InputValue value)
    {
        movement.MoveInput = value.Get<Vector2>();
        Debug.Log($"OnMove called: {movement.MoveInput}");
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Jump");
        }
    }

    private void OnLook(InputValue value)
    {
        camera.LookInput = value.Get<Vector2>();
        Debug.Log(value.Get<Vector2>());
    }
}