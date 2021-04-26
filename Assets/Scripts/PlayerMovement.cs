using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;

    Vector3 velocity;

    InputAction movement;
    InputAction rise;
    InputAction fall;

    void Start()
    {
        movement = new InputAction("PlayerMovement", binding: "<Gamepad>/leftStick");
        movement.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");

        movement.Enable();

        rise = new InputAction("Rise", binding: "<Gamepad>/q");
        rise.AddBinding("<Keyboard>/q");

        rise.Enable();

        fall = new InputAction("Fall", binding: "<Gamepad>/e");
        fall.AddBinding("<Keyboard>/e");

        fall.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float x;
        float y;
        float z;

        var delta = movement.ReadValue<Vector2>();
        x = delta.x;
        y = delta.y;

        float riseValue = rise.ReadValue<float>();
        float fallValue = fall.ReadValue<float>();

        z = riseValue - fallValue;

        Vector3 move = transform.right * x + transform.forward * y + transform.up * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
