using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float distance;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float mouseSensitivity = 2f;

    private float _yaw;
    private float _pitch;

    [NonSerialized] public Vector2 LookInput;

    private void LateUpdate()
    {
        _yaw += LookInput.x * mouseSensitivity;
        _pitch -= LookInput.y * mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, -60f, 60f);

        var rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        transform.position = player.position + rotation * offset;
        transform.rotation = rotation;
    }


    private void FixedUpdate()
    {
        var newPosition = new Vector3(player.position.x + offset.x, player.position.y + offset.y,
            player.position.z + offset.z);
        transform.position = newPosition;
    }
}