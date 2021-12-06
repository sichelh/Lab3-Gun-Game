using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 10.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;
    public float pushPower = 2.0f;

    // needed for jumping and falling
    private Vector3 velocity;
    private bool isGrounded;

    void Update()
    {
        // are we on the ground?
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f; // reset the velocity, force the player on the ground.
        }

        // read WASD input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // an arrow pointing at the direction where we want to move.
        // use local coordinates.
        Vector3 direction = transform.right * x + transform.forward * z;

        // move via character controller. 
        // Use deltatime to make it independent of the framerate
        controller.Move(direction * speed * Time.deltaTime);

        

        // jump, but only if we are on the ground
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Jump velocity needed to jump a certain height: sqrt(h * -2*g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime; // increase velocity by gravity when falling
        controller.Move(velocity * Time.deltaTime); // move by velocity2

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Pin"))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if(rb == null || rb.isKinematic)
            {
                return;
            }

            //no pushing downwards
            if(hit.moveDirection.y < -0.3)
            {
                return;
            }

            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            rb.velocity = pushDirection * pushPower;
        }
    }
}

