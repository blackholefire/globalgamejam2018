using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    public float speed = 2.0f;
    public float maxSpeed = 5.0f;
    private Vector3 jump;
    public float jumpForce = 2.0f;
    public float dashSpeed = 4;

    public GameObject gameModel;

    public GameObject teleportObject;

    public Animator cameraAnim;

    public int lives = 5;

    public GameObject deathCollider;

    //Death Stuff
    public GameObject lastCheckPoint;
    public GameObject curLevel;

    public Image dashFill;
    public Text healthNum;
    

    [Range(0, 100)]
    public float dashCharge;
    bool isCharging = false;

    bool isGrounded;

    Rigidbody rb;
    bool alive = true;
    bool killed = false;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        dashCharge = 100;

        lastCheckPoint = Instantiate(new GameObject("Checkpoint"), transform.position, Quaternion.identity);
        lastCheckPoint.transform.parent = curLevel.transform;
    }

    void Update()
    {
        if(dashCharge < 100 && !isCharging)
        {
            InvokeRepeating("Charge", 2.0f, 0.5f);
            isCharging = true;
        }
        dashFill.fillAmount = dashCharge / 100;
        healthNum.text = lives.ToString();
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
            dashCharge += 1.5f;
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
            other.transform.parent.gameObject.GetComponent<ObstacleSpawning>().backWall.SetActive(true);
            deathCollider.SetActive(false);
            other.transform.parent.gameObject.GetComponent<ObstacleSpawning>().SpawnNext();
        }
        if (other.gameObject.tag == "Death" && alive)
        {
            //alive = false;
            lives--;
            transform.position = lastCheckPoint.transform.position;
            dashCharge = 100;
            killed = true;

        }
        if (other.gameObject.tag == "Death" && !alive)
        {
            alive = true;
        }

        if (other.gameObject.tag == "Win")
        {
            SceneManager.LoadScene("winMenu");
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PuzzlePiece>())
        {
            if(Input.GetButtonDown("Action"))
                other.gameObject.transform.Rotate(0, other.gameObject.transform.rotation.y + 45, 0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            lastCheckPoint.transform.position = transform.position;
            cameraAnim.SetTrigger("Follow");
            lastCheckPoint.transform.parent = curLevel.transform;
            other.isTrigger = false;

            deathCollider.SetActive(true);

            PlatformController.moving = true;
        }

        if (other.gameObject.tag == "Death" && alive)
        {
            alive = false;
        }

        if (other.gameObject.tag == "Death" && !alive)
        {
            alive = true;
        }
    }

    void Death()
    {
        // Load Death Scene Here
    }

}
