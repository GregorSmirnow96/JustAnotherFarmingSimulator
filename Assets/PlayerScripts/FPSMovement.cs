using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMovement : MonoBehaviour
{
    public float horizontalMouseSensitivity = 300f;
    public float verticalMouseSensitivity = 120f;


    private CharacterController characterController;
    private Transform cameraTransform;
    private float xRotation = 0f;

    private float movementSpeed;

    void Start()
    {
        PlayerStats.instance.RegisterOnMovementSpeedChangeCallback((newValue) => movementSpeed = newValue);

        // Get the CharacterController component attached to the player
        characterController = GetComponent<CharacterController>();

        // Get the main camera's transform
        cameraTransform = Camera.main.transform;

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle mouse movement for camera rotation
        RotateCamera();

        // Handle player movement using WASD keys
        MovePlayer();
    }

    void RotateCamera()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * horizontalMouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * verticalMouseSensitivity * Time.deltaTime;

        // Adjust the xRotation based on mouse Y input
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the camera up and down (pitch)
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the player left and right (yaw) based on mouse X input
        transform.Rotate(Vector3.up * mouseX);
    }

    void MovePlayer()
    {
        // Get input from WASD keys
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (moveZ < 0)
        {
            moveZ = moveZ * 0.6f;
        }

        // Create a movement vector based on input and movementSpeed
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = -9.81f;

        // Move the player using the CharacterController
        characterController.Move(move * movementSpeed * Time.deltaTime);
    }
}
