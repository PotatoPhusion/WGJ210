using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;

    private CharacterController controller;
    private float h_input = 0f;
    private bool jump = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        h_input = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(h_input * moveSpeed, jump);
        jump = false;
    }
}
