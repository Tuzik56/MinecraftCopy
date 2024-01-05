using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController characterController;
    private Transform playerTransform;
    public float speedNormal = 8f;
    private float speed;
    public float speedSprint = 14f;
    public float jumpHeight = 1f;
    public float gravity = -9.8f;
    Vector3 velocity;

    private void Start()
    {
        speed = speedNormal;
        playerTransform = transform;
    }

    private void Update()
    {
        MovePlayer();
        PlayerGraviry();
    }

    public void MovePlayer()
    {
        if (velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = speedSprint;
        }
        else
        {
            speed = speedNormal;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerTransform.right * x + playerTransform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void PlayerGraviry()
    {
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
