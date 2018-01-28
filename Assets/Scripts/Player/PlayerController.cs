using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed = 2.0f;
    public float maxSpeed = 5.0f;
    private Vector3 jump;
    public float jumpForce = 2.0f;
    public float dashSpeed = 4;

    public GameObject gameModel;

    public GameObject teleportObject;

    public Animator cameraAnim;
    

    [Range(0, 100)]
    public float dashCharge;
    bool isCharging = false;

    bool isGrounded;

    Rigidbody rb;
	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        dashCharge = 100;
	}

    void Update()
    {
        if(dashCharge < 100 && !isCharging)
        {
            InvokeRepeating("Charge", 2.0f, 0.5f);
            isCharging = true;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float h = speed * Input.GetAxis("Horizontal");
        float v = speed * Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(h, 0.0f, v);

        rb.AddForce(movement * speed);

        if (rb.velocity.z > maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, maxSpeed);
        }

        if (rb.velocity.z < -maxSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -maxSpeed);
        }

        if (rb.velocity.x > maxSpeed)
        {
            rb.velocity = new Vector3(maxSpeed, rb.velocity.y, rb.velocity.z);
        }

        if (rb.velocity.x < -maxSpeed)
        {
            rb.velocity = new Vector3(-maxSpeed, rb.velocity.y, rb.velocity.z);
        }

        float Angle = 10;
        Vector3 Axis = Vector3.Cross(movement.normalized, transform.up);
        gameModel.transform.Rotate(Axis, Angle);

        if (!isGrounded && rb.velocity.y == 0)
        {
            isGrounded = true;
        }

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetButtonDown("Dash") && dashCharge > 0 && dashCharge >= 25) Dash();
    }

    void Dash()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //movement = Vector3.ClampMagnitude(movement, 1);

        Instantiate(teleportObject, new Vector3( transform.position.x , 1.0f, transform.position.z), Quaternion.Euler(90 , 20, 50));

        transform.position = transform.position + movement * dashSpeed;

        if(transform.position.x > 4.3f)
        {
            transform.position = new Vector3(4.0f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -4.3f)
        {
            transform.position = new Vector3(-4.0f, transform.position.y, transform.position.z);
        }

        dashCharge -= 25;

        Instantiate(teleportObject, new Vector3(transform.position.x, 1.0f, transform.position.z), Quaternion.Euler(90, 20, 50));


    }

    void Charge()
    {
        if (dashCharge < 100)
            dashCharge += 0.25f;
        else
        {
            isCharging = false;
            CancelInvoke("Charge");
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ground")
        {
            PlatformController.moving = false;
            cameraAnim.SetTrigger("Top");
            other.gameObject.GetComponent<ObstacleSpawning>().backWall.SetActive(true);
        }
    }

}
