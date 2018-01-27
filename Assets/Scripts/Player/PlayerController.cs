using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 2.0f;
    private Vector3 jump;
    public float jumpForce = 2.0f;
    public float dashSpeed = 4;

    bool isGrounded;

    Rigidbody rb;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float h = speed * Input.GetAxis("Horizontal");
        float v = speed * Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0.0f, v);

        transform.Translate(movement);

        if (!isGrounded && rb.velocity.y == 0)
        {
            isGrounded = true;
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetButtonDown("Dash")) Dash();
    }

    void Dash()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //movement = Vector3.ClampMagnitude(movement, 1);

        transform.position = transform.position + movement * dashSpeed;

        if(transform.position.x > 4.3f)
        {
            transform.position = new Vector3(4.0f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -4.3f)
        {
            transform.position = new Vector3(-4.0f, transform.position.y, transform.position.z);
        }

    }

}
